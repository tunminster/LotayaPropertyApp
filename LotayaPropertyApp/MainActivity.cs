using Android.App;
using Android.Widget;
using Android.OS;
using LotayaPropertyApp.Models;
using System.Collections.Generic;
using System;
using Android.Graphics;
using System.Net;
using LotayaPropertyApp.Adapters;
using Org.Apache.Http.Client;
using System.Net.Http;
using SQLite;
using LotayaPropertyApp.Database.Models;
using Newtonsoft.Json;
using LotayaPropertyApp.Database;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.IO;
using LotayaPropertyApp.Services;
using LotayaPropertyApp.Helpers;
using System.Linq;

namespace LotayaPropertyApp
{
    [Activity(Label = "Lotaya Property", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private CustomAdapter adapter;
        private ListView lv;
        private List<PropertyFeedModel> propertyFeedModels;
        private AppDbContext _db;
        private ILotayaApiService _lotayaApiService;
        private int maxPosition;

        public MainActivity()
        {
            _db = new AppDbContext();
            _lotayaApiService = new LotayaApiService();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            lv = FindViewById<ListView>(Resource.Id.lv);

            propertyFeedModels = GetPropertyFeedModels();

            //maxPosition = propertyFeedModels.Count;
            maxPosition = 20;

            Button btnLoadMore = new Button(this);
            btnLoadMore.Text = "Load More";
            btnLoadMore.Click += BtnLoadMore_Click;
            lv.AddFooterView(btnLoadMore);


            adapter = new CustomAdapter(this, Resource.Layout.PropertyFeedModel, Resource.Id.tvTitle2, propertyFeedModels);

            lv.Adapter = adapter;


            lv.ItemClick += lv_ItemClick;

            AppCenter.Start("9e333419-7601-418f-b781-ecaebcbeaed9",
                   typeof(Analytics), typeof(Crashes));
            AppCenter.Start("9e333419-7601-418f-b781-ecaebcbeaed9", typeof(Analytics), typeof(Crashes));


        }

        private void BtnLoadMore_Click(object sender, EventArgs e)
        {
            RunOnUiThread(() =>
            {
                var result = GetPropertyFeedModels(maxPosition,20);
                propertyFeedModels.AddRange(result);
                adapter = new CustomAdapter(this, Resource.Layout.PropertyFeedModel, Resource.Id.tvTitle2, propertyFeedModels);
                lv.Adapter = adapter;

                int currentPosition = lv.FirstVisiblePosition;
                lv.SetSelectionFromTop(currentPosition + 1, 0);
                lv.SetSelection(maxPosition);

            });

            maxPosition = propertyFeedModels.Count;
        }

        private void lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, propertyFeedModels[e.Position].Title, ToastLength.Short).Show();
        }

        //Todo: get property by skip and take
        private List<PropertyFeedModel> GetPropertyFeedModels(int skip, int take)
        {
            TokenModel result = GetToken();
            return new List<PropertyFeedModel>();
        }


        private List<PropertyFeedModel> GetPropertyFeedModels()
        {
            TokenModel result = GetToken();

            var propertyList = _lotayaApiService.GetPropertyFeedList(result.access_token);
            return propertyList;

        }

        private TokenModel GetToken()
        {
            var data = _db.GetList<TokenModel>();
            var result = data.FirstOrDefault();

            if (result == null || result.expires < DateTime.Now || result.expires.Equals(DateTime.Now))
            {
                var token = GetToken(ConfigHelper.LotayaApiUrl, ConfigHelper.ApiUsername, ConfigHelper.ApiPassword);
                TokenModel tokenModel;

                tokenModel = JsonConvert.DeserializeObject<TokenModel>(token);
                _db.DeleteModel<TokenModel>();
                _db.Insert(tokenModel);

                data = _db.GetList<TokenModel>();
                result = data.FirstOrDefault();
            }

            return result;
        }

        private string GetToken(string url, string userName, string password)
        {
            try
            {
                var pairs = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>( "grant_type", "password" ),
                        new KeyValuePair<string, string>( "username", userName ),
                        new KeyValuePair<string, string> ( "Password", password )
                    };
                var content = new FormUrlEncodedContent(pairs);
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                string userData = string.Format("username={0}&Password={1}&grant_type=password", userName, password);

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url + "token"));
                request.Accept = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";

                using (var requestWriter = new StreamWriter(request.GetRequestStream()))
                {
                    requestWriter.Write(userData);
                    requestWriter.Close();
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        string result = sr.ReadToEnd();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            return string.Empty;

        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}

