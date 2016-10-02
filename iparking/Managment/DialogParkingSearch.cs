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
    class DialogParkingSearch : DialogFragment
    {
        private int mPosition;
        private Parkinglot mParkinglot;

        private TextView mTextName;
        private TextView mTextAddress;
        private TextView mTextTime;
        private TextView mTextPrice;
        private TextView mTextPositions;

        private Button mButtonGo;
        private Button mButtonNext;
        private Button mButtonErrorClose;

        private bool mLoadErrorView;

        public event EventHandler<OnGoEventArgs> mGo;
        public event EventHandler<OnNextEventArgs> mNext;


        public DialogParkingSearch()
        {
            mParkinglot = null;
            mPosition = 0;
            mLoadErrorView = true;
        }

        public DialogParkingSearch(Parkinglot parkinglot, int position)
        {
            mParkinglot = parkinglot;
            mPosition = position;
            mLoadErrorView = false;
        }

        public void SetParkinglot(Parkinglot parkinglot, int position)
        {
            mParkinglot = parkinglot;
            mPosition = position;

            loadParkinglot();
        }

        private void loadParkinglot()
        {
            mTextName.Text = mParkinglot.name;
            mTextAddress.Text = mParkinglot.address;

            if (mParkinglot.is24Open())
            {
                mTextTime.Text = "Abierto las 24 Hs.";
            }
            else
            {
                mTextTime.Text = mParkinglot.openTime.ToString("HH:mm") + " a " + mParkinglot.closeTime.ToString("HH:mm");
            }

            mTextPrice.Text = "$" + mParkinglot.price.ToString();
            mTextPositions.Text = mParkinglot.positions.ToString();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view;

            if (mLoadErrorView)
            {
                view = inflater.Inflate(Resource.Layout.DialogParkingSearchError, container, false);
                mButtonErrorClose = view.FindViewById<Button>(Resource.Id.buttonError);
                mButtonErrorClose.Click += MButtonErrorClose_Click;
            } 
            else
            {
                view = inflater.Inflate(Resource.Layout.DialogParkingSearch, container, false);

                mTextName = view.FindViewById<TextView>(Resource.Id.textViewName);
                mTextAddress = view.FindViewById<TextView>(Resource.Id.textViewAddress);
                mTextTime = view.FindViewById<TextView>(Resource.Id.textViewTime);
                mTextPrice = view.FindViewById<TextView>(Resource.Id.textViewPrice);
                mTextPositions = view.FindViewById<TextView>(Resource.Id.textViewPositions);
           
                loadParkinglot();

                mButtonGo = view.FindViewById<Button>(Resource.Id.buttonGo);
                mButtonNext = view.FindViewById<Button>(Resource.Id.buttonNext);

                mButtonGo.Click += MButtonGo_Click;
                mButtonNext.Click += MButtonNext_Click;

            }


            return view;
        }

        private void MButtonErrorClose_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }

        private void MButtonNext_Click(object sender, EventArgs e)
        {
            // Siguiente Posicion
            mPosition++;
            mNext.Invoke(this, new OnNextEventArgs(mPosition));
        }

        private void MButtonGo_Click(object sender, EventArgs e)
        {
            // Invoco al evento de dar click en el boton "Go"
            mGo.Invoke(this, new OnGoEventArgs(mPosition));
            // Cierro el Dialog
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

    public class OnGoEventArgs : EventArgs
    {
        private int mPosition;

        public int Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }
        
        // Constructor
        public OnGoEventArgs(int p) : base()
        {
            mPosition = p;
        }
    }

    public class OnNextEventArgs : EventArgs
    {
        private int mPosition;

        public int Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }
        
        // Constructor
        public OnNextEventArgs(int p) : base()
        {
            mPosition = p;
        }
    }
}