using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TrainingScheduler
{
    public enum SignupResult
    {
        Ok,
        DataIncomplete,
        AlreadyExists,
        CoachCodeWrong,
        DbError
    }

    public class LoginDatabase
    {
        private const string dbName = "login.sqlite";
        private const string coachCode = "iamcoach";
        private DatabaseManager db;
        private SQLiteCommand command;
        public LoginDatabase()
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

        public async Task<SignupResult> Signup(User user)
        {
            if (user.firstName.Length == 0 ||
                user.secondName.Length == 0 ||
                user.login.Length == 0 ||
                user.pswd.Length == 0)
            {
                return SignupResult.DataIncomplete;
            }

            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT EXISTS (SELECT 1 FROM Users WHERE login = @user);";
            command.Parameters.Clear();
            command.Parameters.Add("@user", DbType.String).Value = user.login;
            command.Parameters.Add("@pswd", DbType.String).Value = user.pswd;
            command.Parameters.Add("@name", DbType.String).Value = user.firstName;
            command.Parameters.Add("@surname", DbType.String).Value = user.secondName;

            if ((long)(await db.ExecuteScalarAsync(command)) == 1)
            {
                return SignupResult.AlreadyExists;
            }

            if (user.coachCode.Length != 0 && user.coachCode != coachCode)
            {
                return SignupResult.CoachCodeWrong;
            }

            command.CommandText = "INSERT INTO Users (name, surname, login, pswd) VALUES (@name, @surname, @user, @pswd);";
            await db.ExecuteNonQueryAsync(command);

            if (user.coachCode.Length != 0)
            {
                command.CommandText = "INSERT INTO Coaches (usr_id) VALUES ((SELECT id FROM Users WHERE login = @user));";
                await db.ExecuteNonQueryAsync(command);
            }

            return SignupResult.Ok;
        }

        public async Task<bool> Login(User user)
        {
            command.CommandText = "SELECT id, name, surname FROM Users WHERE login = @user AND pswd = @pswd;";
            command.CommandType = CommandType.Text;
            command.Parameters.Clear();
            command.Parameters.Add("@user", DbType.String).Value = user.login;
            command.Parameters.Add("@pswd", DbType.String).Value = user.pswd;
            List<object[]> table = await db.ExecuteReaderAsync(command);
            if (table.Count == 0)
            {
                return false;
            }
            long id = (long)table[0][0];
            user.firstName = (string)table[0][1];
            user.secondName = (string)table[0][2];

            command.CommandText = "SELECT EXISTS (SELECT 1 FROM Coaches WHERE usr_id = @usr_id);";
            command.Parameters.Add("@usr_id", DbType.Int32).Value = id;
            if ((long)(await db.ExecuteScalarAsync(command)) == 1)
            {
                user.isCoach = true;
            }

            Debug.WriteLine($"name {user.firstName}, surname {user.secondName}, login {user.login}, pass {user.pswd}, isCoach {user.isCoach}");

            return true;
        }
    }
}
