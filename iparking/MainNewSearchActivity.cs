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
using Android.Gms.Maps;
using Android.Locations;
using static Android.Gms.Maps.GoogleMap;
using iparking.Managment;
using Android.Gms.Maps.Model;
using iparking.Entities;

namespace iparking
{
    [Activity(Label = "MainNewSearchActivity", Theme = "@style/CustomActionBarTheme", LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    public class MainNewSearchActivity : Activity, IOnMapReadyCallback, ILocationListener, IInfoWindowAdapter, IOnInfoWindowClickListener
    {
        public const string errCode = "200";
        public const string errMsg = "Error al intentar cargar la Actividad Principal";

        private GoogleMap mMap;

        private MarkerOptions mMarkerParking;
        private MarkerOptions mMarkerUser;

        private ImageView mImageReload;
        private ImageView mImageBack;

        private FileManager fm;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.SetCustomView(Resource.Layout.ActionBarSearch);
            ActionBar.SetDisplayShowCustomEnabled(true);

            SetContentView(Resource.Layout.MainNewSearch);

            fm = new FileManager();

            mImageReload = FindViewById<ImageView>(Resource.Id.imageViewReload);
            mImageBack = FindViewById<ImageView>(Resource.Id.imageViewBack);

            mImageReload.Click += MImageReload_Click;
            mImageBack.Click += MImageBack_Click;

            SetUpMap();
        }

        private void MImageBack_Click(object sender, EventArgs e)
        {
            Managment.ActivityManager.TakeMeTo(this, typeof(MoreOptionsActivity), true);
        }

        private void MImageReload_Click(object sender, EventArgs e)
        {
            mMap.Clear();
            SearchParkinglots();
        }

        public void SearchParkinglots()
        {
            return;
        }

        // ===================================================================================================
        // Google Maps

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

            //LatLng latlng = GetClientLocation();
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(new LatLng(ConfigManager.DefaultLatMap, ConfigManager.DefaultLongMap), ConfigManager.DefaultZoomMap);
            mMap.MoveCamera(camera);

            // Establezco el tipo de Mapa (Normal, Stelite, Hibrido, etc...)
            mMap.MapType = GoogleMap.MapTypeNormal;

            //searchParkinglots();
        }

        // ===================================================================================================
        // InfoWindow

        public View GetInfoContents(Marker marker) { return null; }
        public View GetInfoWindow(Marker marker) { return null; }
        public void OnInfoWindowClick(Marker marker) { }


        // ===================================================================================================
        // Location
        protected override void OnResume()
        {
            try
            {
                // Pide la posicion cuando la actividad paso a Primer Plano
                base.OnResume();
                //if (locationProvider != string.Empty)
                //{
                //    locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("** Error de Posicion ( OnResume ) ** : " + ex.Message);
            }
        }

        protected override void OnPause()
        {
            try
            {
                base.OnPause();
                //locationManager.RemoveUpdates(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine("** Error de Posicion ( OnPause ) ** : " + ex.Message);
            }
        }

        public void OnLocationChanged(Location location) { }

        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras) { }
    }
}