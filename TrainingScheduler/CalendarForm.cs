using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        const int daysInWeek = 7;
        const int hoursInDay = 13;
        const int startHour = 9;
        private Dictionary<DateTime, Training> localTrainings;
        public CalendarForm(User user)
        {
            InitializeComponent();
            localTrainings = new Dictionary<DateTime, Training>();
            this.user = user;
            calendar.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            date = DateTime.Now;
            date = date.AddDays(-DayOfWeekToNumber(date.DayOfWeek));
            date = date.AddHours(-date.Hour);
            UpdateCurMonthLabel();
            GenCalendar();
        }

        private void UpdateCalendar()
        {
            /*DateTime calDate = new DateTime(date.Year, date.Month, 1);
            calDate = calDate.AddDays(-DayOfWeekToNumber(calDate.DayOfWeek));
            for (int i = daysInWeek; i < calendar.Controls.Count; i++)
            {
                calendar.Controls[i].Text = calDate.Day.ToString();
                
                if (calDate.Month != date.Month)
                {
                    calendar.Controls[i].BackColor = Color.LightGray;
                }
                else
                {
                    calendar.Controls[i].BackColor = SystemColors.Window;
                }
                calDate = calDate.AddDays(1);
            }*/
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
                    btn.Font = new Font(btn.Font.FontFamily, 9, FontStyle.Bold);
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
            if (((Button)sender).Text == "")
            {
                if (!user.isCoach)
                {
                    return;
                }
                training = new Training();
                training.date = date.AddDays(cell.Column - 1);
            }
            else
            {
                localTrainings.TryGetValue(date.AddDays(cell.Column - 1), out training);
            }
            Debug.WriteLine("{0}:{1}", cell.Row, cell.Column);
            TrainingDetailsForm details = new TrainingDetailsForm(user, training);
            if (details.ShowDialog() == DialogResult.OK)
            {
                localTrainings.Add(training.date, training);
            }
        }

        private void nextWeekBtn_Click(object sender, EventArgs e)
        {
            date = date.AddDays(7);
            UpdateCurMonthLabel();
            UpdateCalendar();
        }

        private void prevWeekBtn_Click(object sender, EventArgs e)
        {
            date = date.AddDays(-7);
            UpdateCurMonthLabel();
            UpdateCalendar();
        }

        private void UpdateCurMonthLabel()
        {
            curMonthLabel.Text = date.ToString("dd.MM.yyyy") + date.AddDays(6).ToString(" -\ndd.MM.yyyy");
        }
    }
}
