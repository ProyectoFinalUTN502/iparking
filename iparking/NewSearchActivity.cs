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
    [Activity(Label = "NewSearchActivity", Theme = "@style/MyTheme.Base")]
    public class NewSearchActivity : Activity
    {
        ImageView mBack;
        Button mSearch;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.NewSearch);

            mBack = FindViewById<ImageView>(Resource.Id.imageViewBack);
            mSearch = FindViewById<Button>(Resource.Id.btnNewSearch);

            mBack.Click += MBack_Click;
            mSearch.Click += MSearch_Click;
        }

        private void MSearch_Click(object sender, EventArgs e)
        {
            
        }

        private void MBack_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MoreOptionsActivity));
            this.StartActivity(intent);
            this.Finish();
        }
    }
}