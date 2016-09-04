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

namespace iparking
{
    [Activity(Label = "RegisterUserActivity", Theme = "@style/MyTheme.Base")]
    public class RegisterUserActivity : Activity
    {
        EditText mUserEmail;
        EditText mUserPassword;
        EditText mUserName;
        EditText mUserLastName;
        TextView mTextError;

        Button mButtonRegisterUser;

        Client client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Register);

            mUserEmail = FindViewById<EditText>(Resource.Id.txtUserEmail);
            mUserPassword = FindViewById<EditText>(Resource.Id.txtUserPassword);
            mUserName = FindViewById<EditText>(Resource.Id.txtUserName);
            mUserLastName = FindViewById<EditText>(Resource.Id.txtUserLastName);
            mTextError = FindViewById<TextView>(Resource.Id.txtRegisterError);

            mButtonRegisterUser = FindViewById<Button>(Resource.Id.btnRegisterUser);
            mButtonRegisterUser.Click += MButtonRegisterUser_Click;

        }

        private void MButtonRegisterUser_Click(object sender, EventArgs e)
        {

            client = new Client();
            client.email = mUserEmail.Text.Trim();
            client.password = mUserPassword.Text.Trim();
            client.name = mUserName.Text.Trim();
            client.lastName = mUserLastName.Text.Trim();

            if (validateClient(client))
            {
                mTextError.Visibility = ViewStates.Invisible;
                Intent intent = new Intent(this, typeof(RegisterProfileActivity));
                this.StartActivity(intent);
                this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);
            }
            else
            {
                mTextError.Visibility = ViewStates.Visible;
            }
        }

        private bool validateClient(Client client)
        {
            bool result = true;

            
            if (client.email == String.Empty) { result = false;}
            if (!validateClientEmail(client.email)) { result = false; }
            if (client.password == String.Empty) { result = false; }
            if (client.name == String.Empty) { result = false; }
            if (client.lastName == String.Empty) { result = false; }

            return result;
        }

        private bool validateClientEmail(String email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }
    }
}