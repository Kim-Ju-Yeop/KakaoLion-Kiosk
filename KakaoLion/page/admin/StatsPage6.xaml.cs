using KakaoLion.database.repository;
using KakaoLion.database.repositoryImpl;
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
        private List<OrderModel> orderList = new List<OrderModel>();
        private SeriesCollection seriesCollection = new SeriesCollection();

        private OrderRepository orderRepository;

        public StatsPage6()
        {
            InitializeComponent();

            orderRepository = new OrderRepositoryImpl();

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

            orderList.Clear();
            orderList = orderRepository.getAllTimeOrder(hour);

            foreach (OrderModel order in orderList)
            {
                if ((bool)order.paymentMethod)
                {
                    cardCount++;
                }
                else
                {
                    moneyCount++;
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
