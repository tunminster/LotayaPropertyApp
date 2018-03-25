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
using LotayaPropertyApp.Database.Models;
using SQLite;

namespace LotayaPropertyApp.Database
{
    public class AppDbContext
    {
        string dbPath = string.Empty;
        SQLiteConnection db;

        public AppDbContext()
        {
            dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "lotayaApp.db3");
            db = new SQLiteConnection(dbPath);
        }

        public int Insert<T>(T model)
        {
            return db.Insert(model);
        }

        public int DeleteModel<T>()
        {
            return db.DeleteAll<T>();
        }


        private string CreateDatabase(string path)
        {
            try
            {
                var connection = new SQLiteAsyncConnection(path);
                {
                    connection.CreateTableAsync<TokenModel>();
                    return "Database created";
                }
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
    }
}