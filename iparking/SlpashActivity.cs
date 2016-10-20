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
using System.Collections.Specialized;
using Newtonsoft.Json;
using iparking.Managment;

namespace iparking
{
    [Activity(Label = "iParking", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SlpashActivity : Activity
    {
        public const string errCode = "001";
        public const string errMsg = "Error al intentar Conectar con Servidor Central";

        FileManager fm;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            fm = new FileManager();

            string id = fm.GetValue("id");
            string email = fm.GetValue("email");
            string password = fm.GetValue("password"); 

            if (id == String.Empty || email == String.Empty || password == String.Empty)
            {
                // Redireccionar a Instrucciones
                Managment.ActivityManager.TakeMeTo(this, typeof(InstructionsFirstActivity), true);
            }
            else
            {
                // Realizo el login con las credenciales provistas
                login(email, password);
            }

        }

        public void login(string email, string password)
        {
            try
            {
                // Consulta Web Service con ese usuario y ese password
                System.Net.WebClient wclient = new System.Net.WebClient();
                Uri uri = new Uri(ConfigManager.WebService + "/authenticate.php");
                NameValueCollection param = new NameValueCollection();

                param.Add("email", email);
                param.Add("password", password);

                wclient.UploadValuesCompleted += Wclient_UploadValuesCompleted;
                wclient.UploadValuesAsync(uri, param);
            }
            catch (Exception ex)
            {
                Managment.ActivityManager.TakeMeTo(this, typeof(LoginActivity), true);
            }
        }

        private void Wclient_UploadValuesCompleted(object sender, System.Net.UploadValuesCompletedEventArgs e)
        {
            try
            {
                string json = Encoding.UTF8.GetString(e.Result);
                OperationResult or = JsonConvert.DeserializeObject<OperationResult>(json);

                if (or.error)
                {
                    // Existen los datos, pero no son correctos. Redirecciono a Login
                    fm.Clear();
                    Managment.ActivityManager.TakeMeTo(this, typeof(LoginActivity), true);
                }
                else
                {
                    // Existen los datos y son correctos. Redirecciono a Selector de Vehiculo
                    Managment.ActivityManager.TakeMeTo(this, typeof(VehicleActivity), true);
                }

            } catch(Exception ex)
            {
                Managment.ActivityManager.TakeMeTo(this, typeof(LoginActivity), true);
            }
        }
    }
}