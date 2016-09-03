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
    [Activity(Label = "LoginActivity", Theme = "@style/MyTheme.Base", NoHistory=true )]
    public class LoginActivity : Activity
    {
        Button mButtonLogin;
        TextView mNewUser;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);

            mButtonLogin = FindViewById<Button>(Resource.Id.btnLogin);
            mNewUser = FindViewById<TextView>(Resource.Id.txtNewUser);

            mButtonLogin.Click += MButtonLogin_Click;
            mNewUser.Click += MNewUser_Click;
        }

        private void MNewUser_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegisterUserActivity));
            this.StartActivity(intent);
            this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);
            this.Finish();
        }

        private void MButtonLogin_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Ingreso Nuevo Usuario");
        }
    }
}