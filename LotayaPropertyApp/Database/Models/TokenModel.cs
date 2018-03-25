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
using Newtonsoft.Json;
using SQLite;

namespace LotayaPropertyApp.Database.Models
{
    public class TokenModel
    {
        [PrimaryKey]
        public string access_token { get; set; }
        public string token_type { get; set; }
        [JsonProperty(PropertyName = ".expires_in")]
        public string expires_in { get; set; }
        public string userName { get; set; }
        public string issued { get; set; }
        [JsonProperty(PropertyName = ".expires")]
        public DateTime expires { get; set; }
    }
}