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
using Newtonsoft.Json;

namespace iparking
{
    [Activity(Label = "NavigationActivity", Theme = "@style/MyTheme.Base")]
    public class NavigationActivity : Activity
    {
        private const string errCode = "900";
        private const string errMsg = "No se ha podido iniciar la Navegacion Interna";

        private WebView mWebView;
        private ProgressBar mProgressBar;
        private Button mButtonCancel;
        private WebClient mWebClient;
        private List<int> mNavData;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Navigation);

            mButtonCancel = FindViewById<Button>(Resource.Id.buttonCancel);
            mButtonCancel.Click += MButtonCancel_Click;

            try
            {
                string data = Intent.GetStringExtra("data");
                mNavData = JsonConvert.DeserializeObject<List<int>>(data);

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

                string param = "pk_id=" + mNavData[0] + "&vt_id=" + mNavData[1] + "&cl_id=" + mNavData[2];
                mWebView.LoadUrl(ConfigManager.WebService + "/navigation.php?" + param);
                mWebView.SetWebViewClient(mWebClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine("** Error ** : " + ex.ToString());
                Managment.ActivityManager.ShowError(this, new Error(errCode, errMsg));
            }

        }

        private void MButtonCancel_Click(object sender, EventArgs e)
        {
            // Levantar Dialog de Confirmacion para Cancelar
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            DialogParkingCancel dialogCancel = new DialogParkingCancel();
            dialogCancel.Show(trans, "Dialog Cancel");
            dialogCancel.mCancelEvent += DialogCancel_mCancelEvent;

        }

        private void DialogCancel_mCancelEvent(object sender, OnCancelEvent e)
        {
            Managment.ActivityManager.TakeMeTo(this, typeof(MainActivity), true);   
        }
    }

}