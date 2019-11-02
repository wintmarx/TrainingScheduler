using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainingScheduler
{
    public enum TrainingDetailsMode
    {
        View,
        Edit,
        New
    }
    public partial class TrainingDetailsForm : Form
    {
        User user;
        Training training;
        TrainingDetailsMode mode;
        bool subscribed = false;
        public TrainingDetailsForm(User user, List<User> users, Training training, TrainingDetailsMode mode)
        {
            InitializeComponent();
            this.user = user;
            this.training = training;
            this.mode = mode;
            Text = training.date.ToString();
            switch(mode)
            {
                case TrainingDetailsMode.Edit:
                    //nameEdit.Enabled = true;
                    nameEdit.Text = training.name;
                    addTrainingBtn.Visible = false;
                    subBtn.Visible = false;
                    break;
                case TrainingDetailsMode.New:
                    nameEdit.Enabled = true;
                    okBtn.Visible = false;
                    subBtn.Visible = false;
                    break;
                case TrainingDetailsMode.View:
                    addTrainingBtn.Visible = false;
                    nameEdit.Text = training.name;
                    okBtn.Visible = false;
                    subscribed = training.traineesId.FindIndex(x => x == user.id) >= 0;
                    if (subscribed)
                    {
                        subBtn.Text = "Отписаться";
                    }
                    break;
            }
            User coach = users.Find(x => x.id == training.coachId);
            for (int i = 0; i < training.traineesId.Count; i++)
            {
                User usr = users.Find(x => x.id == training.traineesId[i]);
                traineesList.Items.Add(usr.firstName + " " + usr.secondName);
            }
            coachEdit.Text = coach.firstName + " " + coach.secondName;
        }

        private void addTrainingBtn_Click(object sender, EventArgs e)
        {
            if (nameEdit.Text.Length == 0)
            {
                MessageBox.Show("Введите название тренировки");
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Ignore;
            Close();
        }

        private void subBtn_Click(object sender, EventArgs e)
        {
            if (subscribed)
            {
                DialogResult = DialogResult.Yes;
            }
            else
            {
                DialogResult = DialogResult.No;
            }
            Close();
        }
    }
}
