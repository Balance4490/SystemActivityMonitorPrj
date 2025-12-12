using System;
using System.Linq;
using System.Windows;
using SystemActivityMonitor.Data;
using SystemActivityMonitor.Data.Entities;

namespace SystemActivityMonitor.UI
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtRegUsername.Text;
            string password = txtRegPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля!");
                return;
            }

            using (var db = new MonitorDbContext())
            {
                if (db.Users.Any(u => u.Username == username))
                {
                    MessageBox.Show("Такий користувач вже існує!");
                    return;
                }

                var newUser = new User
                {
                    Username = username,
                    PasswordHash = MonitorDbContext.HashPassword(password),
                    Role = "User", 
                    CreatedAt = DateTime.UtcNow
                };

                db.Users.Add(newUser);
                db.SaveChanges();
            }

            MessageBox.Show("Акаунт успішно створено!");
            
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}