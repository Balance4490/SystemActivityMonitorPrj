using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SystemActivityMonitor.UI
{
    public partial class AppWindow : Window
    {
        public Guid ProcessId { get; private set; }
        public event Action<Guid> AppClosed;
        public event Action<Guid> TabAdded;
        public event Action<Guid> TabClosed;
        private bool _isChrome = false;
        private int _tabCount = 0;

        public AppWindow(Guid id, string appName)
        {
            InitializeComponent();
            ProcessId = id;
            this.Title = appName;
            txtAppName.Text = appName;

            string imagesPath = AppDomain.CurrentDomain.BaseDirectory + "Images\\";

            if (appName.Contains("Chrome"))
            {
                _isChrome = true;
                TopBar.Background = new SolidColorBrush(Color.FromRgb(53, 54, 58));
                txtAppName.Visibility = Visibility.Collapsed;
                BtnAddTab.Visibility = Visibility.Visible;

                try
                {
                    AppScreenshot.Source = new BitmapImage(new Uri(imagesPath + "google_home.png", UriKind.Absolute));
                    AppScreenshot.Visibility = Visibility.Visible;
                    StatsPanel.Visibility = Visibility.Collapsed;
                }
                catch
                {
                    txtContent.Text = "Google Chrome (Image not found)";
                }

                AddNewVisualTab();
            }
            else if (appName.Contains("Visual Studio"))
            {
                TopBar.Background = new SolidColorBrush(Color.FromRgb(86, 44, 124));

                try
                {
                    AppScreenshot.Source = new BitmapImage(new Uri(imagesPath + "vs_screenshot.png", UriKind.Absolute));
                    AppScreenshot.Visibility = Visibility.Visible;
                    StatsPanel.Visibility = Visibility.Collapsed;
                }
                catch
                {
                    txtContent.Text = "Visual Studio 2025 (Image not found)";
                }
            }
        }

        private void BtnAddTab_Click(object sender, RoutedEventArgs e)
        {
            AddNewVisualTab();
            TabAdded?.Invoke(ProcessId);
        }

        private void AddNewVisualTab()
        {
            _tabCount++;
            string tabTitle = $"New Tab {_tabCount}";

            Border tabBorder = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(5, 5, 0, 0),
                Margin = new Thickness(2, 0, 0, 0),
                Padding = new Thickness(5),
                Height = 30,
                MinWidth = 100
            };

            StackPanel contentPanel = new StackPanel { Orientation = Orientation.Horizontal };

            TextBlock titleBlock = new TextBlock
            {
                Text = tabTitle,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 5, 0),
                Foreground = Brushes.Black,
                MaxWidth = 80,
                TextTrimming = TextTrimming.CharacterEllipsis
            };

            Button closeButton = new Button
            {
                Content = "x",
                Width = 20,
                Height = 20,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Foreground = Brushes.Gray,
                FontWeight = FontWeights.Bold,
                Cursor = Cursors.Hand,
                Tag = tabBorder
            };

            closeButton.Click += CloseSingleTab_Click;

            contentPanel.Children.Add(titleBlock);
            contentPanel.Children.Add(closeButton);
            tabBorder.Child = contentPanel;

            TabsContainer.Children.Add(tabBorder);
            UpdateStats();
        }

        private void CloseSingleTab_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Border tabToRemove = (Border)btn.Tag;

            TabsContainer.Children.Remove(tabToRemove);
            _tabCount--;

            TabClosed?.Invoke(ProcessId);
            UpdateStats();

            if (TabsContainer.Children.Count == 0)
            {
                this.Close();
            }
        }

        private void UpdateStats()
        {
            if (_isChrome)
            {
                float estimatedRam = 150 + (_tabCount - 1) * 100;
                txtStats.Text = $"Active Tabs: {_tabCount} | Estimated RAM usage: ~{estimatedRam} MB";
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            AppClosed?.Invoke(ProcessId);
        }
    }
}
