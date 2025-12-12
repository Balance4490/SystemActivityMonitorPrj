using System;
using System.Linq;
using System.Windows;
using SystemActivityMonitor.Data;
using SystemActivityMonitor.Data.Entities;
using SystemActivityMonitor.Data.Patterns.Iterator;

namespace SystemActivityMonitor.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow(string username, string role)
        {
            InitializeComponent();
            txtWelcome.Text = $"Вітаємо, {username}!";
            txtRole.Text = $"Рівень доступу: {role}";
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new MonitorDbContext())
            {
                var admin = db.Users.FirstOrDefault(u => u.Username == "admin");
                if (admin == null) return;

                var session = new Session
                {
                    UserId = admin.Id,
                    MachineName = "TEST-PC",
                    OSVersion = "Windows 11"
                };
                db.Sessions.Add(session);
                db.SaveChanges();

                var rnd = new Random();
                for (int i = 0; i < 5; i++)
                {
                    db.ResourceLogs.Add(new ResourceLog
                    {
                        SessionId = session.Id,
                        CpuLoad = rnd.Next(10, 90),
                        RamUsage = rnd.Next(2000, 8000),
                        CreatedAt = DateTime.UtcNow.AddSeconds(i * 10)
                    });
                }
                db.SaveChanges();
            }
            MessageBox.Show("Тестові дані успішно згенеровано!");
        }

        private void BtnLoadIterator_Click(object sender, RoutedEventArgs e)
        {
            lstLogs.Items.Clear();

            LogCollection collection = new LogCollection();

            using (var db = new MonitorDbContext())
            {
                var logsFromDb = db.ResourceLogs.OrderByDescending(l => l.CreatedAt).Take(20).ToList();
                foreach (var log in logsFromDb)
                {
                    collection.Add(log);
                }
            }

            IIterator iterator = collection.CreateIterator();

            iterator.First();
            while (!iterator.IsDone())
            {
                ResourceLog item = iterator.CurrentItem();

                string displayText = $"[{item.CreatedAt.ToShortTimeString()}] CPU: {item.CpuLoad}% | RAM: {item.RamUsage} MB";
                lstLogs.Items.Add(displayText);

                iterator.Next();
            }
        }
    }
}
