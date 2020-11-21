using KakaoLion.dto.model;
using KakaoLion.model;
using KakaoLion.pages;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
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

        public MainWindow()
        {
            InitializeComponent();
            getOperationTime();
            getAllMenu();
            getAllStore();
            getAllUser();
        }

        private void setTimer()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();

            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler(timer_Tick);
            dispatcherTimer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            operationDateTime = operationDateTime.AddSeconds(1);
            timerLabel.Content = String.Format("{0:tt HH시 mm분 ss초 dddd}", DateTime.Now);
        }

        public void getAllMenu()
        {
            menuList.Clear();
            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
            {
                conn.Open();
                string sql = "SELECT * FROM menu";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string imagePath = "";
                    bool stock = int.Parse(rdr["stock"].ToString()) == 1;

                    switch ((Category)rdr["category"])
                    {
                        case Category.Small:
                            imagePath = "/resources/image/small/" + rdr["name"] + ".jpg";
                            break;
                        case Category.Medium:
                            imagePath = "/resources/image/medium/" + rdr["name"] + ".jpg";
                            break;
                        case Category.Big:
                            imagePath = "/resources/image/big/" + rdr["name"] + ".jpg";
                            break;
                    }

                    menuList.Add(new MenuModel
                    {
                        idx = (int)rdr["idx"],
                        page = (int)rdr["page"],
                        category = (Category)rdr["category"],
                        name = (string)rdr["name"],
                        price = (int)rdr["price"],
                        discount = (int)rdr["discount"],
                        stock = stock,
                        imagePath = imagePath
                    });
                }
            }
        }
        public void getAllStore()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
            {
                conn.Open();
                string sql = "SELECT * FROM lion.shop";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    bool possible = int.Parse(rdr["possible"].ToString()) == 1;
                    storeList.Add(new StoreModel()
                    {
                        idx = (int)rdr["idx"],
                        name = (string)rdr["name"],
                        lastOrder = (string)rdr["lastOrder"],
                        possible = possible
                    });
                }
            }
        }

        public void getOperationTime()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
            {
                conn.Open();
                string sql = "SELECT * FROM program";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string operationTime = (string)rdr["operationTime"];

                    int year = int.Parse(operationTime.Split(' ')[0]);
                    int month = int.Parse(operationTime.Split(' ')[1]);
                    int day = int.Parse(operationTime.Split(' ')[2]);

                    int hour = int.Parse(operationTime.Split(' ')[3]);
                    int minute = int.Parse(operationTime.Split(' ')[4]);
                    int second = int.Parse(operationTime.Split(' ')[5]);

                    operationDateTime = new DateTime(year, month, day, hour, minute, second);
                }
            }
            setTimer();
        }

        public void getAllUser()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
            {
                conn.Open();
                string sql = "SELECT * FROM lion.user";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    userList.Add(new UserModel
                    {
                        id = (string)rdr["id"],
                        pw = (string)rdr["pw"],
                        name = (string)rdr["name"],
                        address = (string)rdr["address"],
                        barcode = (string)rdr["barcode"],
                        qrcode = (string)rdr["qrcode"]
                    });
                }
            }
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

            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
            {
                conn.Open();
                string sql = "UPDATE program SET operationTime = '" + operationTime + "'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            App.MainWindow_ClosedAction(true);
        }
    }
}
