using System.Windows;

namespace SystemActivityMonitor.UI
{
    public partial class UserWindow : Window
    {
        public UserWindow(string username)
        {
            InitializeComponent();
            txtUserWelcome.Text = $"Вітаємо, {username}!";
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}