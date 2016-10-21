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

namespace iparking.Managment
{
    class DialogParkingHistoric : DialogFragment
    {
        private Historic mHistory; 

        private TextView mTextName;
        private TextView mTextAddress;
        private TextView mTextDate;
        private TextView mTextVehicle;

        private Button mButtonAccept;
       
        public DialogParkingHistoric(Historic h)
        {
            mHistory = h;
        }

        private void LoadHistory()
        {
            mTextName.Text = mHistory.parkinglot;
            mTextAddress.Text = mHistory.address;
            mTextDate.Text = mHistory.date.ToString("dd.MM.yyyy HH:mm");
            mTextVehicle.Text = mHistory.vehicle;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.DialogHistoryDetail, container, false);
            
            mTextName = view.FindViewById<TextView>(Resource.Id.textViewName);
            mTextAddress = view.FindViewById<TextView>(Resource.Id.textViewAddress);
            mTextDate = view.FindViewById<TextView>(Resource.Id.textViewDate);
            mTextVehicle = view.FindViewById<TextView>(Resource.Id.textViewCar);

            mButtonAccept = view.FindViewById<Button>(Resource.Id.buttonAccept);
            mButtonAccept.Click += MButtonAccept_Click;

            LoadHistory();

            return view;
        }

        private void MButtonAccept_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            // Le saco el Titulo al Dialog
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            // Le agrego las animaciones para mostrar y cerrar
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.DialogParkingSearchAnimations;
        }
    }
}