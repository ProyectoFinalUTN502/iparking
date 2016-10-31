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
    //class DialogReRouting : DialogFragment
    //{
    //    private Button mButtonReRouting;

    //    public event EventHandler<OnRoutingEvent> mRoutingEvent;

    //    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    //    {
    //        base.OnCreateView(inflater, container, savedInstanceState);
    //        View view = inflater.Inflate(Resource.Layout.DialogReRouting, container, false);

    //        mButtonReRouting = view.FindViewById<Button>(Resource.Id.buttonReRouting);
    //        mButtonReRouting.Click += MButtonReRouting_Click;

    //        return view;
    //    }

    //    private void MButtonReRouting_Click(object sender, EventArgs e)
    //    {
    //        mRoutingEvent.Invoke(this, new OnRoutingEvent());
    //        this.Dismiss();
    //    }

    //    public override void OnActivityCreated(Bundle savedInstanceState)
    //    {
    //        // Le saco el Titulo al Dialog
    //        Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
    //        base.OnActivityCreated(savedInstanceState);
    //        // Le agrego las animaciones para mostrar y cerrar
    //        Dialog.Window.Attributes.WindowAnimations = Resource.Style.DialogParkingSearchAnimations;
    //    }
    //}

    //public class OnRoutingEvent : EventArgs
    //{
    //    public OnRoutingEvent() : base() { }
    //}
}