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
using System.Net;
using System.Collections.Specialized;

using iparking.Entities;
using iparking.Controllers;
using iparking.Managment;
using Newtonsoft.Json;

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
        ProgressBar mProgress;

        Button mButtonRegisterUser;

        Client client;

        FileManager fm;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Register);

            fm = new FileManager();

            mUserEmail = FindViewById<EditText>(Resource.Id.txtUserEmail);
            mUserPassword = FindViewById<EditText>(Resource.Id.txtUserPassword);
            mUserName = FindViewById<EditText>(Resource.Id.txtUserName);
            mUserLastName = FindViewById<EditText>(Resource.Id.txtUserLastName);
            mTextError = FindViewById<TextView>(Resource.Id.txtRegisterError);

            mProgress = FindViewById<ProgressBar>(Resource.Id.progressBarRegister);
            mProgress.Visibility = ViewStates.Invisible;

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

            if (ClientController.validate(client))
            {
                mProgress.Visibility = ViewStates.Visible;
                mTextError.Visibility = ViewStates.Invisible;
                // Enviar Info al Servidor
                RegisterClient();
            }
            else
            {
                mTextError.Text = "La informacion ingresada no es valida";
                mTextError.Visibility = ViewStates.Visible;
            }
        }

        public void RegisterClient()
        {
            try
            {
                System.Net.WebClient wclient = new System.Net.WebClient();
                Uri uri = new Uri(ConfigManager.WebService + "/newClient.php");
                NameValueCollection param = new NameValueCollection();

                param.Add("email", client.email);
                param.Add("password", client.HashPassword());
                param.Add("name", client.name);
                param.Add("lastName", client.lastName);
                param.Add("macAddress", DeviceManager.GetMacAdress(this));

                wclient.UploadValuesCompleted += Wclient_UploadValuesCompleted;
                wclient.UploadValuesAsync(uri, param);
            } 
            catch (Exception ex)
            {
                Managment.ActivityManager.ShowError(this, new Error("100", ex.Message));
            }
        }

        private void Wclient_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {
                string json = Encoding.UTF8.GetString(e.Result);
                OperationResult or = JsonConvert.DeserializeObject<OperationResult>(json);

                // Oculto la Progressbar
                RunOnUiThread(() => { mProgress.Visibility = ViewStates.Invisible;});

                if (or.error)
                {
                    // Ha ocurrido un error!
                    RunOnUiThread(() =>
                    {
                        mTextError.Text = "Ah ocurrido un error al realizar el registro\nPor favor, intente nuevamente mas tarde";
                        mTextError.Visibility = ViewStates.Visible;
                    });
                }
                else
                {
                    // Guardo el Id de mi nuevo Cliente
                    fm.SetValue("id", or.data.ToString());
                    fm.SetValue("email", client.email);
                    fm.SetValue("password", client.HashPassword());

                    // Cargo la vista de Registro de Perfil
                    Managment.ActivityManager.TakeMeTo(this, typeof(RegisterProfileActivity), false);
                }
            }
            catch (Exception ex)
            {
                Managment.ActivityManager.ShowError(this, new Error("100", ex.Message));
            }

        }
    }
}