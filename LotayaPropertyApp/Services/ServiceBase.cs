using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LotayaPropertyApp.Database;
using LotayaPropertyApp.Database.Models;
using LotayaPropertyApp.Helpers;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;

namespace LotayaPropertyApp.Services
{
    public abstract class ServiceBase
    {
        private AppDbContext _db;
        public ServiceBase()
        {
            _db = new AppDbContext();
        }

        public T FetchData<T>(string url, string token, string requestMethod)
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

        public TokenModel GetToken()
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
    }
}