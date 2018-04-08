using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LotayaPropertyApp.Database.Models;
using LotayaPropertyApp.Helpers;
using LotayaPropertyApp.Models;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Org.Json;

namespace LotayaPropertyApp.Services
{
    public class LotayaApiService : ServiceBase, ILotayaApiService
    {
        private TokenModel tokenModel;

        public LotayaApiService()
        {
            tokenModel = GetToken();
        }

        public List<PropertyFeedModel> GetPropertyFeedList()
        {
            var result = FetchData<List<PropertyFeedModel>>(ConfigHelper.PropertyFeedApiUrl, tokenModel.access_token, "GET");
            return result;
        }

        public List<PropertyFeedModel> GetPropertyFeedList(int skip, int take)
        {
            var result = FetchData<List<PropertyFeedModel>>(string.Format(ConfigHelper.PropertyFeedPagingApiUrl, skip, take), tokenModel.access_token, "GET");
            return result;
        }

        //private T FetchData<T>(string url, string token, string requestMethod)
        //{
        //    Analytics.TrackEvent(url);
        //    try
        //    {
        //        // Create an HTTP web request using the URL:
        //        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
        //        request.ContentType = "application/json";
        //        request.Method = requestMethod;
        //        request.Headers.Add("Authorization", "bearer " + token);
                

        //        using (WebResponse response = request.GetResponse())
        //        {

        //            using (var sr = new StreamReader(response.GetResponseStream()))
        //            {
        //                var model = JsonConvert.DeserializeObject<T>(sr.ReadToEnd());

        //                return model;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Crashes.TrackError(ex);
        //    }
        //    return default(T);
        //}
    }
}