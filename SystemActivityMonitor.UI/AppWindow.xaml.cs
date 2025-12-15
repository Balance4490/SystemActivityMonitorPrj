using System;
using System.Windows;
using System.Windows.Media;

namespace SystemActivityMonitor.UI
{
    public partial class AppWindow : Window
    {
        public Guid ProcessId { get; private set; }
        
        public event Action<Guid> AppClosed; 

        public AppWindow(Guid id, string appName)
        {
            InitializeComponent();
            ProcessId = id;
            this.Title = appName;
            txtAppName.Text = appName;

            if (appName.Contains("Chrome"))
            {
                TopBar.Background = new SolidColorBrush(Color.FromRgb(219, 68, 55)); 
                txtContent.Text = "Google Chrome\nNew Tab";
            }
            else if (appName.Contains("Visual Studio"))
            {
                TopBar.Background = new SolidColorBrush(Color.FromRgb(86, 44, 124)); 
                txtContent.Text = "Visual Studio 2025\nSolution Explorer...";
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            AppClosed?.Invoke(ProcessId);
        }
    }
}