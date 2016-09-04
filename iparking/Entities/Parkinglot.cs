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
        public string name { get; set; } 
        public string address { get; set; }
        public string time { get; set; }
        public string price { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
    }
}