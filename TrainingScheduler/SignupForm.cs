using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainingScheduler
{
    public partial class SignupForm : Form
    {
        private LoginDatabase db;
        public SignupForm(LoginDatabase db)
        {
            InitializeComponent();
            this.db = db;
        }

        private async void signupBtn_ClickAsync(object sender, EventArgs e)
        {
            User user = new User();
            user.firstName = nameBox.Text;
            user.secondName = surnameBox.Text;
            user.login = loginBox.Text;
            user.pswd = pswdBox.Text;
            user.coachCode = coachCodeBox.Text;

            foreach (Control item in Controls)
            {
                item.Enabled = false;
            }

            Cursor = Cursors.WaitCursor;
            SignupResult result = await db.Signup(user);
            Cursor = Cursors.Default;

            foreach (Control item in Controls)
            {
                item.Enabled = true;
            }

            switch (result)
            {
                case SignupResult.AlreadyExists:
                    MessageBox.Show("Пользователь с таким логином уже существует");
                    break;
                case SignupResult.CoachCodeWrong:
                    MessageBox.Show("Неверный код тренера");
                    break;
                case SignupResult.DataIncomplete:
                    MessageBox.Show("Заполните все необходимые поля");
                    break;
                case SignupResult.DbError:
                    MessageBox.Show("Ошибка базы данных");
                    break;
                case SignupResult.Ok:
                    Close();
                    break;
            }
        }
    }
}
