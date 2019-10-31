using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainingScheduler
{
    public partial class LoginForm : Form
    {
        private LoginDatabase db;
        private User user;
        public LoginForm(User user)
        {
            InitializeComponent();
            this.user = user;
            db = new LoginDatabase();
        }

        private async void loginButton_ClickAsync(object sender, EventArgs e)
        {
            if (passBox.Text.Length == 0 ||
                loginBox.Text.Length == 0)
            {
                MessageBox.Show("Введите логин и пароль");
            }
            user.login = loginBox.Text;
            loginButton.Enabled = false;
            singupBtn.Enabled = false;
            loginBox.Enabled = false;
            passBox.Enabled = false;
            Cursor = Cursors.WaitCursor;
            bool success = await db.Login(user, passBox.Text);
            Cursor = Cursors.Default;
            loginButton.Enabled = true;
            singupBtn.Enabled = true;
            loginBox.Enabled = true;
            passBox.Enabled = true;
            if (success)
            {
                Debug.WriteLine($"Login successfull login: {user.login}, pass: {passBox.Text}");
                user.login = "";
                DialogResult = DialogResult.OK;       
                Close();
                return;
            }
            passBox.Text = "";
            MessageBox.Show("Неверный логин или пароль");
        }

        private void singupBtn_Click(object sender, EventArgs e)
        {
            SignupForm signup = new SignupForm(db);
            this.Visible = false;
            signup.ShowDialog();
            this.Visible = true;
        }

        private async void LoginForm_LoadAsync(object sender, EventArgs e)
        {
            loginButton.Enabled = false;
            singupBtn.Enabled = false;
            loginBox.Enabled = false;
            passBox.Enabled = false;
            Cursor = Cursors.WaitCursor;
            await db.Create();
            Cursor = Cursors.Default;
            loginButton.Enabled = true;
            singupBtn.Enabled = true;
            loginBox.Enabled = true;
            passBox.Enabled = true;
        }
    }
}
