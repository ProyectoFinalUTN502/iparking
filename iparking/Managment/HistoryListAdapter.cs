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
    class HistoryListAdapter : BaseAdapter<Historic>
    {
        private List<Historic> mItems;
        private Context mContext;

        public HistoryListAdapter(Context context, List<Historic> items)
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
            return mItems[position].id;
        }

        public override Historic this[int position]
        {
            get { return mItems[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.HistoryListRow, null, false);
            }

            TextView txtDate = row.FindViewById<TextView>(Resource.Id.textViewDate);
            TextView txtParkinglot = row.FindViewById<TextView>(Resource.Id.textViewParkinglot);

            txtDate.Text = mItems[position].date.ToString("dd.MM.yy HH:mm");
            txtParkinglot.Text = mItems[position].parkinglot;

            return row;
        }
    }
}