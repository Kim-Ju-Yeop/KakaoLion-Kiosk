using KakaoLion.database.repository;
using KakaoLion.database.util;
using KakaoLion.model;
using KakaoLion.pages;
using KakaoLion.widget.extension;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace KakaoLion.database.repositoryImpl
{
    class OrderRepositoryImpl : OrderRepository
    {
        private Database db = new Database();

        private MySqlConnection conn;
        private MySqlCommand cmd;
        private MySqlDataReader rdr;

        public int getLastOrderCount()
        {
            conn = db.getConnection();
            int lastOrderCount;
            using (conn)
            {
                String sql = "SELECT * FROM lion.order WHERE idx = (SELECT MAX(idx) FROM lion.order); ";

                cmd = new MySqlCommand(sql, conn);

                try
                {
                    rdr = cmd.ExecuteReader();
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
            return lastOrderCount;
        }

        public void insertOrder(int lastOrderCount)
        {
            conn = db.getConnection();
            using (conn)
            {
                string purchaseAt = DateTImeExtension.dateTimeFormat3(DateTime.Now);

                foreach (OrderModel order in OrderPage.orderList)
                {
                    string values = "(" + (lastOrderCount + 1) + ", " + order.menuIdx + ", " + order.quantity + ", " + order.totalPrice + ", '" +
                        order.userId + "', '" + purchaseAt + "', " + order.paymentPlace + ", " + order.paymentMethod + ", " + (int)(order.shopIdx == null ? 0 : order.shopIdx) + ")";

                    string sql = "INSERT INTO lion.order(orderCount, menuIdx, quantity, totalPrice, userId, purchaseAt, paymentPlace, paymentMethod, shopIdx) VALUES" + values;

                    cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<OrderModel> getAllOrder()
        {
            conn = db.getConnection();
            List<OrderModel> orderList = new List<OrderModel>();
            using (conn)
            {
                string sql = "SELECT * FROM lion.order";

                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

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
            return orderList;
        }

        public List<string> getAllPurchaseAt()
        {
            conn = db.getConnection();
            List<string> purchaseAtList = new List<string>();
            using (conn)
            {
                string sql = "SELECT purchaseAt FROM lion.order";

                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string date = (string)rdr["purchaseAt"];
                    string formatDate = date.Substring(0, 10);

                    purchaseAtList.Add(formatDate);
                }
            }
            return purchaseAtList;
        }

        public List<OrderModel> getTodayAllOrder(string date)
        {
            conn = db.getConnection();
            List<OrderModel> todayOrderList = new List<OrderModel>();
            using (conn)
            {
                string sql = "SELECT * FROM lion.order WHERE purchaseAt LIKE '" + date + "%'";

                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Boolean paymentPlace = rdr["paymentPlace"].ToString().Equals("0") ? true : false;
                    Boolean paymentMethod = rdr["paymentMethod"].ToString().Equals("0") ? true : false;

                    todayOrderList.Add(new OrderModel
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
            return todayOrderList;
        }

        public List<OrderModel> getAllTimeOrder(string hour)
        {
            conn = db.getConnection();
            List<OrderModel> allTimeOrderList = new List<OrderModel>();
            using (conn)
            {
                string time = DateTImeExtension.dateTimeFormat4(DateTime.Now);
                string sql = "SELECT * FROM lion.order WHERE purchaseAt LIKE '" + time + " " + hour + "%'";

                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Boolean paymentPlace = rdr["paymentPlace"].ToString().Equals("0") ? true : false;
                    Boolean paymentMethod = rdr["paymentMethod"].ToString().Equals("0") ? true : false;

                    allTimeOrderList.Add(new OrderModel
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
            return allTimeOrderList;
        }

        public List<OrderModel> getUserOrder(string userId)
        {
            conn = db.getConnection();
            List<OrderModel> userOrderList = new List<OrderModel>();
            using (conn)
            {
                string sql = "SELECT * FROM lion.order WHERE userId = '" + userId + "';";

                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Boolean paymentPlace = rdr["paymentPlace"].ToString().Equals("0") ? true : false;
                    Boolean paymentMethod = rdr["paymentMethod"].ToString().Equals("0") ? true : false;

                    userOrderList.Add(new OrderModel
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
            return userOrderList;
        }
    }
}
