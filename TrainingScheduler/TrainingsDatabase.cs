using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TrainingScheduler
{
    class TrainingsDatabase
    {
        private const string dbName = "schedule.sqlite";
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
                        "CREATE TABLE IF NOT EXISTS Trainings " +
                        "(id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                        "name TEXT NOT NULL, " +
                        "coach_id INTEGER NOT NULL, " +
                        "date INTEGER UNIQUE NOT NULL);" +
                        "CREATE TABLE IF NOT EXISTS Trainees " +
                        "(training_id INTEGER NOT NULL," +
                        "user_id INTEGER NOT NULL," +
                        "PRIMARY KEY(training_id, user_id));";
            command.CommandType = CommandType.Text;
            await db.ExecuteNonQueryAsyncAtomic(command);
            Debug.WriteLine("DB Created");
        }

        private void AddSyncTrainingQuery(Training training, SQLiteCommand command)
        {
            int n = command.Parameters.Count;
            command.CommandText += $"INSERT OR IGNORE INTO Trainings (name, coach_id, date) VALUES (@name{n}, @coach_id{n}, @date{n});";
            command.Parameters.Add($"@name{n}", DbType.String).Value = training.name;
            command.Parameters.Add($"@coach_id{n}", DbType.Int32).Value = training.coachId;
            command.Parameters.Add($"@date{n}", DbType.Int64).Value = DateTimeToUnixTimestamp(training.date);
        }

        private void AddDeleteTrainingQuery(Training training, SQLiteCommand command)
        {
            if (training.id < 0)
            {
                return;
            }
            int n = command.Parameters.Count;
            command.CommandText += $"DELETE FROM Trainings WHERE id = @id{n};";
            command.CommandText += $"DELETE FROM Trainees WHERE training_id = @id{n};";
            command.Parameters.Add($"@id{n}", DbType.Int32).Value = training.id;
        }

        private void AddUpdateTrainingQuery(User user, Training training, SQLiteCommand command)
        {
            if (training.id < 0)
            {
                return;
            }
            int n = command.Parameters.Count;
            if (0 == training.traineesId.Find(x => x == user.id))
            {
                command.CommandText += $"DELETE FROM Trainees WHERE training_id = @t_id{n} AND user_id = @u_id{n};";
            }
            else
            {
                command.CommandText += $"INSERT OR IGNORE INTO Trainees VALUES (@t_id{n}, @u_id{n});";
            }
            command.Parameters.Add($"@t_id{n}", DbType.Int32).Value = training.id;
            command.Parameters.Add($"@u_id{n}", DbType.Int32).Value = user.id;
        }

        public async Task<bool> SyncTrainings(User user, List<Training> trainings, Semaphore mutex)
        {
            command.Parameters.Clear();
            command.CommandText = "";
            mutex.WaitOne();
            for (int i = 0; i < trainings.Count; i++)
            {
                switch (trainings[i].syncState)
                {
                    case SyncState.Unsynced:
                        AddSyncTrainingQuery(trainings[i], command);
                        break;
                    case SyncState.Deleted:
                        AddDeleteTrainingQuery(trainings[i], command);
                        break;
                    case SyncState.Updated:
                        AddUpdateTrainingQuery(user, trainings[i], command);
                        break;
                }
            }
            command.CommandText += "SELECT id, name, coach_id, date " +
                                  "FROM Trainings " +
                                  "ORDER BY id;";
            command.CommandType = CommandType.Text;
            await db.FetchDb();
            List<object[]> table = db.ExecuteReader(command);
            command.CommandText = "SELECT * FROM Trainees ORDER BY training_id;";
            List<object[]> trainees = db.ExecuteReader(command);
            await db.CommitDb();

            if (table.Count <= 0)
            {
                Debug.WriteLine("No trainings to sync");
                return false;
            }

            trainings.Clear();
            for (int i = 0; i < table.Count; i++)
            {
                Training t = new Training();
                t.id = (long)table[i][0];
                t.name = (string)table[i][1];
                t.coachId = (long)table[i][2];
                t.date = UnixTimestampToDateTime((long)table[i][3]);
                t.syncState = SyncState.Synced;
                int ti = trainees.FindIndex(x => (long)x[0] == t.id);
                if (ti >= 0)
                {
                    for (int j = ti; j < trainees.Count; j++)
                    {
                        if ((long)trainees[j][0] != t.id)
                        {
                            break;
                        }
                        t.traineesId.Add((long)trainees[j][1]);
                    }
                }

                Debug.WriteLine("User fetched {0}, {1}, {2}, {3}", t.id, t.name, t.coachId, t.date.ToString());
                trainings.Add(t);
            }

            mutex.Release();
            return true;
        }

        public DateTime UnixTimestampToDateTime(long unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTime);
        }

        public long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        }
    }
}
