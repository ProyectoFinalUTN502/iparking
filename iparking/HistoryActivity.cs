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
using iparking.Managment;
using iparking.Entities;
using Newtonsoft.Json;

namespace iparking
{
    [Activity(Label = "HistoryActivity", Theme = "@style/MyTheme.Base")]
    public class HistoryActivity : Activity
    {
        System.Net.WebClient mClient;
        FileManager mFile;
        List<Historic> mHistoric;
        ListView mListView;
        HistoryListAdapter mListAdapter;

        ImageView mBack;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
          
            SetContentView(Resource.Layout.History);

            mFile = new FileManager();

            mListView = FindViewById<ListView>(Resource.Id.listViewHistory);

            mBack = FindViewById<ImageView>(Resource.Id.imageViewBack);
            mBack.Click += MBack_Click;

            GetHistoric();
        }

        private void MBack_Click(object sender, EventArgs e)
        {
            Managment.ActivityManager.TakeMeTo(this, typeof(MoreOptionsActivity), false);
        }

        public void GetHistoric()
        {
            string clientID = mFile.GetValue("id");

            mClient = new System.Net.WebClient();
            Uri url = new Uri(ConfigManager.WebService + "/searchHistoric.php?cl_id=" + clientID);

            mClient.DownloadDataAsync(url);
            mClient.DownloadDataCompleted += MClient_DownloadDataCompleted;

        }

        private void MClient_DownloadDataCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                try
                {
                    string json = Encoding.UTF8.GetString(e.Result);
                    mHistoric = JsonConvert.DeserializeObject<List<Historic>>(json);

                    // Cargo la Listview con los Vehiculos
                    mListAdapter = new HistoryListAdapter(this, mHistoric);
                    mListView.Adapter = mListAdapter;
                    mListView.ItemClick += MListView_ItemClick;
                }
                catch (Exception ex)
                {
                    //Managment.ActivityManager.TakeMeTo(this, typeof(ErrorActivity), true);
                }

            });
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            return;
        }
    }
}