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

namespace iparking
{
    [Activity(Label = "iParking", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SlpashActivity : Activity
    {
        ISharedPreferences pref;
        ISharedPreferencesEditor edit;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            pref = Application.Context.GetSharedPreferences(ConfigManager.SharedFile, FileCreationMode.Private);
            edit = pref.Edit();

            string id = pref.GetString("id", String.Empty);
            string email = pref.GetString("email", String.Empty);
            string password = pref.GetString("password", String.Empty);

            if (id == String.Empty || email == String.Empty || password == String.Empty)
            {
                // Redireccionar a Instrucciones
                Intent intent = new Intent(this, typeof(InstructionsFirstActivity));
                this.StartActivity(intent);
                this.Finish();
            }
            else
            {
                // Realizo el login con las credenciales provistas
                login(email, password);
            }

        }

        public void login(string email, string password)
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

        private void Wclient_UploadValuesCompleted(object sender, System.Net.UploadValuesCompletedEventArgs e)
        {
            string json = Encoding.UTF8.GetString(e.Result);
            OperationResult or = JsonConvert.DeserializeObject<OperationResult>(json);

            if (or.error)
            {
                // Existen los datos, pero no son correctos. Redirecciono a Login
                edit.Clear();
                edit.Apply();

                Intent intent = new Intent(this, typeof(LoginActivity));
                this.StartActivity(intent);
                this.Finish();
            }
            else
            {
                // Existen los datos y son correctos. Redirecciono a Main
                Intent intent = new Intent(this, typeof(MainActivity));
                this.StartActivity(intent);
                this.Finish();
            }
        }
    }
}