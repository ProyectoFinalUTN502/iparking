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
using System.Collections.Specialized;

namespace iparking
{
    [Activity(Label = "VehicleActivity", Theme = "@style/MyTheme.Base")]
    public class VehicleActivity : Activity
    {
        const string errCode = "300";
        const string errMsg = "No se pudo acceder a la Lista de Vehiculos";

        System.Net.WebClient mClient;
        FileManager mFile;
        List<Vehicle> mVehicles;
        ListView mListView;
        Button mButtonAdd;
        VehicleListAdapter mListAdapter;

        FragmentTransaction trans;
        DialogParkingAddVehicle dialogAdd;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Vehicle);

            mListView = FindViewById<ListView>(Resource.Id.listViewVehicles);
            mButtonAdd = FindViewById<Button>(Resource.Id.buttonAddVehicle);
            mButtonAdd.Click += MButtonAdd_Click;

            mFile = new FileManager();

            getClientVehicles();
        }

        private void MButtonAdd_Click(object sender, EventArgs e)
        {
            // Levantar Dialog Para Nuevo Vehiculo
            trans = FragmentManager.BeginTransaction();
            dialogAdd = new DialogParkingAddVehicle();
            dialogAdd.Show(trans, "Dialog Add Vehicle");
            dialogAdd.mAddEvent += DialogAdd_mAddEvent;
        }

        private void DialogAdd_mAddEvent(object sender, OnAddEvent e)
        {
            Vehicle v = e.Vehicle;

            if (v == null){ return; }
            AddVehicle(v);
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int position = e.Position;

            // Guardo el Vehicle Type ID
            mFile.SetValue("vt_id", mVehicles[position].vehicleTypeID.ToString());//mListAdapter.GetItemId(position).ToString());
            // Guardo el Vehicle ID
            mFile.SetValue("vehicle", mVehicles[position].id.ToString());

            // Lo llevo a Main 
            Managment.ActivityManager.TakeMeTo(this, typeof(MainActivity), true);
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

        public void AddVehicle(Vehicle vehicle)
        {
            try
            {
                System.Net.WebClient wclient = new System.Net.WebClient();
                Uri uri = new Uri(ConfigManager.WebService + "/addVehicle.php");
                NameValueCollection param = new NameValueCollection();

                param.Add("name", vehicle.name);
                param.Add("client_id", mFile.GetValue("id"));
                param.Add("vehicle_type_id", vehicle.vehicleTypeID.ToString());

                wclient.UploadValuesCompleted += Wclient_UploadValuesCompleted;
                wclient.UploadValuesAsync(uri, param);
            }
            catch (Exception ex)
            {
                Managment.ActivityManager.ShowError(this, new Error(errCode, errMsg));
            }
        }

        private void Wclient_UploadValuesCompleted(object sender, System.Net.UploadValuesCompletedEventArgs e)
        {
            try
            {
                string json = Encoding.UTF8.GetString(e.Result);
                OperationResult or = JsonConvert.DeserializeObject<OperationResult>(json);

                if (or.error)
                {
                    // Ocurrio un error
                    Console.WriteLine("** Error ** : No se pudo agregar el vehiculo ");
                }
                else
                {
                    getClientVehicles();
                }
            }
            catch (Exception ex)
            {
                Managment.ActivityManager.ShowError(this, new Error(errCode, errMsg));
            }
        }
    }
}