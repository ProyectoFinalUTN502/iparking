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
    [Activity(Label = "MoreOptionsActivity", Theme = "@style/MyTheme.Base")]
    public class MoreOptionsActivity : Activity
    {
        ImageView mBack;
        TextView mEditProfile;
        TextView mNewLocation;
        TextView mNewSearch;
        TextView mHistory;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MoreOptions);

            mBack = FindViewById<ImageView>(Resource.Id.imageViewBack);
            mEditProfile = FindViewById<TextView>(Resource.Id.textViewEditProfile);
            mNewLocation = FindViewById<TextView>(Resource.Id.textViewNewLocation);
            mNewSearch = FindViewById<TextView>(Resource.Id.textViewNewSearch);
            mHistory = FindViewById<TextView>(Resource.Id.textViewHistory);

            mBack.Click += MBack_Click;
            mEditProfile.Click += MEditProfile_Click;
            mHistory.Click += MHistory_Click;
            mNewLocation.Click += MNewLocation_Click;
            mNewSearch.Click += MNewSearch_Click;
        }

        private void MNewSearch_Click(object sender, EventArgs e)
        {
            // CU: Buscar Establecimiento Por Parametros
            Managment.ActivityManager.TakeMeTo(this, typeof(NewSearchActivity), false);
        }

        private void MNewLocation_Click(object sender, EventArgs e)
        {
            // CU: Buscar Establecimiento en Mapa
            Managment.ActivityManager.TakeMeTo(this, typeof(MainSearchLocationActivity), false);
        }

        private void MHistory_Click(object sender, EventArgs e)
        {
            //Intent intent = new Intent(this, typeof(HistoryActivity));
            //this.StartActivity(intent);
            //this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);
            //this.Finish();
        }

        private void MEditProfile_Click(object sender, EventArgs e)
        {
            // Editar el Perfil actual del Cliente
            Managment.ActivityManager.TakeMeTo(this, typeof(EditProfileActivity), false);
        }

        private void MBack_Click(object sender, EventArgs e)
        {
            // Volver a Pantalla Principal
            Managment.ActivityManager.TakeMeTo(this, typeof(MainActivity), true);
        }
    }
}