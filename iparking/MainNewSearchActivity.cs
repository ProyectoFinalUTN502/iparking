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
using Newtonsoft.Json;
using iparking.Controllers;

namespace iparking
{
    [Activity(Label = "MainNewSearchActivity", Theme = "@style/CustomActionBarTheme", LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    public class MainNewSearchActivity : Activity, IOnMapReadyCallback, ILocationListener, IInfoWindowAdapter, IOnInfoWindowClickListener
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

        private ImageView mImageReload;
        private ImageView mImageBack;
        private ProgressBar mProgressBar;

        private FileManager fm;

        private Profile mProfile;

        private FragmentTransaction trans;
        private DialogParkingSearch dialog;
        private DialogParkingEnter dialogEnter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.SetCustomView(Resource.Layout.ActionBarSearch);
            ActionBar.SetDisplayShowCustomEnabled(true);

            SetContentView(Resource.Layout.MainNewSearch);

            mProfile = Managment.ActivityManager.GetData<Profile>(this);

            locationProvider = string.Empty;
            currentLocation = null;

            fm = new FileManager();
            mClient = new System.Net.WebClient();
            mParkinglots = new List<Parkinglot>();
            mPosition = 0;

            mImageReload = FindViewById<ImageView>(Resource.Id.imageViewReload);
            mImageBack = FindViewById<ImageView>(Resource.Id.imageViewBack);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBarRoute);

            mImageReload.Click += MImageReload_Click;
            mImageBack.Click += MImageBack_Click;
            mProgressBar.Visibility = ViewStates.Invisible;

            InitializeLocationManager();
            SetUpMap();
        }

        private void MImageBack_Click(object sender, EventArgs e)
        {
            Managment.ActivityManager.TakeMeTo(this, typeof(MoreOptionsActivity), true);
        }

        private void MImageReload_Click(object sender, EventArgs e)
        {
            if (dialog != null) { dialog.Dismiss(); }
            if (dialogEnter != null) { dialogEnter.Dismiss(); }

            if (mMap != null && mParkinglots != null)
            {
                mMap.Clear();
                mParkinglots.Clear();
                SearchParkinglots();
            }
        }

        public void SearchParkinglots()
        {
            try
            {
                string vehicleTypeID = fm.GetValue("vt_id");
                string lat = ConfigManager.DefaultLatMap.ToString();
                string lng = ConfigManager.DefaultLongMap.ToString();
                string range = mProfile.range.ToString();
                string price = mProfile.maxPrice.ToString();
                string is24 = mProfile.is24.ToString();
                string isCovered = mProfile.isCovered.ToString();

                string urlRef = "vt_id=" + vehicleTypeID + "&" + 
                                "lat=" + lat + "&" + 
                                "lng=" + lng + "&" + 
                                "range=" + range + "&" + 
                                "price=" + price + "&" + 
                                "is24=" + is24 + "&" + 
                                "isCovered=" + isCovered ;


                Uri url = new Uri(ConfigManager.WebService + "/" + "searchParkinglotParam.php?" + urlRef);

                mProgressBar.Visibility = ViewStates.Visible;
                mClient.DownloadDataAsync(url);
                mClient.DownloadDataCompleted += MClient_DownloadDataCompleted;
            }
            catch (Exception ex)
            {
                Managment.ActivityManager.ShowError(this, new Error(errCode, errMsg));
            }
        }

