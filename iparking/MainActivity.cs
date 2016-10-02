using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using static Android.Gms.Maps.GoogleMap;

using iparking.Entities;
using iparking.Managment;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace iparking
{
    [Activity(Label = "iParking",Theme = "@style/CustomActionBarTheme", NoHistory = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    public class MainActivity : Activity, IOnMapReadyCallback
    {
        FragmentTransaction trans;
        DialogParkingSearch dialog;

        private List<Parkinglot> mParkinglots;
        private System.Net.WebClient mClient;
        private Parkinglot parkinglot;
        private GoogleMap mMap;
        private MarkerOptions mSelectedParkingMarkerOptions;
        private MarkerOptions mUserMarkerOptions;

        private ImageView mImageMore;

        private FileManager fm;

        private int mPosition;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.SetCustomView(Resource.Layout.ActionBarMain);
            ActionBar.SetDisplayShowCustomEnabled(true);
            
            SetContentView(Resource.Layout.Main);

            fm = new FileManager();
            mParkinglots = new List<Parkinglot>();
            mPosition = 0;

            mImageMore = FindViewById<ImageView>(Resource.Id.imageViewMore);
            mImageMore.Click += MImageMore_Click;

            SetUpMap();
        }

        private void MImageMore_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MoreOptionsActivity));
            this.StartActivity(intent);
            this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);

            //Intent intent = new Intent(this, typeof(NavigationActivity));
            //this.StartActivity(intent);

        }

        private void SetUpMap()
        {
            if (mMap == null)
            {
                // Carga el Mapa de forma asincrona
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {

            mMap = googleMap;

            // Aca tengo que tomar la Lat y Long del Dispositivo y cargar la LatLng 
            // Si no tiene (porque esta tardando en tomar, tengo que tener una por defecto)


            LatLng latlng = new LatLng(ConfigManager.DefaultLatMap, ConfigManager.DefaultLongMap);
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, ConfigManager.DefaultZoomMap); 
            mMap.MoveCamera(camera);

            // Establezco el tipo de Mapa (Normal, Stelite, Hibrido, etc...)
            mMap.MapType = GoogleMap.MapTypeNormal;

            searchParkinglots();
        }

        public void searchParkinglots()
        {
            string clientID = fm.GetValue("id");
            string vehicleTypeID = fm.GetValue("vt_id");
            string lat = ConfigManager.DefaultLatMap.ToString();
            string lng = ConfigManager.DefaultLongMap.ToString();

            string urlRef = "client_id=" + clientID + "&" + "vt_id=" + vehicleTypeID + "&" + "lat=" + lat + "&" + "lng=" + lng;
            mClient = new System.Net.WebClient();
            Uri url = new Uri(ConfigManager.WebService + "/" + "searchParkinglot.php?" + urlRef);

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
                    mParkinglots = JsonConvert.DeserializeObject<List<Parkinglot>>(json);

                
                    //Levanto el Dialog aca, asi me aseguro que siempre haya mapa donde mostrar los marcadores
                    trans = FragmentManager.BeginTransaction();
                    dialog = new DialogParkingSearch(mParkinglots[mPosition], mPosition);
                    dialog.Show(trans, "Dialog Parking Search");
                    dialog.mGo += Dialog_mGo;
                    dialog.mNext += Dialog_mNext;
                }
                catch (Exception ex)
                {
                    Managment.ActivityManager.TakeMeTo(this, typeof(ErrorActivity), true);
                }
            });

        }

        private void Dialog_mGo(object sender, OnGoEventArgs e)
        {
            //parkinglot = e.Parkinglot;

            //Double lat = Convert.ToDouble(parkinglot.lat);
            //Double lng = Convert.ToDouble(parkinglot.lng);

            //LatLng position = new LatLng(lat, lng);

            //mSelectedParkingMarkerOptions = new MarkerOptions();
            //mSelectedParkingMarkerOptions.SetPosition(position);
            //mSelectedParkingMarkerOptions.SetTitle(parkinglot.name);
            //mSelectedParkingMarkerOptions.SetSnippet(parkinglot.time + " | " + parkinglot.price);
            //mSelectedParkingMarkerOptions.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));

            //LatLng userPosition = new LatLng(ConfigManager.DefaultLatMap, ConfigManager.DefaultLongMap);

            //mUserMarkerOptions = new MarkerOptions();
            //mUserMarkerOptions.SetPosition(userPosition);
            //mUserMarkerOptions.SetTitle("Usted esta aqui!");
            //mUserMarkerOptions.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueGreen));


            //if (mMap != null)
            //{
            //    mMap.AddMarker(mSelectedParkingMarkerOptions);
            //    mMap.AddMarker(mUserMarkerOptions);
            //}

            // Click en Ir...hago el camino hacia el Establecimiento
        }

        private void Dialog_mNext(object sender, OnNextEventArgs e)
        {
            Parkinglot parkinglot;
            int nextPosition = e.Position;

            if (nextPosition >= mParkinglots.Count)
            {
                nextPosition = 0;
            }

            parkinglot = mParkinglots[nextPosition];
            dialog.SetParkinglot(parkinglot, nextPosition);
        }
    }
}

