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

namespace LotayaPropertyApp.Adapters
{
    public abstract class CustomScrollListener : Java.Lang.Object, AbsListView.IOnScrollListener
    {
        private int visibleThreshold = 5;
        private int currentPage = 0;
        private int previousTotalItemCount = 0;
        private bool loading = true;
        private int startingPageIndex = 0;
        
        public CustomScrollListener() { }

        public CustomScrollListener(int visibleThreshold)
        {
            this.visibleThreshold = visibleThreshold;
        }

        public CustomScrollListener(int visibleThreshold, int startPage)
        {
            this.visibleThreshold = visibleThreshold;
            this.startingPageIndex = startPage;
            this.currentPage = startPage;
        }

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            // If the total item count is zero and the previous isn't, assume the
            // list is invalidated and should be reset back to initial state
            if (totalItemCount < previousTotalItemCount)
            {
                this.currentPage = this.startingPageIndex;
                this.previousTotalItemCount = totalItemCount;

                if(totalItemCount == 0)
                {
                    this.loading = true;
                }
            }

            // If it's still loading, check to see if the dataset count has changed.
            // If so conclude it has finished loading and update the current number and total item count.
            if(loading && (totalItemCount > previousTotalItemCount))
            {
                loading = false;
                previousTotalItemCount = totalItemCount;
                currentPage++;
            }

            if(!loading && (firstVisibleItem + visibleItemCount + visibleThreshold) >= totalItemCount)
            {
                loading = OnLoadMore(currentPage + 1, totalItemCount);
            }

        }

        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState)
        {
            // no action
        }

        public abstract bool OnLoadMore(int page, int totalItemsCount);
    }
}