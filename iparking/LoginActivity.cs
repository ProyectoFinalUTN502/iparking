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
using System.Collections.Specialized;
using Newtonsoft.Json;
using iparking.Managment;

namespace iparking
{
    [Activity(Label = "LoginActivity", Theme = "@style/MyTheme.Base", NoHistory=true )]
    public class LoginActivity : Activity
    {
        TextView mTextUser;
        TextView mTextPassword;
        TextView mTextError;

        Button mButtonLogin;
        TextView mNewUser;

        Client mClient;
        FileManager mFile;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);

            mTextUser = FindViewById<TextView>(Resource.Id.txtUserEmail);
            mTextPassword = FindViewById<TextView>(Resource.Id.txtUserPassword);
            mTextError = FindViewById<TextView>(Resource.Id.textViewError);

            mButtonLogin = FindViewById<Button>(Resource.Id.btnLogin);
            mNewUser = FindViewById<TextView>(Resource.Id.txtNewUser);

            mButtonLogin.Click += MButtonLogin_Click;
            mNewUser.Click += MNewUser_Click;
        }

        private void MNewUser_Click(object sender, EventArgs e)
        {
            Managment.ActivityManager.TakeMeTo(this, typeof(RegisterUserActivity), true);
        }

        private void MButtonLogin_Click(object sender, EventArgs e)
        {
            mClient = new Client();
            mClient.email = mTextUser.Text.Trim();
            mClient.password = mTextPassword.Text.Trim();

            if (mClient.email == string.Empty || mClient.password == string.Empty)
            {
                // Mostra Cartel en la Interfaz
                mTextError.Visibility = ViewStates.Visible;
                return;
            }
            else
            {
                mTextError.Visibility = ViewStates.Invisible;
            }

            // Consulta Web Service con ese usuario y ese password
            System.Net.WebClient wclient = new System.Net.WebClient();
            Uri uri = new Uri(ConfigManager.WebService + "/authenticate.php");
            NameValueCollection param = new NameValueCollection();

            param.Add("email", mClient.email);
            param.Add("password", mClient.HashPassword());

            wclient.UploadValuesCompleted += Wclient_UploadValuesCompleted;
            wclient.UploadValuesAsync(uri, param);

            
        }

        private void Wclient_UploadValuesCompleted(object sender, System.Net.UploadValuesCompletedEventArgs e)
        {
            try
            {
                string json = Encoding.UTF8.GetString(e.Result);
                OperationResult or = JsonConvert.DeserializeObject<OperationResult>(json);

                if (or.error)
                {
                    // El Login dio mal
                    mTextError.Visibility = ViewStates.Visible;

                }
                else
                {
                    // Login Correcto, Redirecciono a Selector de Vehiculo
                    mFile = new Managment.FileManager();
                    mFile.SetValue("id", or.data.ToString());
                    mFile.SetValue("email", mClient.email);
                    mFile.SetValue("password", mClient.HashPassword());

                    Managment.ActivityManager.TakeMeTo(this, typeof(VehicleActivity), true);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(" ** ERROR ** : " + ex.Message);
                Managment.ActivityManager.TakeMeTo(this, typeof(ErrorActivity), true);
            }
        }
    }
}