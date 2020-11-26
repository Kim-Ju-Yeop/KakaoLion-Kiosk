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
    public partial class StatusPage5 : Page
    {
        public List<OrderModel> orderList = new List<OrderModel>();
        public SeriesCollection seriesCollection = new SeriesCollection();

        public StatusPage5()
        {
            InitializeComponent();
            setDate();

            this.DataContext = this;
        }

        public void setDate()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
            {
                conn.Open();
                string sql = "SELECT purchaseAt FROM lion.order";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string date = (string)rdr["purchaseAt"];
                    string formatDate = date.Substring(0, 10);

                    if (lbTime.Items.IndexOf(formatDate) == -1)
                    {
                        lbTime.Items.Add(formatDate);
                    }
                }
            }
            if (lbTime.Items.Count != 0) lbTime.SelectedIndex = 0;
            else
            {
                seriesCollection.Add(new PieSeries
                {
                    Title = "카드",
                    Values = new ChartValues<double> { 0 },
                    DataLabels = true,
                    LabelPoint = chartPoint =>
                    string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)
                });
                seriesCollection.Add(new PieSeries
                {
                    Title = "현금",
                    Values = new ChartValues<double> { 0 },
                    DataLabels = true,
                    LabelPoint = chartPoint =>
                    string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)
                });
                pieChart.Series = seriesCollection;

                total.Content = "총 매출액 : " + 0;
                netProfit.Content = "순수 매출액 : " + 0;
            }
        }

        public void getTodayAllOrder(string date)
        {
            int cardCount = 0;
            int moneyCount = 0;

            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
            {
                conn.Open();
                string sql = "SELECT * FROM lion.order WHERE purchaseAt LIKE '" + date + "%'";

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

            getTodayAllOrder(lbTime.SelectedItem.ToString());
        }
    }
}
