using System;
using System.Windows;
using System.Windows.Threading;

namespace KakaoLion
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            setTimer();
        }

        private void setTimer()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();

            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
            dispatcherTimer.Tick += new EventHandler(timer_Tick);
            dispatcherTimer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timerLabel.Content = String.Format("{0:tt HH시 mm분 ss초 dddd}", DateTime.Now);
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Source = new Uri("pages/HomePage.xaml", UriKind.Relative);
        }
    }
}
