using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KakaoLion.pages
{
    public partial class ResultPage : Page
    {
        int tik = 60;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        int lastOrderCount;
        int shopIdx;

        public ResultPage()
        {
            InitializeComponent();

            getLastOrderCount();
            setOrder();
        }

        public void getLastOrderCount()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.CONNSTR))
            {
                conn.Open();
                String sql = "SELECT MAX(orderCount) FROM order";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                try
                {
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();
                    lastOrderCount = (int)rdr["orderCount"];
                } 
                catch (Exception e)
                {
                    lastOrderCount = 0;
                }
            }
        }

        public void setOrder()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.CONNSTR))
            {
                string userId = "student";
                string purchaseAt = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

                conn.Open();
                foreach (OrderModel order in OrderPage.orderList)
                {
                    string values = "(" + (lastOrderCount + 1) + ", " + order.menuIdx + ", " + order.quantity + ", " + order.totalPrice + ", '" +
                        userId + "', '" + purchaseAt + "', " + order.paymentPlace + ", " + order.paymentMethod + ", " + (int)(order.shopIdx == null ? 0 : order.shopIdx) + ")";

                    string sql = "INSERT INTO lion.order(orderCount, menuIdx, quantity, totalPrice, userId, purchaseAt, paymentPlace, paymentMethod, shopIdx) VALUES" + values;

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            if (OrderPage.orderList[0].shopIdx != null) startTimer();
            showData();
        }

        public void showData()
        {
            int totalCount = 0;
            int totalPrice = 0;

            foreach (OrderModel order in OrderPage.orderList)
            {
                totalCount += order.quantity;
                totalPrice += order.totalPrice;
            }

            orderInfo.Text = totalCount + "개 " + totalPrice + "원";
            orderCount.Text = (lastOrderCount + 1).ToString();
        }

        public void startTimer()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.CONNSTR))
            {
                shopIdx = (int)OrderPage.orderList[0].shopIdx;
                string lastOrder = String.Format("{0:HHmmss}", DateTime.Now);

                conn.Open();
                string sql = "UPDATE shop SET lastOrder=" + "'" + lastOrder + "', possible=" + false + " WHERE idx=" + shopIdx;

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler(timer_Tick);
            dispatcherTimer.Start();
        }

        public void stopTimer()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.CONNSTR))
            {
                conn.Open();
                string sql = "UPDATE shop SET possible=" + true + " WHERE idx=" + shopIdx;

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            dispatcherTimer.Stop();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            if (tik > 0) tik--;
            else stopTimer();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            OrderPage.orderList.Clear();
            this.NavigationService.Navigate(new HomePage());
        }
    }
}
