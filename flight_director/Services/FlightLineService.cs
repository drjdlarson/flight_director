using flight_director.Services;
using flight_director.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(FlightLineService))]

namespace flight_director.Services
{
    public class FlightLineService
    {
        static SQLiteAsyncConnection db;
        async static Task Init()
        {
            if (db != null)
            {
                return;
            }

            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyData.db");
            db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<FlightLine>();
        }

        public static async Task AddNewLine(int id, double startlat, double startlon, double startalt, double endlat, double endlon, double endalt, double avgele)
        {
            await Init();
            var flightline = new FlightLine
            {
                ID = id,
                StartLat = startlat,
                StartLon = startlon,
                StartAlt = startalt,
                EndLat = endlat,
                EndLon = endlon,
                EndAlt = endalt,
                AvgEle = avgele
            };
            await db.InsertAsync(flightline);
            
        }

        public static async Task<IEnumerable<FlightLine>> GetLine()
        {
            await Init();
            var flightline = await db.Table<FlightLine>().ToListAsync();
            return flightline;
        }
        public static async Task<FlightLine> GetLine(int id)
        {
            await Init();
            int count = await db.Table<FlightLine>().CountAsync();
            if (id > count)
            {
                return null;
            }
            FlightLine flightline = await db.GetAsync<FlightLine>(id);
            return flightline;
        }

        public static async Task RemoveLine(int id)
        {
            await Init();
            await db.DeleteAsync<FlightLine>(id);
        }
    }
}
