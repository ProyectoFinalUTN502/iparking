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

namespace iparking
{
    [Activity(Label = "DemoVehicleAvtivity", Theme = "@style/MyTheme.Base")]
    public class DemoVehicleAvtivity : Activity
    {
        Button btnVan;
        Button btnSuv;
        Button btnCar;
        Button btnBike;

        ISharedPreferences pref;
        ISharedPreferencesEditor edit;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.demo_vehicle);

            btnVan = FindViewById<Button>(Resource.Id.btnVan);
            btnSuv = FindViewById<Button>(Resource.Id.btnSuv);
            btnCar = FindViewById<Button>(Resource.Id.btnCar);
            btnBike = FindViewById<Button>(Resource.Id.btnBike);

            btnVan.Click += BtnVan_Click;
            btnSuv.Click += BtnSuv_Click;
            btnCar.Click += BtnCar_Click;
            btnBike.Click += BtnBike_Click;
        }

        private void BtnBike_Click(object sender, EventArgs e)
        {
            pref = Application.Context.GetSharedPreferences(ConfigManager.SharedFile, FileCreationMode.Private);
            edit = pref.Edit();

            edit.PutString("vt_id", "4");
            edit.Apply();

            Intent intent = new Intent(this, typeof(DemoParkinglotActivity));
            this.StartActivity(intent);
        }

        private void BtnCar_Click(object sender, EventArgs e)
        {
            pref = Application.Context.GetSharedPreferences(ConfigManager.SharedFile, FileCreationMode.Private);
            edit = pref.Edit();

            edit.PutString("vt_id", "3");
            edit.Apply();

            Intent intent = new Intent(this, typeof(DemoParkinglotActivity));
            this.StartActivity(intent);
        }

        private void BtnSuv_Click(object sender, EventArgs e)
        {
            pref = Application.Context.GetSharedPreferences(ConfigManager.SharedFile, FileCreationMode.Private);
            edit = pref.Edit();

            edit.PutString("vt_id", "2");
            edit.Apply();

            Intent intent = new Intent(this, typeof(DemoParkinglotActivity));
            this.StartActivity(intent);
        }

        private void BtnVan_Click(object sender, EventArgs e)
        {
            pref = Application.Context.GetSharedPreferences(ConfigManager.SharedFile, FileCreationMode.Private);
            edit = pref.Edit();

            edit.PutString("vt_id", "1");
            edit.Apply();

            Intent intent = new Intent(this, typeof(DemoParkinglotActivity));
            this.StartActivity(intent);
        }
    }
}