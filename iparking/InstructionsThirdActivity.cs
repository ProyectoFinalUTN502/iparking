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
    [Activity(Label = "InstructionsThirdActivity", Theme = "@style/MyTheme.Instructions")]
    public class InstructionsThirdActivity : Activity
    {
        Button mButtonFinish;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.InstructionsThird);

            mButtonFinish = FindViewById<Button>(Resource.Id.buttonBegin);
            mButtonFinish.Click += MButtonFinish_Click;
        }

        private void MButtonFinish_Click(object sender, EventArgs e)
        {
            // De aca va a Login
        }
    }
}