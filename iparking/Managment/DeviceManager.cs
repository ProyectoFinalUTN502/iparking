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
using Android.Net.Wifi;
using System.Security.Cryptography;
using Android.Gms.Maps.Model;
using Android.Locations;
using iparking.Entities;

namespace iparking.Managment
{
    class DeviceManager
    {
        public static String GetMacAdress(Context context)
        {
            String macAddress = String.Empty;
            try
            {
                WifiManager wifiManager = (WifiManager)context.GetSystemService(Context.WifiService);
                WifiInfo wInfo = wifiManager.ConnectionInfo;

                macAddress = wInfo.MacAddress;
            }
            catch
            {
                macAddress = String.Empty;
            }

            return macAddress;
        }

        public static LatLng GetClientLocation(string locationProvider, Location currentLocation)
        {
            LatLng clientLocation;

            if (locationProvider != string.Empty && currentLocation != null)
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


    }
}