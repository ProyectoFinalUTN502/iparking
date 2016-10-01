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
using iparking.Managment;
using iparking.Entities;
using Newtonsoft.Json;

namespace iparking
{
    [Activity(Label = "VehicleActivity", Theme = "@style/MyTheme.Base")]
    public class VehicleActivity : Activity
    {
        System.Net.WebClient mClient;
        FileManager mFile;
        List<Vehicle> mVehicles;
        ListView mListView;
        VehicleListAdapter mListAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Vehicle);

            mListView = FindViewById<ListView>(Resource.Id.listViewVehicles);

            mFile = new FileManager();

            getClientVehicles();
        }


        public void getClientVehicles()
        {
            string clientID = mFile.GetValue("id");

            mClient = new System.Net.WebClient();
            Uri url = new Uri(ConfigManager.WebService + "/searchVehicle.php?client_id=" + clientID);

            mClient.DownloadDataAsync(url);
            mClient.DownloadDataCompleted += MClient_DownloadDataCompleted;

        }

        private void MClient_DownloadDataCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                try
                {
                    string json = Encoding.UTF8.GetString(e.Result);
                    mVehicles = JsonConvert.DeserializeObject<List<Vehicle>>(json);

                    // Cargo la Listview con los Vehiculos
                    mListAdapter = new VehicleListAdapter(this, mVehicles);
                    mListView.Adapter = mListAdapter;
                    mListView.ItemClick += MListView_ItemClick;
                }
                catch (Exception ex)
                {
                    Managment.ActivityManager.TakeMeTo(this, typeof(ErrorActivity), true);    
                }

            });
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int position = e.Position;

            // Guardo el Vehicle ID
            mFile.SetValue("vt_id", mListAdapter.GetItemId(position).ToString());

            // Lo llevo a Main 
            Managment.ActivityManager.TakeMeTo(this, typeof(MainActivity), true);
        }
    }
}