using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using SystemActivityMonitor.Data.Patterns.Observer;
using SystemActivityMonitor.Data.Patterns.Command;
using SystemActivityMonitor.Data.Processes;

namespace SystemActivityMonitor.UI
{
    public partial class DesktopWindow : Window, IObserver
    {
        private SystemController _controller;
        private CommandInvoker _invoker;
        private DispatcherTimer _timer;
        private string _currentUsername;
        private string _currentUserRole;

        private Dictionary<Guid, AppWindow> _openWindows = new Dictionary<Guid, AppWindow>();
        private MainWindow _monitorWindow = null;

        public DesktopWindow(string username, string role)
        {
            InitializeComponent();
            this.Title = $"Desktop | User: {username}";

            _currentUsername = username;
            _currentUserRole = role;

            _controller = new SystemController();
            _invoker = new CommandInvoker();

            _controller.Attach(this);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => _controller.UpdateSystemState();
            _timer.Start();
        }

        private void BtnLaunchChrome_Click(object sender, RoutedEventArgs e)
        {
            LaunchApp("Google Chrome", 5.0f, 200.0f);
        }

        private void BtnLaunchVS_Click(object sender, RoutedEventArgs e)
        {
            LaunchApp("Visual Studio", 30.0f, 1500.0f);
        }

        private void LaunchApp(string name, float cpu, float ram)
        {
            var cmd = new StartProcessCommand(_controller, name, cpu, ram);
            _invoker.SetCommand(cmd);
            _invoker.Run();

            var process = _controller.GetProcesses().LastOrDefault(p => p.Name == name);

            if (process != null && !_openWindows.ContainsKey(process.Id))
            {
                var appWindow = new AppWindow(process.Id, name);

                appWindow.AppClosed += (id) =>
                {
                    var killCmd = new KillProcessCommand(_controller, id);
                    _invoker.SetCommand(killCmd);
                    _invoker.Run();
                };

                appWindow.Show();
                _openWindows.Add(process.Id, appWindow);
            }
        }

        private void BtnLaunchMonitor_Click(object sender, RoutedEventArgs e)
        {
            if (_monitorWindow == null || !_monitorWindow.IsLoaded)
            {
                _monitorWindow = new MainWindow(_controller, _currentUsername, _currentUserRole);
                _monitorWindow.Show();
            }
            else
            {
                _monitorWindow.Activate();
            }
        }

        public void Update(float totalCpu, float totalRam, List<VirtualProcess> processes)
        {
            var deadProcessIds = _openWindows.Keys.Where(id => !processes.Any(p => p.Id == id)).ToList();

            foreach (var id in deadProcessIds)
            {
                _openWindows[id].Close();
                _openWindows.Remove(id);
            }
        }
    }
}