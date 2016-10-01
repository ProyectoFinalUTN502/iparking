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
    class VehicleListAdapter : BaseAdapter<Vehicle>
    {
        private List<Vehicle> mItems;
        private Context mContext;

        public VehicleListAdapter(Context context, List<Vehicle> items)
        {
            mItems = items;
            mContext = context;

        }

        public override int Count
        {
            get { return mItems.Count; }
        }

        public override long GetItemId(int position)
        {
            return mItems[position].vehicleTypeID;
        }

        public override Vehicle this[int position]
        {
            get { return mItems[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.VehicleListRow, null, false);
            }

            TextView txtName = row.FindViewById<TextView>(Resource.Id.textViewName);
            txtName.Text = mItems[position].name;

            return row;
        }
    }
}