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
using Android.Graphics;

namespace iparking.Managment
{
    class MarkerManager
    {
        public static MarkerOptions CreateMarker(LatLng position, string title, string snippet, float color)
        {
            MarkerOptions mo = new MarkerOptions();
            mo.SetPosition(position);
            // Nombre del Establecimiento
            mo.SetTitle(title); 
            // Direccion del Establecimiento
            mo.SetSnippet(snippet);
            mo.SetIcon(BitmapDescriptorFactory.DefaultMarker(color));
            
            return mo;
        }

        public static MarkerOptions CreateUserPosition(LatLng position)
        {
            MarkerOptions mo = new MarkerOptions();
            mo.SetPosition(position);
            //mo.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.circle));
            return mo;
        }

    }
}