using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSuperrTasker.MVVM.Model;

namespace WpfSuperrTasker.Core
{
    public static class DataBase
    {
        private static SqliteConnection sql;

        static DataBase()
        {
            sql = new SqliteConnection("Data source=Data.db");
            sql.Open();
            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = sql;
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS 'Task'
            (
                'Id'            INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE ,
                'Name'          TEXT NOT NULL,
                'Description'   TEXT NOT NULL
            )";
            cmd.ExecuteNonQuery();
        }

        public static ulong Add(UserTask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException();
            }
            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = sql;
            cmd.CommandText = @"INSERT INTO Task
            (Name, Description)
             Values
            (@Name, @Description); SELECT last_insert_rowid()";
            cmd.Parameters.AddWithValue("@Name", task.Name);
            cmd.Parameters.AddWithValue("@Description", task.Name);
            cmd.Prepare();

            ulong result = Convert.ToUInt64(cmd.ExecuteScalar());
            return result;
        }


        public static List<UserTask> GetTasks()
        {
            List<UserTask> tasks = new List<UserTask>(100);

            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = sql;
            cmd.CommandText = "SELECT * FROM Task";
            var result = cmd.ExecuteReader();

            while (result.Read())
            {
                tasks.Add(new UserTask()
                {
                    Description = (string)result["Description"],
                    Name = (string)result["Name"]
                });
            }
            return tasks;
        }
    }
}
