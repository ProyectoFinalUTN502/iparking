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
using System.Threading.Tasks;

namespace iparking.Managment
{
    class DeviceWifi : BroadcastReceiver
    {
        public override async void OnReceive(Context context, Intent intent)
        {
            var mainActivity = (MainActivity)context;
            //Scan results hold(among other data) identifiers and measured signal levels of available networks:

            var wifiManager = (WifiManager)mainActivity.GetSystemService(Context.WifiService);
            //var message = string.Join("\r\n", wifiManager.ScanResults.Select(r => $"{r.Ssid}"));
            // .Select(r => $"{r.Ssid} - {r.Level} dB"));

            //mainActivity.Display(message);
            // With use of.NET's async/await it's easy to reschedule another scan after some time:

            await Task.Delay(TimeSpan.FromSeconds(1));
            wifiManager.StartScan();
        }
    }
}