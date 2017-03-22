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

namespace RandomPasswordGenerator
{
    [Activity(Label = "BookmarksActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class BookmarksActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Layout set to BookmarksScreen
            SetContentView(Resource.Layout.listview_row);

            // Toolbar declaration
            var toolbarBookmarks = FindViewById<Toolbar>(Resource.Id.toolbarBookmarks);
            SetActionBar(toolbarBookmarks);
            ActionBar.Title = "Bookmarks";
        }

    }
}