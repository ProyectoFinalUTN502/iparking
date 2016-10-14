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
using iparking.Entities;

namespace iparking.Controllers
{
    class VehicleController
    {
        public static bool validate(Vehicle v)
        {
            return v.name == string.Empty;
        }
    }
}