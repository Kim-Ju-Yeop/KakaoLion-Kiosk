using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KakaoLion.pages
{
    public partial class AdminPage : INotifyPropertyChanged
    {
        private const string CATEGORY1 = "메뉴별 판매 수 및 총액";
        private const string CATEGORY2 = "카테고리 별 판매 수 및 총액";
        private const string CATEGORY3 = "지점별 메뉴별 판매수 및 총액";
        private const string CATEOGRY4 = "지점별 카테고리별 판매수 및 총액";
        private const string CATEGORY5 = "일별 총 매출액";
        private const string CATEGORY6 = "하루 중 시간대별 총 매출액";
        private const string CATEGORY7 = "회원별 총 매출액 및 판매수 총액";

        private DispatcherTimer dispatcherTimer;
        private List<string> categoryList = new List<string>();
        private DateTime dateTime = new DateTime(0001, 01, 01, 00, 00, 00);

        private string _time;
        public string Time 
        {   get { return _time; }
            set
            { 
              _time = value;
              NotifyPropertyChanged(nameof(Time));
            }
        }

        public AdminPage()
        {
            InitializeComponent();

            Time = "가동시간 : " + (MainWindow.operationDateTime - dateTime).ToString();
            this.DataContext = this;

            setTimer();
            setList();
        }

        private void setTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler(timer_Tick);
            dispatcherTimer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Time = "가동시간 : " + (MainWindow.operationDateTime - dateTime).ToString();
        }

        public void setList()
        {
            categoryList.Add(CATEGORY1);
            categoryList.Add(CATEGORY2);
            categoryList.Add(CATEGORY3);
            categoryList.Add(CATEOGRY4);
            categoryList.Add(CATEGORY5);
            categoryList.Add(CATEGORY6);
            categoryList.Add(CATEGORY7);

            categoryListBox.ItemsSource = categoryList.ToList();
        }

        private void categoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            welcomePanel.Visibility = Visibility.Collapsed;

            string category = (string)categoryListBox.SelectedItem;
            switch (category)
            {
                case CATEGORY1:
                    pageFrame.Source = new Uri("StatsPage1.xaml", UriKind.Relative);
                    break;
                case CATEGORY2:
                    pageFrame.Source = new Uri("StatsPage2.xaml", UriKind.Relative);
                    break;
                case CATEGORY3:
                    pageFrame.Source = new Uri("StatsPage3.xaml", UriKind.Relative);
                    break;
                case CATEOGRY4:
                    pageFrame.Source = new Uri("StatsPage4.xaml", UriKind.Relative);
                    break;
                case CATEGORY5:
                    pageFrame.Source = new Uri("StatsPage5.xaml", UriKind.Relative);
                    break;
                case CATEGORY6:
                    pageFrame.Source = new Uri("StatsPage6.xaml", UriKind.Relative);
                    break;
                case CATEGORY7:
                    pageFrame.Source = new Uri("StatsPage7.xaml", UriKind.Relative);
                    break;
            }
        }

        private void UserPage_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Source = new Uri("UserPage.xaml", UriKind.Relative);
            categoryListBox.UnselectAll();
        }

        private void MenuPage_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Source = new Uri("MenuPage.xaml", UriKind.Relative);
            categoryListBox.UnselectAll();
        }

        private void ChatPage_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Source = new Uri("ChatPage.xaml", UriKind.Relative);
            categoryListBox.UnselectAll();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
