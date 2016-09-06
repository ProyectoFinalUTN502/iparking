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
        ImageView mBack;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MoreOptions);

            mBack = FindViewById<ImageView>(Resource.Id.imageViewBack);

            mBack.Click += MBack_Click;
        }

        private void MBack_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent);
            this.Finish();
        }
    }
}