using KakaoLion.model;
using KakaoLion.widget;
using LiveCharts;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace KakaoLion.pages.admin
{
    public partial class StatsPage6 : Page
    {
        public List<OrderModel> orderList = new List<OrderModel>();
        public SeriesCollection seriesCollection = new SeriesCollection();

        public StatsPage6()
        {
            InitializeComponent();
            setTime();
        }

        public void setTime()
        {
            for (int i = 0; i < 24; i++) 
            {
                string item = i < 10 ? "0" + i + "시" : i + "시";
                lbTime.Items.Add(item);
            }
            lbTime.SelectedIndex = 0;
        }

        public void getAllTimeOrder(string hour)
        {
            int cardCount = 0;
            int moneyCount = 0;

            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
            {
                conn.Open();
                string time = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                string sql = "SELECT * FROM lion.order WHERE purchaseAt LIKE '" + time + " " + hour + "%'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Boolean paymentPlace = rdr["paymentPlace"].ToString().Equals("0") ? true : false;
                    Boolean paymentMethod = rdr["paymentMethod"].ToString().Equals("0") ? true : false;

                    if (paymentMethod) cardCount++;
                    else moneyCount++;

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
            seriesCollection.Add(new PieSeries
            {
                Title = "카드",
                Values = new ChartValues<double> { cardCount },
                DataLabels = true,
                LabelPoint = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)
            });
            seriesCollection.Add(new PieSeries
            {
                Title = "현금",
                Values = new ChartValues<double> { moneyCount },
                DataLabels = true,
                LabelPoint = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)
            });
            pieChart.Series = seriesCollection;

            getTotalPrice();
            getNetProfit();
        }

        public void getTotalPrice()
        {
            int totalPrice = 0;

            foreach (MenuModel menu in MainWindow.menuList)
            {
                List<OrderModel> menuOrderList = orderList.Where(order => menu.idx == order.menuIdx).ToList();
                foreach (OrderModel order in menuOrderList)
                {
                    totalPrice += menu.price * order.quantity;
                }
            }
            total.Content = "총 매출액 : " + totalPrice;
        }

        public void getNetProfit()
        {
            int totalNetProfitPrice = 0;

            foreach (OrderModel order in orderList)
            {
                totalNetProfitPrice += order.totalPrice;
            }
            netProfit.Content = "순수 매출액 : " + totalNetProfitPrice;
        }

        private void lbTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            orderList.Clear();
            seriesCollection.Clear();

            getAllTimeOrder(lbTime.SelectedItem.ToString().Substring(0, 2));
        }
    }
}
