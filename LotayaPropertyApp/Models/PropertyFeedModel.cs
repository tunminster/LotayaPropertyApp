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

namespace LotayaPropertyApp.Models
{
    public class PropertyFeedModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Android.Graphics.Bitmap Image1 { get; set; }  
        public string Image2 { get; set; }
    }
}