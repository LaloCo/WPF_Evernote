using Microsoft.WindowsAzure.MobileServices;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NotesApp.ViewModel
{
    public class DatabaseHelper
    {
        // public static string dbFile = Path.Combine(Environment.CurrentDirectory, "notesDb.db3");
        public static MobileServiceClient client = new MobileServiceClient("https://wpfevernote.azurewebsites.net");

        public static async Task<bool> Insert<T>(T item)
        {
            bool result = false;

            //using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            //{
            //    conn.CreateTable<T>();
            //    int numberOfRows = conn.Insert(item);
            //    if (numberOfRows > 0)
            //        result = true;
            //}

            try
            {
                await client.GetTable<T>().InsertAsync(item);
                result = true;
            }
            catch(Exception ex)
            {
                result = false;
            }

            return result;
        }

        public static async Task<bool> Update<T>(T item)
        {
            bool result = false;

            //using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            //{
            //    conn.CreateTable<T>();
            //    int numberOfRows = conn.Update(item);
            //    if (numberOfRows > 0)
            //        result = true;
            //}

            try
            {
                await client.GetTable<T>().UpdateAsync(item);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public static async Task<bool> Delete<T>(T item)
        {
            bool result = false;

            //using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            //{
            //    conn.CreateTable<T>();
            //    int numberOfRows = conn.Delete(item);
            //    if (numberOfRows > 0)
            //        result = true;
            //}

            try
            {
                await client.GetTable<T>().DeleteAsync(item);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public static async Task<List<T>> Read<T>()
        {
            try
            {
                var items = await client.GetTable<T>().ToListAsync();
                return items;
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }
    }
}
