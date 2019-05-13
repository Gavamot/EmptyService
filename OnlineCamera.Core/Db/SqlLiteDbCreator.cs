using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCamera.Core.Db
{
    class SqlLiteDbCreator
    {
        readonly string fileName;
        readonly string localFolder;
        public SqlLiteDbCreator(string fileName = "online_camera.db")
        {
            localFolder = System.Reflection.Assembly.GetEntryAssembly().Location;
            this.fileName = Path.Combine(localFolder, fileName);
        }

        public const string T_STAT = "T_STAT";
        public void Init()
        {
            if (!File.Exists(fileName)) 
            {
                SQLiteConnection.CreateFile(fileName);
            }

            using (var connect = new SQLiteConnection(@"Data Source=C:\TestDB.db; Version=3")) 
            {
                // строка запроса, который надо будет выполнить
                string commandText =
                    $@"CREATE TABLE IF NOT EXISTS {T_STAT} ( 
                        [id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
                        [timeStampt] VARCHAR(40), 
                        [ip] VARCHAR(25),
                        [brigade_code] INTEGER,
                        [camera_array] VARCHAR(64))"; 
                var cmd = new SQLiteCommand(commandText, connect);
                connect.Open(); 
                cmd.ExecuteNonQuery(); 
                connect.Close(); 
            }
        }
    }
}
