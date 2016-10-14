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
using Android.Graphics;

namespace iparking.Managment
{
    public class WebClient : WebViewClient
    {
        public delegate void ToggleProgressBar(int state);
        public ToggleProgressBar mOnProgressBarChanged;

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            // El view es del Web View
            view.LoadUrl(url);
            return true;
        }

        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            if (mOnProgressBarChanged != null)
            {
                mOnProgressBarChanged.Invoke(1);
            }
            base.OnPageStarted(view, url, favicon);
        }

        public override void OnPageFinished(WebView view, string url)
        {
            if (mOnProgressBarChanged != null)
            {
                mOnProgressBarChanged.Invoke(0);
            }
            base.OnPageFinished(view, url);
        }
    }
}