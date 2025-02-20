using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaWord.Services
{
    public static class PathDb
    {
        //var downloadPath = $"Filename={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SigmaWordDb.db")}";
        public static string GetPath(string nameDb)
        {
            string pathDbSqlite = "";
            if(DeviceInfo.Platform == DevicePlatform.Android)
            {
                pathDbSqlite = Path.Combine(FileSystem.AppDataDirectory, nameDb);
                pathDbSqlite = $"Filename = {pathDbSqlite}";
            }
            else if(DeviceInfo.Platform == DevicePlatform.iOS)
            {
                pathDbSqlite = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                pathDbSqlite = Path.Combine(pathDbSqlite, "...", "Library", nameDb);
            }
            return pathDbSqlite;
        }
    }
}
