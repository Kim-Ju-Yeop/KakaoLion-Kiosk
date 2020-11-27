using KakaoLion.dto.model;
using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace KakaoLion.pages.admin
{
    public partial class StatsPage2 : Page
    {
        public List<OrderModel> orderList = new List<OrderModel>();
        public List<StatsModel> statsList = new List<StatsModel>();

        public StatsPage2()
        {
            InitializeComponent();
            getAllOrder();
        }

        public void getAllOrder()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
            {
                conn.Open();
                string sql = "SELECT * FROM lion.order";

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
            combineData();
        }

        public void combineData()
        {
            for (int i = 0; i < 3; i++)
            {
                int quantity = 0;
                int totalPrice = 0;
                int salePrice = 0;

                foreach(MenuModel menu in MainWindow.menuList)
                {
                    if (menu.category == (Category)i)
                    {
                        List<OrderModel> menuOrderList = orderList.Where(order => menu.idx == order.menuIdx).ToList();
                        foreach (OrderModel order in menuOrderList)
                        {
                            quantity += order.quantity;
                            totalPrice += order.quantity * menu.price;
                            salePrice += order.totalPrice;
                        }
                    }
                }
                statsList.Add(new StatsModel
                {
                    category = (Category)i,
                    quantity = quantity,
                    totalPrice = totalPrice,
                    salePrice = salePrice
                });
            }
            lvResult.ItemsSource = statsList.ToList();
        }
    }
}
