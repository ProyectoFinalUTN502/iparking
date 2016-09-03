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
    [Activity(Label = "RegisterProfileActivity", Theme = "@style/MyTheme.Base")]
    public class RegisterProfileActivity : Activity
    {
        SeekBar mSeekBarRange;
        TextView mTextViewRange;
        EditText mTextPrice;

        RadioButton mIs24Yes;
        RadioButton mIs24No;

        RadioButton mIsCoveredYes;
        RadioButton mIsCoveredNo;

        Button mButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RegisterProfile);

            mSeekBarRange = FindViewById<SeekBar>(Resource.Id.seekBarRange);
            mTextViewRange = FindViewById<TextView>(Resource.Id.textViewRange);

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
            bool result = true;  // Aca hay que hacer el control y validacion de campos

            if (result)
            {
                Intent intent = new Intent(this, typeof(RegisterVehicleActivity));
                this.StartActivity(intent);
                this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);
            }     
        }

        private void MSeekBarRange_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            int value = e.Progress + 1;
            string progress = value.ToString();
            mTextViewRange.Text = "Rango de Búsqueda " + progress + " Km";
        }
    }
}