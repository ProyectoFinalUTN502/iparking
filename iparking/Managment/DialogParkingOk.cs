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
    class DialogParkingOk : DialogFragment
    {
        private Button mButtonContinue;
        public event EventHandler<OnCancelEvent> mCancelEvent;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.DialogParkingOk, container, false);

            mButtonContinue = view.FindViewById<Button>(Resource.Id.buttonConfirm);
            mButtonContinue.Click += MButtonContinue_Click;

            return view;
        }

        private void MButtonContinue_Click(object sender, EventArgs e)
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