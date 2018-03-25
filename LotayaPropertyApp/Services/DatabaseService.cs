using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace LotayaPropertyApp.Services
{
    public class DatabaseService
    {
        string dbPath = string.Empty;
        SQLiteConnection db;
        public DatabaseService()
        {
            dbPath = Path.Combine(Android.OS.Environment.DataDirectory.AbsolutePath, "lotayadb.db3");
        }

        public void CreateConnection()
        {
            db = new SQLiteConnection(dbPath);
        }


    }
}