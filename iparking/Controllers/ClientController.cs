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
using System.Text.RegularExpressions;

namespace iparking.Controllers
{
    class ClientController
    {
        public static bool validate(Client client)
        {
            bool result = true;


            if (client.email == String.Empty) { result = false; }
            if (!validateClientEmail(client.email)) { result = false; }
            if (client.password == String.Empty) { result = false; }
            if (client.name == String.Empty) { result = false; }
            if (client.lastName == String.Empty) { result = false; }

            return result;
        }

        private static bool validateClientEmail(String email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        
    }
}