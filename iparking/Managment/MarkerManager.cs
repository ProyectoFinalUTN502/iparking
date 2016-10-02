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
using Android.Gms.Maps.Model;

namespace iparking.Managment
{
    class MarkerManager
    {
        public static MarkerOptions CreateMarker(LatLng position, string title, string snippet, float color)
        {
            MarkerOptions mo = new MarkerOptions();
            mo.SetPosition(position);
            mo.SetTitle(title);
            mo.SetSnippet(snippet);
            mo.SetIcon(BitmapDescriptorFactory.DefaultMarker(color));

            return mo;
            //mSelectedParkingMarkerOptions = new MarkerOptions();
            //mSelectedParkingMarkerOptions.SetPosition(position);
            //mSelectedParkingMarkerOptions.SetTitle(parkinglot.name);
            //mSelectedParkingMarkerOptions.SetSnippet(parkinglot.time + " | " + parkinglot.price);
            //mSelectedParkingMarkerOptions.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
        }

    }
}