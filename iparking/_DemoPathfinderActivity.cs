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

using iparking.Managment;
using iparking.Entities;
using Android.Webkit;

namespace iparking
{
    [Activity(Label = "DemoPathfinderActivity", Theme = "@style/MyTheme.Base")]
    public class _DemoPathfinderActivity : Activity
    {
        WebView mWebView;
        WebClient mWebClient;
        ProgressBar mProgress;

        ISharedPreferences pref;
        ISharedPreferencesEditor edit;

        Button btnEnd;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout._demo_pathfinder);

            btnEnd = FindViewById<Button>(Resource.Id.btnEnd);
            mProgress = FindViewById<ProgressBar>(Resource.Id.progressBar);

            btnEnd.Click += BtnEnd_Click;

            pref = Application.Context.GetSharedPreferences(ConfigManager.SharedFile, FileCreationMode.Private);
            edit = pref.Edit();

            string pkid = pref.GetString("pk_id", String.Empty);
            string vtid = pref.GetString("vt_id", String.Empty);

            mWebClient = new WebClient();

            mWebClient.mOnProgressBarChanged += (int state) =>
            {
                if (state == 0)
                {
                    // termino, ocultar la progress
                    mProgress.Visibility = ViewStates.Invisible;
                }
                else
                {
                    mProgress.Visibility = ViewStates.Visible;
                }
            };



            mWebView = FindViewById<WebView>(Resource.Id.webView);


            mWebView.Settings.JavaScriptEnabled = true;
            mWebView.LoadUrl(ConfigManager.WebService + "/demo2/demo.php?pk_id=" + pkid + "&vt_id=" + vtid + "&cl_id=1");
            mWebView.SetWebViewClient(mWebClient);
            Console.WriteLine(ConfigManager.WebService + "/demo2/demo.php?pk_id=" + pkid + "&vt_id=" + vtid + "&cl_id=1");
        }

        private void BtnEnd_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent);
        }
    }
}