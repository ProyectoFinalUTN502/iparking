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
    class ClientLocation
    {
        private double mLat;
        private double mLng;

        public ClientLocation()
        {

        }

        public ClientLocation(double lat, double lng)
        {
            mLat = lat;
            mLng = lng;
        }

        public double Latitude
        {
            get { return mLat; }
            set { mLat = value; }
        }

        public double Longitude
        {
            get { return mLng; }
            set { mLng = value; }
        }
    }
}