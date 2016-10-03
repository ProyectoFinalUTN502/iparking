﻿using System;
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
using Android.Locations;
using System.Linq;
using Android.Graphics;

namespace iparking
{
    [Activity(Label = "iParking",Theme = "@style/CustomActionBarTheme", NoHistory = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    public class MainActivity : Activity, IOnMapReadyCallback,  ILocationListener
    {
        LocationManager locationManager;
        Location currentLocation;
        string locationProvider;

        private FragmentTransaction trans;
        private DialogParkingSearch dialog;

        private List<Parkinglot> mParkinglots;
        private System.Net.WebClient mClient;
        private GoogleMap mMap;

        private MarkerOptions mMarkerParking;
        private MarkerOptions mMarkerUser;

        private ImageView mImageMore;

        private FileManager fm;

        private int mPosition;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.SetCustomView(Resource.Layout.ActionBarMain);
            ActionBar.SetDisplayShowCustomEnabled(true);
            
            SetContentView(Resource.Layout.Main);

            locationProvider = string.Empty;

            fm = new FileManager();
            mParkinglots = new List<Parkinglot>();
            mPosition = 0;

            mImageMore = FindViewById<ImageView>(Resource.Id.imageViewMore);
            mImageMore.Click += MImageMore_Click;

            SetUpMap();
        }

        private void MImageMore_Click(object sender, EventArgs e)
        {
            Managment.ActivityManager.TakeMeTo(this, typeof(MoreOptionsActivity), false);
        }

        public void InitializeLocationManager()
        {
            locationManager = (LocationManager)GetSystemService(LocationService);

            // define its Criteria
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.NoRequirement,
                PowerRequirement = Power.NoRequirement
            };

            // find a location provider (GPS, wi-fi, etc.)
            IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);



            // if we have any, use the first one
            locationProvider = string.Empty;
            if (acceptableLocationProviders.Any())
            {
                locationProvider = acceptableLocationProviders.First();
            }
        }

        protected override void OnResume()
        {
            // Pide la posicion cuando la actividad paso a Primer Plano
            base.OnResume();
            if (locationProvider != string.Empty)
            {
                locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
            }
        }

        protected override void OnPause()
        {
            // Elimina las actualizaciones sin la aplicacion paso a Segundo Plano
            // Gasta menos bateria
            try
            {
                base.OnPause();
                locationManager.RemoveUpdates(this);
            } catch(Exception ex)
            {
                Console.WriteLine("** Error de Posicion ( OnPause ) ** : " + ex.Message);
            }
        }

        public void OnLocationChanged(Location location)
        {
            try
            {
                currentLocation = location;
                if (currentLocation != null)
                {
                    // Guardo la posicion obtenida para tener siempre una ultima posicion a la que recurrir
                    FileManager fm = new FileManager();
                    fm.SetValue("lat", currentLocation.Latitude.ToString());
                    fm.SetValue("lng", currentLocation.Longitude.ToString());

                    // Actualizo la posicion del Cliente en el Mapa
                    UpdateClientPosition(new LatLng(currentLocation.Latitude, currentLocation.Longitude));
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("** Error de Posicion ** : " + ex.Message);
                //textLocation.Text = "Error: " + ex.Message;
            }
        }

        public void OnProviderDisabled(string provider)
        {

        }

        public void OnProviderEnabled(string provider)
        {

        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {

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

            LatLng latlng = GetClientLocation();
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

                    if (mParkinglots.Count == 0)
                    {
                        trans = FragmentManager.BeginTransaction();
                        dialog = new DialogParkingSearch();
                        dialog.Show(trans, "Dialog Parking Search");

                    } else
                    {
                        //Levanto el Dialog aca, asi me aseguro que siempre haya mapa donde mostrar los marcadores
                        trans = FragmentManager.BeginTransaction();
                        dialog = new DialogParkingSearch(mParkinglots[mPosition], mPosition);
                        dialog.Show(trans, "Dialog Parking Search");
                        dialog.mGo += Dialog_mGo;
                        dialog.mNext += Dialog_mNext;

                    }
                
                }
                catch (Exception ex)
                {
                    Console.WriteLine("** Error de Carga ( Establecimientos) ** : " + ex.Message);
                    Managment.ActivityManager.TakeMeTo(this, typeof(ErrorActivity), true);
                }
            });

        }

        private void Dialog_mGo(object sender, OnGoEventArgs e)
        {
            int index = e.Position;

            Parkinglot parkinglot = mParkinglots[index];
            LatLng parkingPosition = new LatLng(parkinglot.lat, parkinglot.lng);
            LatLng clientPosition = GetClientLocation();

            mMarkerParking = MarkerManager.CreateMarker(parkingPosition, parkinglot.name, parkinglot.address, BitmapDescriptorFactory.HueRed);
            mMarkerUser = MarkerManager.CreateUserPosition(clientPosition);

            if (mMap != null)
            {
                mMap.AddMarker(mMarkerParking);
                mMap.AddMarker(mMarkerUser);

                mMap.MarkerClick += MMap_MarkerClick;
            }

            // Click en Ir...hago el camino hacia el Establecimiento
        }

        private void MMap_MarkerClick(object sender, MarkerClickEventArgs e)
        {
            Parkinglot p = mParkinglots.Find(x => x.name == e.Marker.Title);

            if (p == null) { return; }

            // El usuario llego al Establecimiento, ir a Navegacion Interna
            e.Marker.ShowInfoWindow();
            Console.WriteLine(" El Usuario llego a " + p.name + " (" + p.address + ")");
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

        private LatLng GetClientLocation()
        {
            LatLng clientLocation;

            if (locationProvider != string.Empty)
            {
                // Encontro una posicion con el GPS
                clientLocation = new LatLng(currentLocation.Latitude, currentLocation.Longitude);
            }
            else
            {
                // Tengo que usar la posicion anterior
                FileManager fm = new FileManager();

                string fileLat = fm.GetValue("lat");
                string fileLng = fm.GetValue("lng");

                if (fileLat == string.Empty || fileLng == string.Empty)
                {
                    // No hay ni posicion con GPS, ni estan grabadas en el archivo
                    // uso valores por defecto
                    clientLocation = new LatLng(ConfigManager.DefaultLatMap, ConfigManager.DefaultLongMap);
                }
                else
                {
                    clientLocation = new LatLng(Convert.ToDouble(fileLat), Convert.ToDouble(fileLng));
                }
            }

            return clientLocation;
        }

        private void UpdateClientPosition(LatLng clientPosition)
        {
            bool control = mMap != null && mMarkerUser != null && mMarkerParking != null;
            if (control)
            {
                mMarkerUser.SetPosition(clientPosition);

                mMap.Clear();
                mMap.AddMarker(mMarkerParking);
                mMap.AddMarker(mMarkerUser);
            }
        }
    }
}

