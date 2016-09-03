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
using System.Threading.Tasks;
using iparking.Entities;

namespace iparking
{
    [Activity(Label = "SlpashActivity", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SlpashActivity : Activity
    {
        //static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ISharedPreferences pref = Application.Context.GetSharedPreferences(ConfigManager.SharedFile, FileCreationMode.Private);
            ISharedPreferencesEditor edit = pref.Edit();

            string email = pref.GetString("email", String.Empty);
            string password = pref.GetString("password", String.Empty);

            if (email == String.Empty || password == String.Empty)
            {
                // Redireccionar a Instrucciones
                Intent intent = new Intent(this, typeof(InstructionsFirstActivity));
                this.StartActivity(intent);
            }
            else
            {
                // Consulta Web Service con ese usuario y ese password
                bool response = true;


                if (response)
                {
                    // Todo esta bien, Redireccionar a Seleccion de Vehiculo
                    //Intent intent = new Intent(this, typeof(MainActivity));
                    //this.StartActivity(intent);
                    //this.Finish();
                }
                else
                {
                    // Existen los datos, pero no son correctos. Redirecciono a Login
                    edit.Clear();
                    edit.Apply();

                    //Intent intent = new Intent(this, typeof(RegisterActivity));
                    //this.StartActivity(intent);
                }
            }

        }

    }
}