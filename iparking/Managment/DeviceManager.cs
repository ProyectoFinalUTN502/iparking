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

        
    }
}