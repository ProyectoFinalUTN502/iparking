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
    class DialogNavigationControl : DialogFragment
    {
        private Button mButtonRoute;
        private Button mButtonPark;

        public event EventHandler<OnParkingEvent> mParkingEvent;
        public event EventHandler<OnRoutingEvent> mRoutingEvent;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.DialogControl, container, false);

            mButtonPark = view.FindViewById<Button>(Resource.Id.buttonPark);
            mButtonRoute = view.FindViewById<Button>(Resource.Id.buttonRoute);

            mButtonPark.Click += MButtonPark_Click;
            mButtonRoute.Click += MButtonRoute_Click;

            return view;
        }

        private void MButtonRoute_Click(object sender, EventArgs e)
        {
            mRoutingEvent.Invoke(this, new OnRoutingEvent());
            this.Dismiss();
        }

        private void MButtonPark_Click(object sender, EventArgs e)
        {
            mParkingEvent.Invoke(this, new OnParkingEvent());
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

    public class OnParkingEvent : EventArgs
    {
        public OnParkingEvent() : base() { }
    }

    public class OnRoutingEvent : EventArgs
    {
        public OnRoutingEvent() : base() { }
    }
}