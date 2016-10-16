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

namespace iparking
{
    [Activity(Label = "NewSearchActivity", Theme = "@style/MyTheme.Base")]
    public class NewSearchActivity : Activity
    {
        ImageView mBack;

        SeekBar mSeekBarRange;
        TextView mTextViewRange;
        EditText mTextPrice;

        RadioButton mIs24Yes;
        RadioButton mIs24No;

        RadioButton mIsCoveredYes;
        RadioButton mIsCoveredNo;

        Button mSearch;

        FileManager fm;
        System.Net.WebClient mClient;

        Profile mProfile;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.NewSearch);

            fm = new FileManager();
            mClient = new System.Net.WebClient();
            mProfile = new Profile();

            mSeekBarRange = FindViewById<SeekBar>(Resource.Id.seekBarRange);
            mTextViewRange = FindViewById<TextView>(Resource.Id.textViewRange);

            mTextPrice = FindViewById<EditText>(Resource.Id.textPrice);

            mIs24Yes = FindViewById<RadioButton>(Resource.Id.radioButtonIs24Yes);
            mIs24No = FindViewById<RadioButton>(Resource.Id.radioButtonIs24No);

            mIsCoveredYes = FindViewById<RadioButton>(Resource.Id.radioButtonIsCoveredYes);
            mIsCoveredNo = FindViewById<RadioButton>(Resource.Id.radioButtonIsCoveredNo);

            mBack = FindViewById<ImageView>(Resource.Id.imageViewBack);
            mSearch = FindViewById<Button>(Resource.Id.btnNewSearch);

            mSeekBarRange.ProgressChanged += MSeekBarRange_ProgressChanged;
            mBack.Click += MBack_Click;
            mSearch.Click += MSearch_Click;
        }

        private void MSeekBarRange_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            int value = e.Progress + 1;
            string progress = value.ToString();
            mTextViewRange.Text = "Rango de Búsqueda " + progress + " Km";
        }

        private void MSearch_Click(object sender, EventArgs e)
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

            // Aca va a la MainNewSearchActivity
            Managment.ActivityManager.TakeMeTo(this, typeof(MainNewSearchActivity), false, mProfile);
        }

        private void MBack_Click(object sender, EventArgs e)
        {
            Managment.ActivityManager.TakeMeTo(this, typeof(MoreOptionsActivity), true);
        }
    }
}