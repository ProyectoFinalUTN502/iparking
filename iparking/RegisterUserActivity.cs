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

namespace iparking
{
    [Activity(Label = "RegisterUserActivity", Theme = "@style/MyTheme.Base")]
    public class RegisterUserActivity : Activity
    {
        EditText mUserEmail;
        EditText mUserPassword;
        EditText mUserName;
        EditText mUserLastName;

        Button mButtonRegisterUser;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Register);

            mUserEmail = FindViewById<EditText>(Resource.Id.txtUserEmail);
            mUserPassword = FindViewById<EditText>(Resource.Id.txtUserPassword);
            mUserName = FindViewById<EditText>(Resource.Id.txtUserName);
            mUserLastName = FindViewById<EditText>(Resource.Id.txtUserLastName);

            mButtonRegisterUser = FindViewById<Button>(Resource.Id.btnRegisterUser);
            mButtonRegisterUser.Click += MButtonRegisterUser_Click;

        }

        private void MButtonRegisterUser_Click(object sender, EventArgs e)
        {
            bool result = true;  // Aca hay que hacer el control y validacion de campos

            if (result)
            {
                Intent intent = new Intent(this, typeof(RegisterProfileActivity));
                this.StartActivity(intent);
                this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);
            }
        }
    }
}