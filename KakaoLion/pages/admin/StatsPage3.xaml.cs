using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace KakaoLion.pages.admin
{
    public partial class StatsPage3 : Page
    {
        public List<OrderModel> orderList = new List<OrderModel>();
        public List<OrderModel> statsList = new List<OrderModel>();

        public StatsPage3()
        {
            InitializeComponent();
            setStore();
        }

        public void setStore()
        {
            foreach (StoreModel store in MainWindow.storeList)
            {
                lbStore.Items.Add(store.name);
            }
            lbStore.SelectedIndex = 0;
        }

        public void getAllOrder(int storeIdx)
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.CONNSTR))
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
            combineData(storeIdx);
        }

        public void combineData(int storeIdx)
        {
            foreach (MenuModel menu in MainWindow.menuList)
            {
                int quantity = 0;
                int totalPrice = 0;

                List<OrderModel> menuOrderList = orderList.Where(order => menu.idx == order.menuIdx && order.shopIdx == storeIdx).ToList();
                foreach (OrderModel order in menuOrderList)
                {
                    quantity += order.quantity;
                    totalPrice += order.totalPrice;
                }

                statsList.Add(new OrderModel
                {
                    idx = null,
                    orderCount = null,
                    menuIdx = menu.idx,
                    quantity = quantity,
                    totalPrice = totalPrice,
                    userId = null,
                    purchaseAt = null,
                    paymentPlace = null,
                    paymentMethod = null,
                    shopIdx = null
                });
            }
            lvResult.ItemsSource = statsList.ToList();
        }

        private void lbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            orderList.Clear();
            statsList.Clear();

            getAllOrder(lbStore.SelectedIndex+1);
        }
    }
}
