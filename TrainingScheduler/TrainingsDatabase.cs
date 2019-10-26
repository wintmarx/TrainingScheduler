using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TrainingScheduler
{
    class TrainingsDatabase
    {
        private const string dbName = "login.sqlite";
        private const string coachCode = "iamcoach";
        private DatabaseManager db;
        private SQLiteCommand command;
        public TrainingsDatabase()
        {
            db = new DatabaseManager(dbName);
            command = new SQLiteCommand();
        }

        public async Task Create()
        {
            command.Parameters.Clear();
            command.CommandText = "PRAGMA foreign_keys = ON;" +
                        "CREATE TABLE IF NOT EXISTS Users " +
                        "(id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                        "name TEXT NOT NULL, " +
                        "surname TEXT NOT NULL, " +
                        "login TEXT NOT NULL, " +
                        "pswd TEXT NOT NULL);" +
                        "CREATE TABLE IF NOT EXISTS Coaches " +
                        "(id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                        "usr_id INTEGER NOT NULL," +
                        "FOREIGN KEY(usr_id) REFERENCES Users(id) ON DELETE CASCADE);";
            command.CommandType = CommandType.Text;
            await db.ExecuteNonQueryAsync(command);
            Debug.WriteLine("DB Created");
        }
    }
}
