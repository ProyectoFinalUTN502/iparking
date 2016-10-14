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
using iparking.Entities;
using Newtonsoft.Json;

namespace iparking
{
    [Activity(Label = "ErrorActivity", Theme = "@style/MyTheme.Base")]
    public class ErrorActivity : Activity
    {
        Error currentError;

        TextView txtError;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Error);

            try
            {
                string unserialized = Intent.GetStringExtra("Error");
                currentError = JsonConvert.DeserializeObject<Error>(unserialized);

                txtError = FindViewById<TextView>(Resource.Id.textViewError);
                txtError.Text = "Error #" + currentError.Code + ": " + currentError.Message; 

            }
            catch
            {
            }
        }
    }
}