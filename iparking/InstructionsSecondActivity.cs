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
    [Activity(Label = "InstructionsSecondActivity", Theme = "@style/MyTheme.Base")]
    public class InstructionsSecondActivity : Activity
    {
        Button mButtonSecond;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.InstructionsSecond);

            mButtonSecond = FindViewById<Button>(Resource.Id.buttonSecond);
            mButtonSecond.Click += MButtonSecond_Click;
        }

        private void MButtonSecond_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(InstructionsThirdActivity));
            this.StartActivity(intent);
            this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);
            this.Finish();
        }
    }
}