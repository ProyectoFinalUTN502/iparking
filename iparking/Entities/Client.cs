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
    public class Client
    {
        public int id { get; set; }
        public string key { get; set; }
        public string macAddress { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
    }
}