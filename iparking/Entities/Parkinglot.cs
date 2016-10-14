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

namespace iparking.Entities
{
    public class Parkinglot
    {
        public int id { get; set; }
        public string ssid { get; set; }
        public string name { get; set; } 
        public string description { get; set; }
        public string address { get; set; }
        public DateTime openTime { get; set; }
        public DateTime closeTime { get; set; }
        public int is24 { get; set; }
        public int isCovered { get; set; }
        public double distance { get; set; }
        public double price { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public int positions { get; set; }

        public bool is24Open()
        {
            return is24 == 1;
        }

        public bool isCoveredRoof()
        {
            return isCovered == 1;
        }
    }
}