using KakaoLion.database.repository;
using KakaoLion.database.repositoryImpl;
using KakaoLion.model;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace KakaoLion.pages.admin
{
    public partial class StatusPage5 : Page
    {
        private List<string> purchaseAtList = new List<string>();
        private List<OrderModel> orderList = new List<OrderModel>();
        private SeriesCollection seriesCollection = new SeriesCollection();

        private OrderRepository orderRepository;

        public StatusPage5()
        {
            InitializeComponent();

            orderRepository = new OrderRepositoryImpl();
            this.DataContext = this;

            setDate();
        }

        public void setDate()
        {
            purchaseAtList.Clear();
            purchaseAtList = orderRepository.getAllPurchaseAt();

            foreach (string purchaseAt in purchaseAtList)
            {
                if (lbTime.Items.IndexOf(purchaseAt) == -1)
                {
                    lbTime.Items.Add(purchaseAt);
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

            orderList.Clear();
            orderList = orderRepository.getTodayAllOrder(date);

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

            getTodayAllOrder(lbTime.SelectedItem.ToString());
        }
    }
}
