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
using System.Threading.Tasks;

namespace iparking
{
    [Activity(Label = "SlpashActivity", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SlpashActivity : Activity
    {
        //static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() => {

                // ACA VAN TODAS LAS ACTIVIDADES QUE TENDRIA QUE HACER LA APP MIENTRAS
                // CARGA LA SPLASH
                for (int i = 0; i < 10000000; i++) { }
            });

            startupWork.ContinueWith(t => {
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }
    }
}