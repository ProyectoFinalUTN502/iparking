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
    class Historic
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string vehicle { get; set; }
        public string parkinglot { get; set; }
        public string address { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public int xPoint { get; set; }
        public int yPoint { get; set; }
        public int client_id { get; set; }
    }
}