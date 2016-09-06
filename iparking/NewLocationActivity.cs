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
using iparking.Entities;
using Android.Gms.Maps.Model;

namespace iparking
{
    [Activity(Label = "NewLocationActivity", Theme = "@style/MyTheme.Base")]
    public class NewLocationActivity : Activity, IOnMapReadyCallback
    {
        ImageView mBack;
        private GoogleMap mMap;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.NewLocation);
            mBack = FindViewById<ImageView>(Resource.Id.imageViewBack);
            mBack.Click += MBack_Click;

            SetUpMap();
        }

        private void MBack_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MoreOptionsActivity));
            this.StartActivity(intent);
            this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);
            this.Finish();
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
        }
    }
}