        private void MClient_DownloadDataCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
            try
            {
                string json = Encoding.UTF8.GetString(e.Result);
                mParkinglots = JsonConvert.DeserializeObject<List<Parkinglot>>(json);

                if (mParkinglots.Count == 0)
                {
                    RunOnUiThread(() =>
                    {
                        mProgressBar.Visibility = ViewStates.Invisible;
                        trans = FragmentManager.BeginTransaction();
                        dialog = new DialogParkingSearch();
                        dialog.Show(trans, "Dialog Parking Search");
                    });

                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        //Levanto el Dialog aca, asi me aseguro que siempre haya mapa donde mostrar los marcadores
                        mProgressBar.Visibility = ViewStates.Invisible;
                        trans = FragmentManager.BeginTransaction();
                        dialog = new DialogParkingSearch(mParkinglots[mPosition], mPosition);
                        dialog.Show(trans, "Dialog Parking Search");
                        dialog.mGo += Dialog_mGo;
                        dialog.mNext += Dialog_mNext;
                    });
                }

            }
            catch (Exception ex)
            {
                Managment.ActivityManager.ShowError(this, new Error(errCode, errMsg));
            }
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

        private void Dialog_mGo(object sender, OnGoEventArgs e)
        {
            int index = e.Position;

            Parkinglot parkinglot = mParkinglots[index];
            LatLng parkingPosition = new LatLng(parkinglot.lat, parkinglot.lng);
            LatLng clientPosition = DeviceManager.GetClientLocation(locationProvider, currentLocation);

            mMarkerParking = MarkerManager.CreateMarker(parkingPosition, parkinglot.name, parkinglot.id + ":" + parkinglot.address, BitmapDescriptorFactory.HueRed);
            mMarkerUser = MarkerManager.CreateUserPosition(clientPosition);

            if (mMap != null)
            {
                mMap.AddMarker(mMarkerParking);
                mMap.AddMarker(mMarkerUser);

                // Sete el Info Windows
                mMap.SetInfoWindowAdapter(this);

                // Agregarle Eventos a la Info Window
                // La Info Window solo puede contener eventos que involucren a TODA la ventana
                // No a componentes internos

                mMap.SetOnInfoWindowClickListener(this);
                RouteToDestination(clientPosition, parkingPosition);
            }
        }

        private void UpdateClientPosition(LatLng clientPosition)
        {
            bool control = mMap != null && 
                mMarkerUser != null && 
                mMarkerParking != null && 
                dialog != null && 
                dialogEnter != null;

            if (control)
            {
                mMarkerUser.SetPosition(clientPosition);

                dialog.Dismiss();
                dialogEnter.Dismiss();
                mMap.Clear();
                mMap.AddMarker(mMarkerParking);
                mMap.AddMarker(mMarkerUser);
            }
        }

        public void RouteToDestination(LatLng client, LatLng parkinglot)
        {
            try
            {
                string origin = client.Latitude.ToString() + "," + client.Longitude.ToString();
                string destination = parkinglot.Latitude.ToString() + "," + parkinglot.Longitude.ToString();

                System.Net.WebClient localClient = new System.Net.WebClient();
                Uri url = new Uri(ConfigManager.GoogleService + "origin=" + origin + "&destination=" + destination);

                mProgressBar.Visibility = ViewStates.Visible;
                localClient.DownloadDataAsync(url);
                localClient.DownloadDataCompleted += LocalClient_DownloadDataCompleted;
            }
            catch (Exception ex)
            {
                Console.WriteLine("** Error ** : No se pudo conectar a Google Route /n " + ex.Message);
                //Managment.ActivityManager.ShowError(this, new Error(errCode, errMsg));
            }
        }

        private void LocalClient_DownloadDataCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
            try
            {
                string json = Encoding.UTF8.GetString(e.Result);
                var googleDirection = JsonConvert.DeserializeObject<GoogleDirection>(json);
                PolylineOptions po = DirectionController.ResolveRoute(googleDirection);

                RunOnUiThread(() =>
                {
                    mProgressBar.Visibility = ViewStates.Invisible;
                    mMap.AddPolyline(po);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("** Error ** : El servidor Google Route no devolvio valores correctos /n " + ex.Message);
            }

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

            LatLng latlng = DeviceManager.GetClientLocation(locationProvider, currentLocation);
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, ConfigManager.DefaultZoomMap);
            mMap.MoveCamera(camera);

            // Establezco el tipo de Mapa (Normal, Stelite, Hibrido, etc...)
            mMap.MapType = GoogleMap.MapTypeNormal;

            SearchParkinglots();
        }

        // ===================================================================================================
        // InfoWindow

        public View GetInfoContents(Marker marker) { return null; }

        public View GetInfoWindow(Marker marker)
        {
            // En el Snippet del marcador viene el ID del Parkinlot
            // Primero separo ID de Address

            TextView txtName;
            TextView txtAddress;

            string data = marker.Snippet;
            string[] exploded = data.Split(':');
            string id = exploded[0];
            string address = exploded[1];

            View view = LayoutInflater.Inflate(Resource.Layout.InfoWindowsMain, null, false);
            txtName = view.FindViewById<TextView>(Resource.Id.textViewName);
            txtAddress = view.FindViewById<TextView>(Resource.Id.textViewAddress);

            txtName.Text = marker.Title;
            txtAddress.Text = address;

            return view;
        }

        public void OnInfoWindowClick(Marker marker)
        {
            // Snippet = id:address
            string[] data = marker.Snippet.Split(':');

            int id = Convert.ToInt32(data[0]);
            int c = Convert.ToInt32(fm.GetValue("id"));
            int vt = Convert.ToInt32(fm.GetValue("vt_id"));

            trans = FragmentManager.BeginTransaction();
            dialogEnter = new DialogParkingEnter(id, vt, c);
            dialogEnter.Show(trans, "Dialog Enter");
            dialogEnter.mEnterEvent += DialogEnter_mEnterEvent;
            //dialogEnter.mEnterEvent += DialogEnter_mEnterEvent;
        }

        private void DialogEnter_mEnterEvent(object sender, OnEnterEventArgs e)
        {
            string p = e.ParkinglotID.ToString();
            string vt = e.VehicleTypeID.ToString();
            string c = e.ClientID.ToString();

            List<int> navData = new List<int>();
            navData.Add(e.ParkinglotID);
            navData.Add(e.VehicleTypeID);
            navData.Add(e.ClientID);

            Console.WriteLine("** Ingreso a: " + p + " " + vt + " " + c + " **");
            Managment.ActivityManager.TakeMeTo(this, typeof(NavigationActivity), true, navData);
        }


        // ===================================================================================================
        // Location
        protected override void OnResume()
        {
            try
            {
                // Pide la posicion cuando la actividad paso a Primer Plano
                base.OnResume();
                if (locationProvider != string.Empty)
                {
                    locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
                }
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
                locationManager.RemoveUpdates(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine("** Error de Posicion ( OnPause ) ** : " + ex.Message);
            }
        }

        public void InitializeLocationManager()
        {
            locationManager = (LocationManager)GetSystemService(LocationService);

            // Defino Criterios de Busqueda
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.NoRequirement,
                PowerRequirement = Power.NoRequirement
            };

            // Lista con Proveedores de Servicio (GPS, wi-fi, etc.)
            IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);

            // Si hay alguno en la lista, entonces usamos el primero
            locationProvider = string.Empty;
            if (acceptableLocationProviders.Any())
            {
                locationProvider = acceptableLocationProviders.First();
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
            }
        }

        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras) { }
    }
}