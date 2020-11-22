using KakaoLion.dto.model;
using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

/**
 * 쓰레드 자동 종료 기능 구현 해야함
 */

namespace KakaoLion
{
    public partial class App : Application
    {
        private static TcpClient client = new TcpClient();
        public static NetworkStream stream;

        public static string userId;
        private static bool isRunning = true;
        private static bool isLoginWindowClosed = false;
        private static bool isMainWindowClosed = false;

        public List<MenuModel> menuList = new List<MenuModel>();
        public List<OrderModel> orderList = new List<OrderModel>();

        public App()
        { 
            
            client.Connect(Constants.SERVER_CONNSTR, Constants.PORT);
            stream = client.GetStream();

            Thread thread = new Thread(new ThreadStart(messageThread));
            thread.Start();
            getAllMenu();
            
        }

        public static void LoginWindow_CloseAction(bool isClosed)
        {
            isLoginWindowClosed = isClosed;
        }

        public static void MainWindow_ClosedAction(bool isClosed)
        {
            isMainWindowClosed = isClosed;
        }

        public void messageThread()
        {
            byte[] buffer = new byte[1024];
            string msg;

            while (isRunning)
            {
               if (client.Client.Receive(buffer, SocketFlags.Peek) != 0)
               {
                    try
                    {
                        stream.Read(buffer, 0, buffer.Length);
                        msg = Encoding.UTF8.GetString(buffer);

                        if (msg.Contains("총매출액")) getTodayAllOrder();
                        MessageBox.Show(msg);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                }
                else
                {
                    MessageBox.Show("서버와 연결이 유실되었습니다.");
                    break;
                }
            }
            client.Close();
            stream.Close();
        }

        public void getAllMenu()
        {
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

        public void getTodayAllOrder()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
            {
                conn.Open();
                string today = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                string sql = "SELECT * FROM lion.order WHERE purchaseAt LIKE '" + today + "%'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Boolean paymentPlace = rdr["paymentPlace"].ToString().Equals("0") ? true : false;
                    Boolean paymentMethod = rdr["paymentMethod"].ToString().Equals("0") ? true : false;

                    orderList.Add(new OrderModel
                    {
                        idx = (int)rdr["idx"],
                        orderCount = (int)rdr["orderCount"],
                        menuIdx = (int)rdr["menuIdx"],
                        quantity = (int)rdr["quantity"],
                        totalPrice = (int)rdr["totalPrice"],
                        userId = (string)rdr["userId"],
                        purchaseAt = (string)rdr["purchaseAt"],
                        paymentPlace = paymentPlace,
                        paymentMethod = paymentMethod,
                        shopIdx = (int)rdr["shopIdx"]
                    });
                }
            }
            getTotalPrice();
        }

        public void getTotalPrice()
        {
            int totalPrice = 0;

            foreach (MenuModel menu in menuList)
            {
                List<OrderModel> menuOrderList = orderList.Where(order => menu.idx == order.menuIdx).ToList();
                foreach (OrderModel order in menuOrderList)
                {
                    totalPrice += menu.price * order.quantity;
                }
            }
            getNetProfit(totalPrice);
        }

        public void getNetProfit(int totalPrice)
        {
            int totalNetProfitPrice = 0;

            foreach (OrderModel order in orderList)
            {
                totalNetProfitPrice += order.totalPrice;
            }
            sendMessage(totalPrice, totalNetProfitPrice);
        }

        public void sendMessage(int totalPrice, int totalNetProfitPrice)
        {
            JObject json = new JObject();

            string content = "총 금액 : " + totalPrice + "순수 이익 : " + totalNetProfitPrice;

            json.Add("MSGType", 1);
            json.Add("Id", userId);
            json.Add("Content", content);
            json.Add("ShopName", "");
            json.Add("OrderNumber", "");
            json.Add("Group", true);
            json.Add("Menus", "");

            byte[] buffer = new byte[4096];
            string message = JsonConvert.SerializeObject(json);
            buffer = Encoding.UTF8.GetBytes(message);

            App.stream.Write(buffer, 0, buffer.Length);
        }
    }
}
