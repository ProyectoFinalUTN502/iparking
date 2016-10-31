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
    class DialogNoPosition : DialogFragment
    {
        private Button mButtonCancel;
        public event EventHandler<OnCancelEvent> mCancelEvent;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.DialogNoPosition, container, false);

            mButtonCancel = view.FindViewById<Button>(Resource.Id.buttonCancel);
            mButtonCancel.Click += MButtonCancel_Click;

            return view;
        }

        private void MButtonCancel_Click(object sender, EventArgs e)
        {
            mCancelEvent.Invoke(this, new OnCancelEvent());
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