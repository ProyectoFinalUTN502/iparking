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
    public class ProfileController
    {
        public static bool validate(Profile p)
        {
            bool result = true;

            if (p.maxPrice == 0)
            {
                result = false;
            }

            return result;
        }
    }
}