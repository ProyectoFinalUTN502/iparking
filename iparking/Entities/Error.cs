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

namespace iparking.Entities
{
    class Error
    {
        private string mCode;
        private string mMessage;

        public string Code
        {
            get { return mCode; }
            set { mCode = value; }
        }

        public string Message
        {
            get { return mMessage; }
            set { mMessage = value; }
        }

        // Constructor
        public Error(string code, string message) 
        {
            mCode = code;
            mMessage = message;
        }
    }
}