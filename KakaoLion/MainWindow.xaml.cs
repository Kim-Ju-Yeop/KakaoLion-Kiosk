using KakaoLion.database.repository;
using KakaoLion.database.repositoryImpl;
using KakaoLion.model;
using KakaoLion.pages;
using KakaoLion.widget.extension;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace KakaoLion
{
    public partial class MainWindow : Window
    {
        public static DateTime operationDateTime;

        public static List<MenuModel> menuList = new List<MenuModel>();
        public static List<StoreModel> storeList = new List<StoreModel>();
        public static List<UserModel> userList = new List<UserModel>();

        private DispatcherTimer dispatcherTimer;

        private MenuRepository menuRepository;
        private StoreRepository storeRepository;
        private ProgramRepository programRepository;
        private UserRepository userRepository;

        public MainWindow()
        {
            InitializeComponent();

            menuRepository = new MenuRepositoryImpl();
            storeRepository = new StoreRepositoryImpl();
            programRepository = new ProgramRepositoryImpl();
            userRepository = new UserRepositoryImpl();

            getAllMenu();
            getAllStore();
            getOperationTime();
            getAllUser();
        }

        private void getAllMenu()
        {
            menuList.Clear();
            menuList = menuRepository.getAllMenu();
        }

        private void getAllStore()
        {
            storeList.Clear();
            storeList = storeRepository.getAllStore();
        }

        private void getOperationTime()
        {
            operationDateTime = new DateTime();
            operationDateTime = programRepository.getOperationTime();
            setTimer();
        }

        private void getAllUser()
        {
            userList.Clear();
            userList = userRepository.getAllUser();
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
            operationDateTime = operationDateTime.AddSeconds(1);
            timerLabel.Content = DateTImeExtension.dateTimeFormat(DateTime.Now);
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            if (pageFrame.Source == null)
            {
                if(OrderPage.orderList.Count == 0)
                {
                    pageFrame.Source = new Uri("pages/customer/HomePage.xaml", UriKind.Relative);
                }
                else if (MessageBox.Show("정말로 홈화면으로 돌아가시겠습니까?\n(주문 목록이 삭제됩니다.)", "이전으로", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    OrderPage.orderList.Clear();
                    pageFrame.Source = new Uri("pages/customer/HomePage.xaml", UriKind.Relative);
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            string operationTime = operationDateTime.Year + " " + operationDateTime.Month + " " + operationDateTime.Day + " " + 
                operationDateTime.Hour + " " + operationDateTime.Minute + " " + operationDateTime.Second;

            programRepository.updateOperationTime(operationTime);
            App.MainWindow_ClosedAction(true);
        }
    }
}
