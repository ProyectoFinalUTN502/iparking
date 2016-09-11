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
    public class Profile
    {
        public int id { get; set; }
        public int range { get; set; }
        public double maxPrice { get; set; }
        public bool is24 { get; set; }
        public bool isCovered { get; set; }
        public Client client { get; set; }

        public string GetIs24()
        {
            return is24 ? "1" : "0";
        }

        public string GetIsCovered()
        {
            return isCovered ? "1" : "0";
        }
    }
}