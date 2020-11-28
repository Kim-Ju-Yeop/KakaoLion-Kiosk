using KakaoLion.database.repository;
using KakaoLion.database.repositoryImpl;
using KakaoLion.model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace KakaoLion.pages.admin
{
    public partial class StatsPage1 : Page
    {
        private List<OrderModel> orderList = new List<OrderModel>();
        private List<OrderModel> statsList = new List<OrderModel>();

        private OrderRepository orderRepository;

        public StatsPage1()
        {
            InitializeComponent();

            orderRepository = new OrderRepositoryImpl();

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
            foreach(MenuModel menu in MainWindow.menuList)
            {
                int quantity = 0;
                int totalPrice = 0;
                int salePrice = 0;

                List<OrderModel> menuOrderList = orderList.Where(order => menu.idx == order.menuIdx).ToList();
                foreach(OrderModel order in menuOrderList)
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
    }
}
