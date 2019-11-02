using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace TrainingScheduler
{
    public partial class CalendarForm : Form
    {
        private User user;
        private DateTime date;
        private DateTime oldestDate;
        const int daysInWeek = 7;
        const int hoursInDay = 13;
        const int startHour = 9;
        private List<User> users;
        private List<Training> localTrainings;
        private Thread usersUpdateThread;
        private bool shouldRunUsersUpdateThread;
        private Thread trainingsUpdateThread;
        private bool shouldRunTrainingsUpdateThread;
        TrainingsDatabase trainingsDatabase;
        LoginDatabase loginDatabase;
        Semaphore trainingsMutex;
        public CalendarForm(User user)
        {
            InitializeComponent();
            trainingsMutex = new Semaphore(1, 1);
            users = new List<User>();
            localTrainings = new List<Training>();
            this.user = user;
            calendar.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            date = DateTime.Today.AddDays(-DayOfWeekToNumber(DateTime.Today.DayOfWeek));
            oldestDate = date;
            UpdateCurMonthLabel();
            GenCalendar();
            trainingsDatabase = new TrainingsDatabase();
            loginDatabase = new LoginDatabase();
        }

        private async void UsersUpdateThreadFunc()
        {
            while (shouldRunUsersUpdateThread)
            {
                await loginDatabase.FetchAllUsers(users);
                Thread.Sleep(30000);
            }
        }

        private async void TrainingsUpdateThreadFunc()
        {
            while (shouldRunTrainingsUpdateThread)
            {           
                await trainingsDatabase.SyncTrainings(user, localTrainings, trainingsMutex);
                Action action = () => UpdateCalendar();
                if(!IsDisposed)
                {
                    Invoke(action);
                }
                Thread.Sleep(15000);
            }
        }

        public async Task Init()
        {
            await loginDatabase.FetchAllUsers(users);
            await trainingsDatabase.Create();
            await trainingsDatabase.SyncTrainings(user, localTrainings, trainingsMutex);
            usersUpdateThread = new Thread(UsersUpdateThreadFunc);
            shouldRunUsersUpdateThread = true;
            usersUpdateThread.Start();
            trainingsUpdateThread = new Thread(TrainingsUpdateThreadFunc);
            shouldRunTrainingsUpdateThread = true;
            trainingsUpdateThread.Start();
        }

        private void UpdateCalendar()
        {
            for (int i = daysInWeek + hoursInDay; i < calendar.Controls.Count; i++)
            {
                Button btn = (Button)calendar.Controls[i];
                TableLayoutPanelCellPosition cell = calendar.GetPositionFromControl(btn);
                Training training = localTrainings.Find(t => 
                    DateTime.Compare(t.date, date.AddDays(cell.Column - 1).AddHours(cell.Row - 1 + startHour)) == 0
                    && t.syncState != SyncState.Deleted);

                if (training != null)
                {
                    AddTrainingToCell(btn, training);
                }
                else
                {
                    ClearCell(btn);
                }
            }
        }

        private DayOfWeek NumberToDayOfWeek(int number)
        {
            return (DayOfWeek)(++number >= daysInWeek ? (number - daysInWeek) : number);
        }

        private int DayOfWeekToNumber(DayOfWeek dayOfWeek)
        {
            int number = (int)dayOfWeek;
            return --number < 0 ? (daysInWeek + number) : number;
        }

        private void GenCalendar()
        {
            calendar.RowCount = 1 + hoursInDay;
            calendar.RowStyles.Clear();
            calendar.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));

            for (int i = 0; i < daysInWeek; i++)
            {
                Label l = new Label();
                l.Dock = DockStyle.Fill;
                l.Margin = new Padding(0);
                l.TextAlign = ContentAlignment.MiddleCenter;
                l.Text = NumberToDayOfWeek(i).ToString();
                calendar.Controls.Add(l, i + 1, 0);
            }

            for (int i = 0; i < hoursInDay; i++)
            {
                Label l = new Label();
                l.Dock = DockStyle.Fill;
                l.Margin = new Padding(0);
                l.TextAlign = ContentAlignment.MiddleCenter;
                l.Text = $"{startHour + i}:00";
                calendar.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
                calendar.Controls.Add(l, 0, i + 1);
            }

            for (int hour = 0; hour < hoursInDay; hour++)
            {
                for (int day = 0; day < daysInWeek; day++)
                {
                    Button btn = new Button();
                    btn.Dock = DockStyle.Fill;
                    btn.Margin = new Padding(0);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    //btn.Font = new Font(btn.Font.FontFamily, 9, FontStyle.Bold);
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Click += Cell_Click;
                    calendar.Controls.Add(btn, day + 1, hour + 1);
                }
            }
        }

        private void Cell_Click(object sender, EventArgs e)
        {          
            TableLayoutPanelCellPosition cell = calendar.GetPositionFromControl((Control)sender);
            if (cell.Column == 0 && cell.Row == 0)
            {
                return;
            }
            Training training;
            TrainingDetailsMode mode = TrainingDetailsMode.View;
            Button btn = (Button)sender;
            if (btn.Text == "")
            {
                if (!user.isCoach)
                {
                    return;
                }
                training = new Training();
                mode = TrainingDetailsMode.New;
                training.coachId = user.id;
                training.date = date.AddDays(cell.Column - 1).AddHours(cell.Row - 1 + startHour);
            }
            else
            {
                training = localTrainings.Find(t =>
                    DateTime.Compare(t.date, date.AddDays(cell.Column - 1).AddHours(cell.Row - 1 + startHour)) == 0
                    && t.syncState != SyncState.Deleted);
                if (training == null)
                {
                    return;
                }
                if (user.id == training.coachId)
                {
                    mode = TrainingDetailsMode.Edit;
                }
            }
            Debug.WriteLine("{0}:{1}, mode {2}, coach {3}", cell.Row, cell.Column, mode, training.coachId);
            TrainingDetailsForm details = new TrainingDetailsForm(user, users, training, mode);
            DialogResult result = details.ShowDialog();
            if (result == DialogResult.Abort)
            {
                return;
            }
            Cursor = Cursors.WaitCursor;
            trainingsMutex.WaitOne();
            if (result == DialogResult.OK)
            {
                training.name = details.nameEdit.Text;
                localTrainings.Add(training);
                AddTrainingToCell(btn, training);
            }
            else if (result == DialogResult.Ignore)
            {
                training.syncState = SyncState.Deleted;
                if (training.id == -1)
                {
                    localTrainings.Remove(training);
                }
                ClearCell(btn);
            }
            else if (result == DialogResult.Yes)
            {
                training.traineesId.Remove(user.id);
                training.syncState = SyncState.Updated;
            }
            else if (result == DialogResult.No)
            {
                training.traineesId.Add(user.id);
                training.syncState = SyncState.Updated;
            }
            trainingsMutex.Release();
            Cursor = Cursors.Default;
        }

        private void nextWeekBtn_Click(object sender, EventArgs e)
        {
            /*if (DateTime.Compare(oldestDate.AddDays(7), date) <= 0)
            {
                return;
            }*/
            date = date.AddDays(7);
            UpdateCurMonthLabel();
            UpdateCalendar();
        }

        private void prevWeekBtn_Click(object sender, EventArgs e)
        {
            if (DateTime.Compare(oldestDate.AddDays(7), date) > 0)
            {
                return;
            }
            date = date.AddDays(-7);
            UpdateCurMonthLabel();
            UpdateCalendar();
        }

        private void UpdateCurMonthLabel()
        {
            curMonthLabel.Text = date.ToString("dd.MM.yyyy") + date.AddDays(6).ToString(" -\ndd.MM.yyyy");
        }

        private void AddTrainingToCell(Button cell, Training training)
        {
            User coach = users.Find(u => u.id == training.coachId);
            if (coach == null)
            {
                return;
            }
            cell.BackColor = Color.LightGreen;
            cell.Text = training.name + "\n" + coach.firstName + " " + coach.secondName;
        }

        private void ClearCell(Button cell)
        {
            cell.BackColor = SystemColors.Window;
            cell.Text = "";
        }

        private async void CalendarForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            shouldRunUsersUpdateThread = false;
            shouldRunTrainingsUpdateThread = false;
            usersUpdateThread.Join();
            trainingsUpdateThread.Join();
            await trainingsDatabase.SyncTrainings(user, localTrainings, trainingsMutex);
        }

        private async void CalendarForm_Shown(object sender, EventArgs e)
        {
            Hide();
            LoadingForm loading = new LoadingForm();
            loading.Show();
            await Init();
            UpdateCalendar();
            loading.Close();
            Show();
        }
    }
}
