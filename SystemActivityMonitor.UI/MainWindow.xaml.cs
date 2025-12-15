using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using SystemActivityMonitor.Data.Patterns.Command;
using SystemActivityMonitor.Data.Patterns.Observer;
using SystemActivityMonitor.Data.Processes;
using SystemActivityMonitor.Data.Patterns.Visitor;

namespace SystemActivityMonitor.UI
{
    public partial class MainWindow : Window, IObserver
    {
        private SystemController _controller = new SystemController();
        private CommandInvoker _invoker = new CommandInvoker();
        private DispatcherTimer _timer;

        public MainWindow(string username, string role)
        {
            InitializeComponent();
            this.Title = $"System Activity Monitor | Logged in as: {username} ({role})";

            if (role == "Admin")
            {
                AdminPanel.Visibility = Visibility.Visible;
                UserPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                UserPanel.Visibility = Visibility.Visible;
                AdminPanel.Visibility = Visibility.Collapsed;
            }

            _controller.Attach(this);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => _controller.UpdateSystemState();
            _timer.Start();
        }

        public void Update(float totalCpu, float totalRam, List<VirtualProcess> processes)
        {
            var viewList = processes.Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                StatusName = p.GetStatus(),
                CurrentCpu = $"{p.GetCurrentCpuUsage():F1}%",
                CurrentRam = $"{p.GetCurrentRamUsage()} MB"
            }).ToList();

            lstProcesses.ItemsSource = viewList;

            lblTotalCpu.Text = $"{totalCpu:F1}%";
            lblTotalRam.Text = $"{totalRam} MB";

            if (totalCpu > 80)
                lblStatus.Text = "CRITICAL LOAD!";
            else
                lblStatus.Text = "Normal operation";
        }

        private void BtnStartChrome_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new StartProcessCommand(_controller, "Google Chrome", 5, 200);
            _invoker.SetCommand(cmd);
            _invoker.Run();
        }

        private void BtnStartVS_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new StartProcessCommand(_controller, "Visual Studio", 30, 1500);
            _invoker.SetCommand(cmd);
            _invoker.Run();
        }

        private void BtnKillProcess_Click(object sender, RoutedEventArgs e)
        {
            dynamic selected = lstProcesses.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Виберіть процес зі списку!");
                return;
            }

            var cmd = new KillProcessCommand(_controller, selected.Id);
            _invoker.SetCommand(cmd);

            try
            {
                _invoker.Run();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Системна помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не вдалося завершити процес: " + ex.Message);
            }
        }

        private void BtnFreezeProcess_Click(object sender, RoutedEventArgs e)
        {
            dynamic selected = lstProcesses.SelectedItem;
            if (selected == null) return;

            _controller.SetProcessState(selected.Id, new NotRespondingState());
        }

        private void BtnShowHistory_Click(object sender, RoutedEventArgs e)
        {
            UserWindow history = new UserWindow("System Archive");
            history.Show();
        }

        private void BtnClearDb_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Ви впевнені, що хочете очистити всю базу даних?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _controller.ClearAllData();
                MessageBox.Show("Базу даних очищено!");
            }
        }

        private void BtnKillRow_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            if (button.Tag is Guid processId)
            {
                var cmd = new KillProcessCommand(_controller, processId);
                _invoker.SetCommand(cmd);

                try
                {
                    _invoker.Run();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Системна помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не вдалося завершити процес: " + ex.Message);
                }
            }
        }

        private void BtnRunAnalysis_Click(object sender, RoutedEventArgs e)
        {
            var visitor = new ResourceAnalysisVisitor();
            _controller.ApplyVisitor(visitor);
            MessageBox.Show(visitor.GetReport(), "Глибокий аналіз системи");
        }
    }
}