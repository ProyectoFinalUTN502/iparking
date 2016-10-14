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
using System.Security.Cryptography;

namespace iparking.Entities
{
    public class Client
    {
        public int id { get; set; }
        public string macAddress { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }

        public string HashPassword()
        {
            SHA1 sh1 = SHA1.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(password);
            byte[] data = sh1.ComputeHash(bytes);

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            string result = sBuilder.ToString();
            return (result);


        }
    }
}