using KakaoLion.database.repository;
using KakaoLion.database.repositoryImpl;
using KakaoLion.dto.model;
using KakaoLion.model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace KakaoLion.pages.admin
{
    public partial class StatsPage2 : Page
    {
        private List<OrderModel> orderList = new List<OrderModel>();
        private List<StatsModel> statsList = new List<StatsModel>();

        private OrderRepository orderRepository;

        public StatsPage2()
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
