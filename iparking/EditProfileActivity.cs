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
using Newtonsoft.Json;
using iparking.Controllers;
using System.Collections.Specialized;

namespace iparking
{
    [Activity(Label = "EditProfileActivity", Theme = "@style/MyTheme.Base")]
    public class EditProfileActivity : Activity
    {
        SeekBar mSeekBarRange;
        TextView mTextViewRange;
        EditText mTextPrice;

        RadioButton mIs24Yes;
        RadioButton mIs24No;

        RadioButton mIsCoveredYes;
        RadioButton mIsCoveredNo;

        Button mButtonEdit;

        FileManager fm;
        System.Net.WebClient mClient;
        Profile mProfile;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
                        
            SetContentView(Resource.Layout.EditProfile);

            mSeekBarRange = FindViewById<SeekBar>(Resource.Id.seekBarRange);
            mTextViewRange = FindViewById<TextView>(Resource.Id.textViewRange);

            mTextPrice = FindViewById<EditText>(Resource.Id.textPrice);

            mIs24Yes = FindViewById<RadioButton>(Resource.Id.radioButtonIs24Yes);
            mIs24No = FindViewById<RadioButton>(Resource.Id.radioButtonIs24No);

            mIsCoveredYes = FindViewById<RadioButton>(Resource.Id.radioButtonIsCoveredYes);
            mIsCoveredNo = FindViewById<RadioButton>(Resource.Id.radioButtonIsCoveredNo);

            mButtonEdit = FindViewById<Button>(Resource.Id.btnEditProfile);

            mSeekBarRange.ProgressChanged += MSeekBarRange_ProgressChanged;
            mButtonEdit.Click += MButton_Click;

            fm = new FileManager();
            mClient = new System.Net.WebClient();

            LoadClientProfile();
        }

        private void MSeekBarRange_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            int value = e.Progress + 1;
            string progress = value.ToString();
            mTextViewRange.Text = "Rango de Búsqueda " + progress + " Km";
        }

        private void MButton_Click(object sender, EventArgs e)
        {
            int range = mSeekBarRange.Progress + 1;
            string price = mTextPrice.Text.Trim();
            int is24 = mIs24Yes.Checked ? 1 : 0;
            int isCovered = mIsCoveredYes.Checked ? 1 : 0;

            if (price == string.Empty)
            {
                price = "100";
                mTextPrice.Text = price;
            }
            
            mProfile.range = range;
            mProfile.maxPrice = Convert.ToDouble(price);
            mProfile.is24 = is24;
            mProfile.isCovered = isCovered;

            UpdateClientProfile();
        }

        public void LoadClientProfile()
        {
            string clientID = fm.GetValue("id");

            string urlRef = "client_id=" + clientID;
            mClient = new System.Net.WebClient();
            Uri url = new Uri(ConfigManager.WebService + "/" + "searchClientProfile.php?" + urlRef);

            mClient.DownloadDataAsync(url);
            mClient.DownloadDataCompleted += MClient_DownloadDataCompleted; ;
        }

        public void UpdateClientProfile()
        {
            string id = fm.GetValue("id");
            System.Net.WebClient wclient = new System.Net.WebClient();
            Uri uri = new Uri(ConfigManager.WebService + "/updProfile.php?client_id=" + id);

            NameValueCollection param = new NameValueCollection();
            param.Add("range", mProfile.range.ToString());
            param.Add("maxPrice", mProfile.maxPrice.ToString());
            param.Add("is24", mProfile.is24.ToString());
            param.Add("isCovered", mProfile.isCovered.ToString());
            param.Add("client_id", id);

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
                    // Ha ocurrido un error!
                    Managment.ActivityManager.TakeMeTo(this, typeof(ErrorActivity), true);
                }
                else
                {
                    // Vuelvo a la Pagina de Opciones
                    Managment.ActivityManager.TakeMeTo(this, typeof(MoreOptionsActivity), true);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("** Error de Actualizacion ( Perfil ) ** : " + ex.Message);
                Managment.ActivityManager.TakeMeTo(this, typeof(ErrorActivity), true);
            }
        }

        private void MClient_DownloadDataCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
                try
                {
                    string json = Encoding.UTF8.GetString(e.Result);
                    mProfile = JsonConvert.DeserializeObject<Profile>(json);
                    
                    if (mProfile == null)
                    {
                        Console.WriteLine("** Error de Carga ( Perfil de Busqueda ) ** : No existe perfil para ese usuario");
                        Managment.ActivityManager.TakeMeTo(this, typeof(ErrorActivity), true);
                        return;
                    }

                    RunOnUiThread(() => { 
                        mSeekBarRange.Progress = mProfile.range;
                        mTextViewRange.Text = "Rango de Búsqueda " + mProfile.range.ToString() + " Km";
                        mTextPrice.Text = mProfile.maxPrice.ToString();

                        if (mProfile.GetIs24())
                        {
                            mIs24Yes.Checked = true;
                            mIs24No.Checked = false;
                        } else
                        {
                            mIs24Yes.Checked = false;
                            mIs24No.Checked = true;
                        }

                        if (mProfile.GetIsCovered())
                        {
                            mIsCoveredYes.Checked = true;
                            mIsCoveredNo.Checked = false;
                        } else
                        {
                            mIsCoveredYes.Checked = false;
                            mIsCoveredNo.Checked = true;
                        }
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("** Error de Carga ( Perfil ) ** : " + ex.Message);
                    Managment.ActivityManager.TakeMeTo(this, typeof(ErrorActivity), true);
                }
           
        }
    }
}