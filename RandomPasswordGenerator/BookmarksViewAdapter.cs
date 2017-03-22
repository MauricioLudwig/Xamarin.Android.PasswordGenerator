using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RandomPasswordGenerator.Models;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RandomPasswordGenerator
{
    class BookmarksViewAdapter : BaseAdapter<Bookmark>
    {

        public List<Bookmark> bookmarks;
        private Context context;

        public BookmarksViewAdapter(Context context, List<Bookmark> bookmarks)
        {
            this.context = context;
            this.bookmarks = bookmarks;
        }

        public override int Count
        {
            get { return bookmarks.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Bookmark this[int position]
        {
            get { return bookmarks[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(context).Inflate(Resource.Layout.listview_row, null);
            }

            TextView txtSource = view.FindViewById<TextView>(Resource.Id.txtSource);
            TextView txtPassword = view.FindViewById<TextView>(Resource.Id.txtPassword);

            return view;
        }
    }
}