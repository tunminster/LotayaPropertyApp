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

namespace LotayaPropertyApp.Holders
{
    public class PropertyFeedHolder
    {
        public TextView txtTitle;
        public ImageView ivImage;

        public PropertyFeedHolder(View itemView)
        {
            txtTitle = itemView.FindViewById<TextView>(Resource.Id.tvTitle);
            ivImage = itemView.FindViewById<ImageView>(Resource.Id.ivFeedImage);
        }
    }
}