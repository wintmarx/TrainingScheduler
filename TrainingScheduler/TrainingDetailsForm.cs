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
    public partial class TrainingDetailsForm : Form
    {
        User user;
        Training training;
        public TrainingDetailsForm(User user, Training training)
        {
            this.user = user;
            if (!user.isCoach)
            {
                addTrainingBtn.Visible = false;
            }
            InitializeComponent();
        }
    }
}
