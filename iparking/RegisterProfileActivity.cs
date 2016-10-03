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
using iparking.Controllers;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace iparking
{
    [Activity(Label = "RegisterProfileActivity", Theme = "@style/MyTheme.Base")]
    public class RegisterProfileActivity : Activity
    {
        Profile clientProfile;
        SeekBar mSeekBarRange;
        TextView mTextViewRange;
        TextView mTextError;
        EditText mTextPrice;

        RadioButton mIs24Yes;
        RadioButton mIs24No;

        RadioButton mIsCoveredYes;
        RadioButton mIsCoveredNo;

        Button mButton;

        ISharedPreferences pref;
        ISharedPreferencesEditor edit;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RegisterProfile);

            mSeekBarRange = FindViewById<SeekBar>(Resource.Id.seekBarRange);
            mTextViewRange = FindViewById<TextView>(Resource.Id.textViewRange);
            mTextError = FindViewById<TextView>(Resource.Id.textViewError);
            mTextError.Visibility = ViewStates.Invisible;

            mTextPrice = FindViewById<EditText>(Resource.Id.textPrice);

            mIs24Yes = FindViewById<RadioButton>(Resource.Id.radioButtonIs24Yes);
            mIs24No = FindViewById<RadioButton>(Resource.Id.radioButtonIs24No);

            mIsCoveredYes = FindViewById<RadioButton>(Resource.Id.radioButtonIsCoveredYes);
            mIsCoveredNo = FindViewById<RadioButton>(Resource.Id.radioButtonIsCoveredNo);

            mButton = FindViewById<Button>(Resource.Id.btnRegisterProfile);

            mButton.Click += MButton_Click;
            mSeekBarRange.ProgressChanged += MSeekBarRange_ProgressChanged;

        }

        private void MButton_Click(object sender, EventArgs e)
        {
            int range = mSeekBarRange.Progress + 1;
            string price = mTextPrice.Text.Trim();
            int is24 = mIs24Yes.Checked ? 1 : 0;
            int isCovered = mIsCoveredYes.Checked ? 1 : 0;

            clientProfile = new Profile();
            clientProfile.range = range;
            clientProfile.maxPrice = price == string.Empty ? 0.0 : Convert.ToDouble(mTextPrice.Text.Trim());
            clientProfile.is24 = is24;
            clientProfile.isCovered = isCovered;
            
            if (ProfileController.validate(clientProfile))
            {
                // Enviar al servidor
                createProfile();
            }
            else
            {
                // Mostrar Mensaje de error
                mTextError.Visibility = ViewStates.Visible;
                mTextError.Text = "La informacion ingresada no es valida";
            }
        }

        private void MSeekBarRange_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            int value = e.Progress + 1;
            string progress = value.ToString();
            mTextViewRange.Text = "Rango de Búsqueda " + progress + " Km";
        }

        public void createProfile()
        {
            System.Net.WebClient wclient = new System.Net.WebClient();
            Uri uri = new Uri(ConfigManager.WebService + "/newProfile.php");

            pref = Application.Context.GetSharedPreferences(ConfigManager.SharedFile, FileCreationMode.Private);
            edit = pref.Edit();

            string id = pref.GetString("id", String.Empty);

            NameValueCollection param = new NameValueCollection();
            param.Add("range", clientProfile.range.ToString());
            param.Add("maxPrice", clientProfile.maxPrice.ToString());
            param.Add("is24", clientProfile.is24.ToString());
            param.Add("isCovered", clientProfile.isCovered.ToString());
            param.Add("client_id", id);

            wclient.UploadValuesCompleted += Wclient_UploadValuesCompleted; ;
            wclient.UploadValuesAsync(uri, param);
        }

        private void Wclient_UploadValuesCompleted(object sender, System.Net.UploadValuesCompletedEventArgs e)
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
                // Cargo la vista de Registro de Vehiculo
                Intent intent = new Intent(this, typeof(RegisterVehicleActivity));
                this.StartActivity(intent);
                this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);
            }
        }
    }
}