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

namespace iparking.Managment
{
    class DialogDelVehicle : DialogFragment
    {
        private int mPosition;
        private Button mButtonContinue;
        public event EventHandler<OnDeleteEvents> mDeleteEvent;


        public DialogDelVehicle(int position)
        {
            mPosition = position;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.DialogDelVehicle, container, false);

            mButtonContinue = view.FindViewById<Button>(Resource.Id.buttonDelete);
            mButtonContinue.Click += MButtonContinue_Click;

            return view;
        }

        private void MButtonContinue_Click(object sender, EventArgs e)
        {
            mDeleteEvent.Invoke(this, new OnDeleteEvents(mPosition) );
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

    public class OnDeleteEvents : EventArgs
    {
        private int mPosition;

        public int Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        // Constructor
        public OnDeleteEvents(int p) : base()
        {
            mPosition = p;
        }
    }
}