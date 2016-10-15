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
    class DialogParkingEnter : DialogFragment
    {
        private int ParkinglotID;
        private int VehicleTypeID;
        private int ClientID;

        private Button mButtonEnter;

        public event EventHandler<OnEnterEventArgs> mEnterEvent;

        public DialogParkingEnter(int parkingID, int vehicleTypeID, int clientID)
        {
            ParkinglotID = parkingID;
            VehicleTypeID = vehicleTypeID;
            ClientID = clientID;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view;

            view = inflater.Inflate(Resource.Layout.DialogParkingEnter, container, false);

            mButtonEnter = view.FindViewById<Button>(Resource.Id.buttonEnter);
            mButtonEnter.Click += MButtonEnter_Click;

            return view;
        }

        private void MButtonEnter_Click(object sender, EventArgs e)
        {
            mEnterEvent.Invoke(this, new OnEnterEventArgs(ParkinglotID, VehicleTypeID, ClientID));   
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

    public class OnEnterEventArgs : EventArgs
    {
        private int mParkinglotID;
        private int mVehicleTypeID;
        private int mClientID;

        public int ParkinglotID
        {
            get { return mParkinglotID; }
            set { mParkinglotID = value; }
        }

        public int VehicleTypeID
        {
            get { return mVehicleTypeID; }
            set { mVehicleTypeID = value; }
        }

        public int ClientID
        {
            get { return mClientID; }
            set { mClientID = value; }
        }

        public OnEnterEventArgs(int p, int vt, int c) : base()
        {
            mParkinglotID = p;
            mVehicleTypeID = vt;
            mClientID = c;
        }
    }
}