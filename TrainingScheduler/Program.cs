using System;
using System.Windows.Forms;

namespace TrainingScheduler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            User user = new User();
            user.isCoach = true;
            user.firstName = "Test";
            user.secondName = "Second";
            user.id = 0;
            /*LoginForm loginForm = new LoginForm(user);
            
            if (loginForm.ShowDialog() == DialogResult.OK)
            {*/
                Application.Run(new CalendarForm(user));
            //}
            
        }
    }
}
