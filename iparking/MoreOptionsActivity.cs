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

namespace iparking
{
    [Activity(Label = "MoreOptionsActivity", Theme = "@style/MyTheme.Base", NoHistory = false)]
    public class MoreOptionsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MoreOptions);
        }
    }
}