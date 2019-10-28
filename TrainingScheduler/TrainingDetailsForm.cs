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
        public TrainingDetailsForm(User user, Training training, TrainingDetailsMode mode)
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
                    break;
                case TrainingDetailsMode.New:
                    nameEdit.Enabled = true;
                    okBtn.Visible = false;
                    break;
                case TrainingDetailsMode.View:
                    addTrainingBtn.Visible = false;
                    nameEdit.Text = training.name;
                    okBtn.Visible = false;
                    break;
            }
            coachEdit.Text = training.coach.firstName + " " + training.coach.secondName;
        }

        private void addTrainingBtn_Click(object sender, EventArgs e)
        {
            if (nameEdit.Text.Length == 0)
            {
                MessageBox.Show("Введите название тренировки");
                return;
            }
            training.name = nameEdit.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            training.isDeleted = true;
            DialogResult = DialogResult.Ignore;
            Close();
        }
    }
}
