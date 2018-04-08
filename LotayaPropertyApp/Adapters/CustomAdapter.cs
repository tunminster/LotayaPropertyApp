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
using LotayaPropertyApp.Models;
using Android.Graphics;
using Square.Picasso;
using LotayaPropertyApp.Holders;
using static Android.InputMethodServices.InputMethodService;


namespace LotayaPropertyApp.Adapters
{
    public class CustomAdapter : ArrayAdapter
    {
        private Context _context;
        private List<PropertyFeedModel> propertyFeeds;
        private int resource;
        private LayoutInflater inflater;
        private int _deviceWidth;

        public CustomAdapter(Context context, int resource, int textViewResourceId, List<PropertyFeedModel> objects, int deviceWidth) : base(context, resource, textViewResourceId, objects)
        {
            this._context = context;
            this.resource = resource;
            this.propertyFeeds = objects;
            this._deviceWidth = deviceWidth;
        }

        //public CustomAdapter(Context context, int resource, List<PropertyFeedModel> objects) : base(context, resource, objects)
        //{
        //    this._context = context;
        //    this.resource = resource;
        //    this.propertyFeeds = objects;
        //}

        //public CustomAdapter(Context context, int resource, List<PropertyFeedModel> objects) 
        //    : base(context, resource, objects)
        //{
        //    this._context = context;
        //    this.resource = resource;
        //    this.propertyFeeds = objects;
        //}



        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (inflater == null)
            {
                inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
            }

            if (convertView == null)
            {
                convertView = inflater.Inflate(resource, parent, false);
            }

            // bind data
            PropertyFeedHolder holder = new PropertyFeedHolder(convertView);
            var font = Typeface.CreateFromAsset(Context.Assets, "ZawgyiOne2008.ttf");
            holder.txtTitle.Typeface = font;

            holder.txtTitle.Text = propertyFeeds[position].Title;
            //holder.ivImage.SetImageBitmap(propertyFeeds[position].Image1);



            //ImageService.Instance.LoadUrl(propertyFeeds[position].Image2).Into(holder.ivImage);
            //.Resize(600, 350)
            Picasso.With(Context).Load(propertyFeeds[position].Image1)
                .Resize(_deviceWidth - 10, 350)
                .Into(holder.ivImage);

            convertView.SetBackgroundColor(Color.AliceBlue);

            return base.GetView(position, convertView, parent);
        }
    }
}