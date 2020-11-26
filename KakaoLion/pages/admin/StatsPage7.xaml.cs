using KakaoLion.model;
using KakaoLion.widget;
using LiveCharts;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace KakaoLion.pages.admin
{
    public partial class StatsPage7 : Page
    {
        public List<UserModel> userList = new List<UserModel>();
        public List<OrderModel> orderList = new List<OrderModel>();
        public List<OrderModel> statsList = new List<OrderModel>();
        public SeriesCollection seriesCollection = new SeriesCollection();

        bool sortMethod;

        public StatsPage7()
        {
            InitializeComponent();
            getAllUser();
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
                        qrcode = (string)rdr["qrcode"],
                    });
                    lbUser.Items.Add((string)rdr["id"]);
                }
            }
            lbUser.SelectedIndex = 0;
        }

        public void getUserOrder(string userId)
        {
            int cardCount = 0;
            int moneyCount = 0;

            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
            {
                conn.Open();
                string sql = "SELECT * FROM lion.order WHERE userId = '" + userId + "';";

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
            int resultPrice = 0;

            foreach (MenuModel menu in MainWindow.menuList)
            {
                int quantity = 0;
                int totalPrice = 0;
                int salePrice = 0;

                List<OrderModel> menuOrderList = orderList.Where(order => menu.idx == order.menuIdx).ToList();
                foreach (OrderModel order in menuOrderList)
                {
                    quantity += order.quantity;
                    totalPrice += order.quantity * menu.price;
                    salePrice += order.totalPrice;
                }

                resultPrice += totalPrice;

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
                    shopIdx = null,
                    salePrice = salePrice,
                    discount = menu.discount
                });
            }
            total.Content = "총 매출액 : " + resultPrice;
            lvResult.ItemsSource = statsList.ToList();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvResult.ItemsSource);

            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("totalPrice", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("quantity", ListSortDirection.Ascending));
            view.Refresh();
            sortMethod = false;
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

        private void lbUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            orderList.Clear();
            statsList.Clear();
            seriesCollection.Clear();

            getUserOrder(lbUser.SelectedItem.ToString());
        }

        private void lvResult_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvResult.ItemsSource);
            if (sortMethod)
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("totalPrice", ListSortDirection.Ascending));
                view.SortDescriptions.Add(new SortDescription("quantity", ListSortDirection.Ascending));
                view.Refresh();
                sortMethod = false;
            }
            else
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("totalPrice", ListSortDirection.Descending));
                view.SortDescriptions.Add(new SortDescription("quantity", ListSortDirection.Descending));
                view.Refresh();
                sortMethod = true;
            }
        }
    }
}
