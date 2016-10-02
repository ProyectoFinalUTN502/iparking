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

namespace iparking.Managment
{
    class ActivityManager
    {
        public static void TakeMeTo(Activity activity, Type type, bool finish)
        {
            Intent intent = new Intent(activity, type);
            activity.StartActivity(intent);
            activity.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);

            if (finish)
            {
                activity.Finish();
            }
        }


    }
}