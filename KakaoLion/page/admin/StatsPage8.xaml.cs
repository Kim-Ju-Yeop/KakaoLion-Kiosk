using KakaoLion.database.repository;
using KakaoLion.database.repositoryImpl;
using KakaoLion.model;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace KakaoLion.page.admin
{
    public partial class StatsPage8 : Page
    {
        private List<OrderModel> orderList = new List<OrderModel>();
        private SeriesCollection seriesCollection = new SeriesCollection();

        private OrderRepository orderRepository;

        private int totalPrice = 0;
        private int totalNetProfitPrice = 0;

        public StatsPage8()
        {
            InitializeComponent();

            orderRepository = new OrderRepositoryImpl();

            getAllOrder();
        }

        public void getAllOrder()
        {
            int cardCount = 0;
            int moneyCount = 0;

            orderList.Clear();
            orderList = orderRepository.getAllOrder();

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
            getDiscountPrice();
        }

        public void getTotalPrice()
        {
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
            foreach (OrderModel order in orderList)
            {
                totalNetProfitPrice += order.totalPrice;
            }
            netProfit.Content = "순수 매출액 : " + totalNetProfitPrice;
        }

        public void getDiscountPrice()
        {
            discount.Content = "할인 금액 : " + (totalPrice - totalNetProfitPrice);
        }
    }
}
