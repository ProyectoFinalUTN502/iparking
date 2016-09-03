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
    [Activity(Label = "InstructionsFirstActivity", Theme = "@style/MyTheme.Instructions" )]
    public class InstructionsFirstActivity : Activity
    {
        Button mButtonNext;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.InstructionsFirst);

            mButtonNext = FindViewById<Button>(Resource.Id.buttonFirst);
            mButtonNext.Click += MButtonNext_Click;
        }

        private void MButtonNext_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(InstructionsSecondActivity));
            this.StartActivity(intent);
            this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);    
        }
    }
}