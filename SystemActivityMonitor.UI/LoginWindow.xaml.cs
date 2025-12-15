using System.Linq;
using System;
using System.Windows;
using SystemActivityMonitor.Data;
using SystemActivityMonitor.Data.Entities;

namespace SystemActivityMonitor.UI
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            using (var db = new MonitorDbContext())
            {
                db.Database.EnsureCreated();
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblStatus.Text = "Введіть логін та пароль!";
                return;
            }

            string inputHash = MonitorDbContext.HashPassword(password);

            using (var db = new MonitorDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == inputHash);

                if (user != null)
                {
                    var newSession = new Session
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        CreatedAt = DateTime.UtcNow
                    };

                    db.Sessions.Add(newSession);
                    db.SaveChanges();

                    MainWindow main = new MainWindow(user.Username, user.Role);
                    main.Show();

                    this.Close();
                }
                else
                {
                    lblStatus.Text = "Невірний логін або пароль";
                }
            }
        }

        private void BtnGoToRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow reg = new RegisterWindow();
            reg.Show();
            this.Close();
        }
    }
}