using System;
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

namespace iparking
{
    [Activity(Label = "iparking", Theme = "@style/MyTheme.Base", NoHistory = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    public class MainActivity : Activity, IOnMapReadyCallback
    {
        private Parkinglot parkinglot;
        private TextView mTextContent;
        private GoogleMap mMap;
        private MarkerOptions mSelectedParkingMarkerOptions;
        private MarkerOptions mUserMarkerOptions;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            SetUpMap();

            mTextContent = FindViewById<TextView>(Resource.Id.textViewContent);

        }

        private void Dialog_mGo(object sender, OnGoEventArgs e)
        {
            parkinglot = e.Parkinglot;

            Double lat = Convert.ToDouble(parkinglot.lat);
            Double lng = Convert.ToDouble(parkinglot.lng);

            LatLng position = new LatLng(lat, lng);

            mSelectedParkingMarkerOptions = new MarkerOptions();
            mSelectedParkingMarkerOptions.SetPosition(position);
            mSelectedParkingMarkerOptions.SetTitle(parkinglot.name);
            mSelectedParkingMarkerOptions.SetSnippet(parkinglot.time + " | " + parkinglot.price);
            mSelectedParkingMarkerOptions.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));

            LatLng userPosition = new LatLng(ConfigManager.DefaultLatMap, ConfigManager.DefaultLongMap);

            mUserMarkerOptions = new MarkerOptions();
            mUserMarkerOptions.SetPosition(userPosition);
            mUserMarkerOptions.SetTitle("Usted esta aqui!");
            mUserMarkerOptions.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueGreen));


            if (mMap != null)
            {
                mMap.AddMarker(mSelectedParkingMarkerOptions);
                mMap.AddMarker(mUserMarkerOptions);
            }


            //mTextContent.Text = "El Contenido es: " + parkinglot.name + " " + parkinglot.address;
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

            //Levanto el Dialog aca, asi me aseguro que siempre haya mapa donde mostrar los marcadores
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            DialogParkingSearch dialog = new DialogParkingSearch();
            dialog.Show(trans, "Dialog Parking Search");
            dialog.mGo += Dialog_mGo;


            //-----------------------------------------------------------------------------------------------------
            //MarkerOptions options = new MarkerOptions();
            //options.SetPosition(latlng);
            //options.SetTitle("Casa de Cesar");
            //options.SetSnippet("Aca vive el astro de la programacion Cesar Cappetto");
            //options.Draggable(true);

            //mMap.AddMarker(options);
            //mMap.AddMarker(new MarkerOptions()
            //    .SetPosition(latlng2)
            //    .SetTitle("El otro marcador")
            //    .SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueBlue)));

            /**
             * 
             * IMPORTANTE: Si se quiere usar Info Windows, no se tiene que agregar un 
             * evento click al marcador. O se usa el CLick, o se usa Info WIndows
             * 
             */


            //mMap.MarkerClick += MMap_MarkerClick;
            //mMap.MarkerDragEnd += MMap_MarkerDragEnd;


            // Sete el Info Windows
            //mMap.SetInfoWindowAdapter(this);

            // Agregarle Eventos a la Info Window
            // La Info Window solo puede contener eventos que involucren a TODA la ventana
            // No a componentes internos

            //mMap.SetOnInfoWindowClickListener(this);

        }

    }
}

