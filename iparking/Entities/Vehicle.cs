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
    class Vehicle
    {
        public int id { get; set; }
        public string name { get; set; }
        public Client client { get; set; }
        public int vehicleTypeID { get; set; }
    }
}