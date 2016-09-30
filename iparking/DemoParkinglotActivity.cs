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
    [Activity(Label = "DemoParkinglotActivity", Theme = "@style/MyTheme.Base")]
    public class DemoParkinglotActivity : Activity
    {
        Button btnLayout1;
        Button btnLayout2;
        Button btnLayout3;


        ISharedPreferences pref;
        ISharedPreferencesEditor edit;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.demo_parkinglot);

            btnLayout1 = FindViewById<Button>(Resource.Id.btnLayout1);
            btnLayout2 = FindViewById<Button>(Resource.Id.btnLayout2);
            btnLayout3 = FindViewById<Button>(Resource.Id.btnLayout3);

            btnLayout1.Click += BtnLayout1_Click;
            btnLayout2.Click += BtnLayout2_Click;
            btnLayout3.Click += BtnLayout3_Click;
        }

        private void BtnLayout3_Click(object sender, EventArgs e)
        {
            pref = Application.Context.GetSharedPreferences(ConfigManager.SharedFile, FileCreationMode.Private);
            edit = pref.Edit();

            edit.PutString("pk_id", "3");
            edit.Apply();

            Intent intent = new Intent(this, typeof(DemoPathfinderActivity));
            this.StartActivity(intent);
        }

        private void BtnLayout2_Click(object sender, EventArgs e)
        {
            pref = Application.Context.GetSharedPreferences(ConfigManager.SharedFile, FileCreationMode.Private);
            edit = pref.Edit();

            edit.PutString("pk_id", "2");
            edit.Apply();

            Intent intent = new Intent(this, typeof(DemoPathfinderActivity));
            this.StartActivity(intent);
        }

        private void BtnLayout1_Click(object sender, EventArgs e)
        {
            pref = Application.Context.GetSharedPreferences(ConfigManager.SharedFile, FileCreationMode.Private);
            edit = pref.Edit();

            edit.PutString("pk_id", "1");
            edit.Apply();

            Intent intent = new Intent(this, typeof(DemoPathfinderActivity));
            this.StartActivity(intent);
        }

    }
}