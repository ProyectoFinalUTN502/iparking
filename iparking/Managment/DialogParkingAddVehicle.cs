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

namespace iparking.Managment
{
    class DialogParkingAddVehicle : DialogFragment
    {
        EditText mName;
        RadioButton mRadioCar;
        RadioButton mRadioSUV;
        RadioButton mRadioVAN;
        RadioButton mRadioMotorcicle;
        TextView mTextError;

        Button mButtonAdd;

        Vehicle mVehicle;

        public event EventHandler<OnAddEvent> mAddEvent;

        public DialogParkingAddVehicle()
        {
            mVehicle = new Vehicle();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            // Le saco el Titulo al Dialog
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            // Le agrego las animaciones para mostrar y cerrar
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.DialogParkingSearchAnimations;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.DialogAddVehicle, container, false);

            mRadioCar = view.FindViewById<RadioButton>(Resource.Id.radioButtonCar);
            mRadioSUV = view.FindViewById<RadioButton>(Resource.Id.radioButtonSuv);
            mRadioVAN = view.FindViewById<RadioButton>(Resource.Id.radioButtonVan);
            mRadioMotorcicle = view.FindViewById<RadioButton>(Resource.Id.radioButtonMotorcicle);

            mTextError = view.FindViewById<TextView>(Resource.Id.textViewError);
            mTextError.Visibility = ViewStates.Invisible;

            mName = view.FindViewById<EditText>(Resource.Id.editTextName);

            mButtonAdd = view.FindViewById<Button>(Resource.Id.btnAddVehicle);
            mButtonAdd.Click += MButtonAdd_Click;

            return view;
        }

        private void MButtonAdd_Click(object sender, EventArgs e)
        {
            mVehicle.name = mName.Text.Trim();

            if (mRadioVAN.Checked) { mVehicle.vehicleTypeID = 1; }
            if (mRadioSUV.Checked) { mVehicle.vehicleTypeID = 2; }
            if (mRadioCar.Checked) { mVehicle.vehicleTypeID = 3; }
            if (mRadioMotorcicle.Checked) { mVehicle.vehicleTypeID = 4; }

            if (VehicleController.validate(mVehicle))
            {
                mTextError.Visibility = ViewStates.Visible;
                mTextError.Text = "La informacion ingresada no es valida";

                mAddEvent.Invoke(this, new OnAddEvent(null));
            }
            else
            {
                mTextError.Visibility = ViewStates.Invisible;
                mTextError.Text = "";

                mAddEvent.Invoke(this, new OnAddEvent(mVehicle));
                this.Dismiss();
            }


        }
    }

    public class OnAddEvent : EventArgs
    {
        public Vehicle mVehicle;

        public OnAddEvent(Vehicle v) : base()
        {
            mVehicle = v;
        }

        public Vehicle Vehicle
        {
            get { return mVehicle; }
            set { mVehicle = value; }
        }
    }
}