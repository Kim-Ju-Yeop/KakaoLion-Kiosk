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

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
