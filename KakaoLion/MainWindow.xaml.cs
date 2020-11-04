using KakaoLion.pages;
using System;
using System.Windows;
using System.Windows.Threading;

namespace KakaoLion
{
    public partial class MainWindow : Window
    {
        public static DateTime startOperationTime { get; set; } = DateTime.Now;

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
            if (pageFrame.Source == null)
            {
                if(OrderPage.orderList.Count == 0)
                {
                    pageFrame.Source = new Uri("pages/HomePage.xaml", UriKind.Relative);
                }
                else if (MessageBox.Show("정말로 홈화면으로 돌아가시겠습니까?\n(주문 목록이 삭제됩니다.)", "이전으로", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    OrderPage.orderList.Clear();
                    pageFrame.Source = new Uri("pages/HomePage.xaml", UriKind.Relative);
                }
            }
        }
    }
}
