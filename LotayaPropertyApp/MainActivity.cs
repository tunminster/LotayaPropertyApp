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

            var metrics = Resources.DisplayMetrics;
            

            lv = FindViewById<ListView>(Resource.Id.lv);

            propertyFeedModels = GetPropertyFeedModels();

            //maxPosition = propertyFeedModels.Count;
            maxPosition = 20;

            Button btnLoadMore = new Button(this);
            btnLoadMore.Text = "Load More";
            btnLoadMore.Click += BtnLoadMore_Click;
            lv.AddFooterView(btnLoadMore);


            adapter = new CustomAdapter(this, Resource.Layout.PropertyFeedModel, Resource.Id.tvTitle2, propertyFeedModels, metrics.WidthPixels);

            lv.Adapter = adapter;


            lv.ItemClick += lv_ItemClick;

            AppCenter.Start("9e333419-7601-418f-b781-ecaebcbeaed9",
                   typeof(Analytics), typeof(Crashes));
            AppCenter.Start("9e333419-7601-418f-b781-ecaebcbeaed9", typeof(Analytics), typeof(Crashes));


        }

        private void BtnLoadMore_Click(object sender, EventArgs e)
        {
            var metrics = Resources.DisplayMetrics;

            RunOnUiThread(() =>
            {
                var result = GetPropertyFeedModels(maxPosition,10);
                propertyFeedModels.AddRange(result);
                adapter = new CustomAdapter(this, Resource.Layout.PropertyFeedModel, Resource.Id.tvTitle2, propertyFeedModels, metrics.WidthPixels);
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
            var propertyList = _lotayaApiService.GetPropertyFeedList(skip, take);
            return propertyList;
        }


        private List<PropertyFeedModel> GetPropertyFeedModels()
        {
            var propertyList = _lotayaApiService.GetPropertyFeedList(0,10);
            return propertyList;
        }

        

        //private Bitmap GetImageBitmapFromUrl(string url)
        //{
        //    Bitmap imageBitmap = null;

        //    using (var webClient = new WebClient())
        //    {
        //        var imageBytes = webClient.DownloadData(url);
        //        if (imageBytes != null && imageBytes.Length > 0)
        //        {
        //            imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
        //        }
        //    }

        //    return imageBitmap;
        //}
    }
}

