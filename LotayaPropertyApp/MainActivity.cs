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

namespace LotayaPropertyApp
{
    [Activity(Label = "LotayaPropertyApp", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private CustomAdapter adapter;
        private ListView lv;
        private List<PropertyFeedModel> propertyFeedModels;
        private AppDbContext _db;

        public MainActivity()
        {
            _db = new AppDbContext();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            lv = FindViewById<ListView>(Resource.Id.lv);

            propertyFeedModels = GetPropertyFeedModels();

            adapter = new CustomAdapter(this, Resource.Layout.PropertyFeedModel, Resource.Id.tvTitle2, propertyFeedModels);

            lv.Adapter = adapter;

            lv.ItemClick += lv_ItemClick;

            
        }

        private void lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, propertyFeedModels[e.Position].Title, ToastLength.Short).Show();
        }

        private List<PropertyFeedModel> GetPropertyFeedModels()
        {

            var token = GetToken("http://lotayaapi.harveynetwork.com/", "tminhein@gmail.com", "lotayaproperty");
            TokenModel tokenModel;

            tokenModel = JsonConvert.DeserializeObject<TokenModel>(token);

            //string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "lotayaApp.db3");
            //var db = new SQLiteConnection(dbPath);
            //db.CreateTable<TokenModel>();
            //db.Insert(tokenModel);


            //var data = db.Table<TokenModel>();
            _db.DeleteModel<TokenModel>();
            _db.Insert(tokenModel);

            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "lotayaApp.db3");
            var db = new SQLiteConnection(dbPath);
            var data = db.Table<TokenModel>();

            var result = data.FirstOrDefault();

            if(result.expires > DateTime.Now)
            {
                var test = "Live";
            }
            else
            {
                var test = "expired";
            }





            var propertyFeedModels = new List<PropertyFeedModel>();
            var model1 = new PropertyFeedModel();
            model1.Id = 123;
            model1.Title = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            model1.Image1 = "https://scontent.xx.fbcdn.net/v/t1.0-9/s720x720/29496520_1079882905487743_2733123361083044221_n.jpg?_nc_cat=0&oh=043b2ab85bd7b0ae248063644c3fc0d3&oe=5B29164E";
            model1.Message = "Hello Text";

            var model2 = new PropertyFeedModel();
            model2.Id = 456;
            model2.Title = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source.Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of de Finibus Bonorum et Malorumby Cicero, written in 45 BC.This book is a treatise on the theory of ethics, very popular during the Renaissance.The first line of Lorem Ipsum, comes from a line in section 1.10.32.";
            model2.Image1 = "https://scontent.xx.fbcdn.net/v/t1.0-0/p180x540/29468019_1039437996205170_6787169052151250944_n.jpg?oh=21a4233b213926f45535f28d584daf50&oe=5B75C1F0";
            model2.Message = "Hello Text 456";

            propertyFeedModels.Add(model1);
            propertyFeedModels.Add(model2);



            return propertyFeedModels;
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

                //WebClient webClient = new WebClient();


                using (var client = new HttpClient())
                {
                    var response = client.PostAsync(url + "Token", content).Result;

                    return response.Content.ReadAsStringAsync().Result;
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

