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

namespace RandomPasswordGenerator.Models
{
    class Bookmark
    {
        public string Source { get; set; }
        public string Password { get; set; }
    }
}