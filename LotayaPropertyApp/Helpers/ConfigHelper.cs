using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace LotayaPropertyApp.Helpers
{
    public class ConfigHelper
    {
        public static string LotayaApiUrl
        {
            get { return "http://lotayaapi.harveynetwork.com/"; }
        }

        public static string PropertyFeedApiUrl
        {
            get { return "http://lotayaapi.harveynetwork.com/api/propertyfeed"; }
        }

        public static string PropertyFeedPagingApiUrl
        {
            get { return "http://lotayaapi.harveynetwork.com/api/propertyfeed/GetLatestProperties?skip={0}&take={1}"; }
        }

        public static string ApiUsername
        {
            get { return "tminhein@gmail.com"; }
        }

        public static string ApiPassword
        {
            get { return "lotayaproperty"; }
        }
    }
}