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
using Android.Webkit;

using iparking.Managment;
using iparking.Entities;

namespace iparking
{
    [Activity(Label = "NavigationActivity", Theme = "@style/MyTheme.Base")]
    public class NavigationActivity : Activity
    {
        private WebView mWebView;
        private ProgressBar mProgressBar;
        private WebClient mWebClient;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Navigation);

            mWebClient = new WebClient();

            mWebClient.mOnProgressBarChanged += (int state) =>
            {
                if (state == 0)
                {
                    // termino, ocultar la progress
                    mProgressBar.Visibility = ViewStates.Invisible;
                }
                else
                {
                    mProgressBar.Visibility = ViewStates.Visible;
                }
            };

            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            mWebView = FindViewById<WebView>(Resource.Id.webView);

            mWebView.Settings.JavaScriptEnabled = true;
            mWebView.LoadUrl(ConfigManager.WebService + "/parkinglot/map/2");
            mWebView.SetWebViewClient(mWebClient);
        }
    }

}