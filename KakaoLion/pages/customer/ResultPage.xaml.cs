using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Text;
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
            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
            {
                conn.Open();
                String sql = "SELECT * FROM lion.order WHERE idx = (SELECT MAX(idx) FROM lion.order); ";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                try
                {
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();

                    lastOrderCount = (int)rdr["orderCount"];

                    if (lastOrderCount == 100)
                    {
                        lastOrderCount = 0;
                    }
                } 
                catch (Exception e)
                {
                    lastOrderCount = 0;
                }
            }
        }

        public void setOrder()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
            {
                string purchaseAt = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

                conn.Open();
                foreach (OrderModel order in OrderPage.orderList)
                {
                    string values = "(" + (lastOrderCount + 1) + ", " + order.menuIdx + ", " + order.quantity + ", " + order.totalPrice + ", '" +
                        order.userId + "', '" + purchaseAt + "', " + order.paymentPlace + ", " + order.paymentMethod + ", " + (int)(order.shopIdx == null ? 0 : order.shopIdx) + ")";

                    string sql = "INSERT INTO lion.order(orderCount, menuIdx, quantity, totalPrice, userId, purchaseAt, paymentPlace, paymentMethod, shopIdx) VALUES" + values;

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            if (OrderPage.orderList[0].shopIdx != null)
            {
                startTimer();
            }

            showData();
            sendMessage();
        }

        public void showData()
        {
            int totalCount = 0;
            int totalPrice = 0;
            string userName = "";
            string userPurchaseMethodId = "";

            foreach (OrderModel order in OrderPage.orderList)
            {
                totalCount += order.quantity;
                totalPrice += order.totalPrice;

                if (MainWindow.userList.Where(user => user.id == order.userId).ToList().Count != 0)
                {
                    UserModel user = MainWindow.userList.Where(u => u.id == order.userId).ToList()[0];
                    userName = user.name;
                
                    if (order.paymentMethod == false) userPurchaseMethodId = user.qrcode;
                    else userPurchaseMethodId = user.barcode;
                }
            }
            userInfo.Text = "이름 : " + userName + " | 번호 : " + userPurchaseMethodId;
            orderInfo.Text = totalCount + "개 " + totalPrice + "원";
            orderCount.Text = (lastOrderCount + 1).ToString();
        }

        public void startTimer()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
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
            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
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

        private void sendMessage()
        {
            JArray jarray = new JArray();

            for (int i = 0; i < OrderPage.orderList.Count; i++)
            {
                JObject jObject = new JObject();
                string menuName = MainWindow.menuList.Where(menu => menu.idx == OrderPage.orderList[i].menuIdx).ToList()[0].name;

                jObject.Add("Name", menuName);
                jObject.Add("Count", OrderPage.orderList[i].quantity);
                jObject.Add("Price", OrderPage.orderList[i].totalPrice);

                jarray.Add(jObject);
            }

            string orderCount;
            if (lastOrderCount + 1 < 10) orderCount = "00" + (lastOrderCount + 1);
            else if (lastOrderCount + 1 < 100) orderCount = "0" + (lastOrderCount + 1);
            else orderCount = "" + (lastOrderCount + 1);

            JObject json = new JObject();
            json.Add("MSGType", 2);
            json.Add("Id", OrderPage.orderList[0].userId);
            json.Add("Content", "");
            json.Add("ShopName", "카카오프렌즈");
            json.Add("OrderNumber", orderCount);
            json.Add("Group", false);
            json.Add("Menus", jarray);

            byte[] buffer = new byte[4096];
            string message = JsonConvert.SerializeObject(json);
            buffer = Encoding.UTF8.GetBytes(message);

            App.stream.Write(buffer, 0, buffer.Length);
        }
    }
}
