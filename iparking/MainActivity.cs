using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using iparking.Entities;
using iparking.Managment;

namespace iparking
{
    [Activity(
        Label = "iparking", 
        Theme = "@style/MyTheme.Base", 
        NoHistory = true, 
        LaunchMode = Android.Content.PM.LaunchMode.SingleTask
        )]


    public class MainActivity : Activity
    {
        private Parkinglot parkinglot;
        private TextView mTextContent;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            mTextContent = FindViewById<TextView>(Resource.Id.textViewContent);

            FragmentTransaction trans = FragmentManager.BeginTransaction();
            DialogParkingSearch dialog = new DialogParkingSearch();
            dialog.Show(trans, "Dialog Parking Search");
            dialog.mGo += Dialog_mGo;

        }

        private void Dialog_mGo(object sender, OnGoEventArgs e)
        {
            parkinglot = e.Parkinglot;

            mTextContent.Text = "El Contenido es: " + parkinglot.name + " " + parkinglot.address;
        }
    }
}

