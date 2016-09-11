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
    public class FileManager
    {
        private ISharedPreferences pref;
        private ISharedPreferencesEditor edit;

        public FileManager()
        {
            pref = Application.Context.GetSharedPreferences(Entities.ConfigManager.SharedFile, FileCreationMode.Private);
            edit = pref.Edit();
        }

        public string GetValue(string key)
        {
            return pref.GetString(key, String.Empty);
        }

        public void SetValue(string key, string value)
        {
            edit.PutString(key, value);
            edit.Apply();
        }

        public void Clear()
        {
            edit.Clear();
            edit.Apply();
        }
    }
}