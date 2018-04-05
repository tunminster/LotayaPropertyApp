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
using LotayaPropertyApp.Helpers;
using LotayaPropertyApp.Models;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Org.Json;

namespace LotayaPropertyApp.Services
{
    public class LotayaApiService : ILotayaApiService
    {
        public List<PropertyFeedModel> GetPropertyFeedList(string token)
        {
            var result = FetchData<List<PropertyFeedModel>>(ConfigHelper.PropertyFeedApiUrl, token, "GET");
            return result;
        }

        private T FetchData<T>(string url, string token, string requestMethod)
        {
            Analytics.TrackEvent(url);
            try
            {
                // Create an HTTP web request using the URL:
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = requestMethod;
                request.Headers.Add("Authorization", "bearer " + token);
                

                using (WebResponse response = request.GetResponse())
                {

                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        var model = JsonConvert.DeserializeObject<T>(sr.ReadToEnd());

                        return model;
                    }

                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return default(T);
        }
    }
}