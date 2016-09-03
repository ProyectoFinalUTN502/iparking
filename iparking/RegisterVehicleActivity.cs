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
    [Activity(Label = "RegisterVehicleActivity", Theme = "@style/MyTheme.Base")]
    public class RegisterVehicleActivity : Activity
    {
        RadioButton mRadioCar;
        RadioButton mRadioSUV;
        RadioButton mRadioVAN;
        RadioButton mRadioMotorcicle;

        Button mButtonFinish;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RegisterVehicle);

            mRadioCar = FindViewById<RadioButton>(Resource.Id.radioButtonCar);
            mRadioSUV = FindViewById<RadioButton>(Resource.Id.radioButtonSuv);
            mRadioVAN = FindViewById<RadioButton>(Resource.Id.radioButtonVan);
            mRadioMotorcicle = FindViewById<RadioButton>(Resource.Id.radioButtonMotorcicle);

            mButtonFinish = FindViewById<Button>(Resource.Id.btnRegisterAll);
            mButtonFinish.Click += MButtonFinish_Click;

        }

        private void MButtonFinish_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent);
            this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);
            this.Finish();
        }
    }
}