using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace LotayaPropertyApp.Models
{
    public class CustomAdapter : ArrayAdapter
    {
        private Context _context;
        private List<PropertyFeedModel> propertyFeeds;
        private int resource;
        private LayoutInflater inflater;

        public CustomAdapter(Context context, int resource, int textViewResourceId, List<PropertyFeedModel> objects) : base(context, resource, textViewResourceId, objects)
        {
            this._context = context;
            this.resource = resource;
            this.propertyFeeds = objects;
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
            if(inflater == null)
            {
                inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
            }

            if(convertView == null)
            {
                convertView = inflater.Inflate(resource, parent, false);
            }

            // bind data
            MyHolder holder = new MyHolder(convertView);
            holder.txtTitle.Text = propertyFeeds[position].Title;
            holder.ivImage.SetImageBitmap(propertyFeeds[position].Image1);

            convertView.SetBackgroundColor(Color.AliceBlue);

            return base.GetView(position, convertView, parent);
        }
    }
}