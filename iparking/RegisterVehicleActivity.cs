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
using iparking.Managment;
using iparking.Entities;
using iparking.Controllers;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace iparking
{
    [Activity(Label = "RegisterVehicleActivity", Theme = "@style/MyTheme.Base")]
    public class RegisterVehicleActivity : Activity
    {
        RadioButton mRadioCar;
        RadioButton mRadioSUV;
        RadioButton mRadioVAN;
        RadioButton mRadioMotorcicle;
        TextView mTextError;
        EditText mName;

        Button mButtonFinish;

        FileManager fm;

        Vehicle vehicle;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RegisterVehicle);

            fm = new FileManager();

            mRadioCar = FindViewById<RadioButton>(Resource.Id.radioButtonCar);
            mRadioSUV = FindViewById<RadioButton>(Resource.Id.radioButtonSuv);
            mRadioVAN = FindViewById<RadioButton>(Resource.Id.radioButtonVan);
            mRadioMotorcicle = FindViewById<RadioButton>(Resource.Id.radioButtonMotorcicle);

            mTextError = FindViewById<TextView>(Resource.Id.textViewError);
            mTextError.Visibility = ViewStates.Invisible;

            mName = FindViewById<EditText>(Resource.Id.editTextName);

            mButtonFinish = FindViewById<Button>(Resource.Id.btnRegisterAll);
            mButtonFinish.Click += MButtonFinish_Click;

        }

        private void MButtonFinish_Click(object sender, EventArgs e)
        {
            vehicle = new Vehicle();
            vehicle.name = mName.Text.Trim();
            
            if (mRadioVAN.Checked) { vehicle.vehicleTypeID = 1; }
            if (mRadioSUV.Checked) { vehicle.vehicleTypeID = 2; }
            if (mRadioCar.Checked) { vehicle.vehicleTypeID = 3; }
            if (mRadioMotorcicle.Checked) { vehicle.vehicleTypeID = 4; }

            if (VehicleController.validate(vehicle))
            {
                mTextError.Visibility = ViewStates.Visible;
                mTextError.Text = "La informacion ingresada no es valida";
            }
            else
            {
                mTextError.Visibility = ViewStates.Invisible;
                mTextError.Text = "";

                createVehicle();
            }

            
        }

        public void createVehicle()
        {
            System.Net.WebClient wclient = new System.Net.WebClient();
            Uri uri = new Uri(ConfigManager.WebService + "/newVehicle.php");
            NameValueCollection param = new NameValueCollection();

            param.Add("name", vehicle.name);
            param.Add("client_id", fm.GetValue("id"));
            param.Add("vehicle_type_id", vehicle.vehicleTypeID.ToString());


            wclient.UploadValuesCompleted += Wclient_UploadValuesCompleted1;
            wclient.UploadValuesAsync(uri, param);
        }

        private void Wclient_UploadValuesCompleted1(object sender, System.Net.UploadValuesCompletedEventArgs e)
        {
            string json = Encoding.UTF8.GetString(e.Result);
            OperationResult or = JsonConvert.DeserializeObject<OperationResult>(json);
            
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
                Intent intent = new Intent(this, typeof(MainActivity));
                this.StartActivity(intent);
                this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);
                this.Finish();
            }

        }
    }
}