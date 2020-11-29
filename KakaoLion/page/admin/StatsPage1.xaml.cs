using KakaoLion.csv.repository;
using KakaoLion.csv.repositoryImpl;
using KakaoLion.database.repository;
using KakaoLion.database.repositoryImpl;
using KakaoLion.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;

namespace KakaoLion.pages.admin
{
    public partial class StatsPage1 : Page
    {
        private List<OrderModel> orderList = new List<OrderModel>();
        private List<OrderModel> statsList = new List<OrderModel>();

        private OrderRepository orderRepository;
        private StatsRepository1 statsRepository1;

        public StatsPage1()
        {
            InitializeComponent();

            orderRepository = new OrderRepositoryImpl();
            statsRepository1 = new StatsRepositoryImpl1();

            getAllOrder();
        }

        public void getAllOrder()
        {
            orderList.Clear();
            orderList = orderRepository.getAllOrder();

            combineData();
        }

        public void combineData()
        {
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
            lvResult.ItemsSource = statsList;
        }

        private void ExportButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            List<string> stats = new List<string>();
            for (int i = 0; i < lvResult.Items.Count; i++)
            {
                string strText = "";
                OrderModel order = (OrderModel)lvResult.Items[i];

                strText += MainWindow.menuList.Where(menu => menu.idx == order.menuIdx).ToList()[0].name + "\t";
                strText += order.quantity + "\t";
                strText += order.discount + "\t";
                strText += statsList[i].totalPrice + "\t";
                strText += statsList[i].salePrice;

                stats.Add(strText);
            }
            statsRepository1.exportStats(stats);
        }
    }
}
