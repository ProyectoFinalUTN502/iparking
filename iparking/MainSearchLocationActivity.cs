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
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.Gms.Maps;
using static Android.Gms.Maps.GoogleMap;

namespace iparking
{
    [Activity(Label = "MainSearchLocationActivity", Theme = "@style/CustomActionBarTheme", LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    public class MainSearchLocationActivity : Activity, IOnMapReadyCallback
    {
        public const string errCode = "200";
        public const string errMsg = "Error al intentar cargar la Actividad Principal";

        LocationManager locationManager;
        Location currentLocation;
        string locationProvider;

        private GoogleMap mMap;

        private MarkerOptions mMarkerParking;
        private MarkerOptions mMarkerUser;

        private int mPosition;
        private List<Parkinglot> mParkinglots;
        private System.Net.WebClient mClient;

        private FragmentTransaction trans;
        private DialogSearchInstructions dialogInstructions;

        private FileManager fm;

        private ImageView mImageSearch;
        private ImageView mImageBack;
        private ProgressBar mProgressMap;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.SetCustomView(Resource.Layout.ActionBarLocation);
            ActionBar.SetDisplayShowCustomEnabled(true);

            SetContentView(Resource.Layout.MainNewSearch);

            mImageSearch = FindViewById<ImageView>(Resource.Id.imageViewSearch);
            mImageBack = FindViewById<ImageView>(Resource.Id.imageViewBack);
            mProgressMap = FindViewById<ProgressBar>(Resource.Id.progressBarRoute);

            mImageSearch.Click += MImageSearch_Click;
            mImageBack.Click += MImageBack_Click;

            ShowInstructions();
            SetUpMap();


        }

        private void MImageBack_Click(object sender, EventArgs e)
        {
            Managment.ActivityManager.TakeMeTo(this, typeof(MoreOptionsActivity), true);
        }

        private void MImageSearch_Click(object sender, EventArgs e)
        {
            return;
        }

        public void ShowInstructions()
        {
            trans = FragmentManager.BeginTransaction();
            dialogInstructions = new DialogSearchInstructions();
            dialogInstructions.Show(trans, "Instructions");
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

            LatLng latlng = new LatLng(ConfigManager.DefaultLatMap, ConfigManager.DefaultLongMap);
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, ConfigManager.DefaultDraggZoomMap);
            mMap.MoveCamera(camera);

            // Establezco el tipo de Mapa (Normal, Stelite, Hibrido, etc...)
            mMap.MapType = GoogleMap.MapTypeNormal;

            // Cargo el Marcador que va a servir de centro 
            mMarkerUser = MarkerManager.CreateUserDragable();
            mMap.AddMarker(mMarkerUser);

        }
    }

   
}