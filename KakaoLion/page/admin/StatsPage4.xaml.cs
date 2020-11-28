using KakaoLion.database.repository;
using KakaoLion.database.repositoryImpl;
using KakaoLion.dto.model;
using KakaoLion.model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace KakaoLion.pages.admin
{
    public partial class StatsPage4 : Page
    {
        private List<OrderModel> orderList = new List<OrderModel>();
        private List<StatsModel> statsList = new List<StatsModel>();

        private OrderRepository orderRepository;

        public StatsPage4()
        {
            InitializeComponent();

            orderRepository = new OrderRepositoryImpl();

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
            orderList.Clear();
            orderList = orderRepository.getAllOrder();

            combineData(storeIdx);
        }

        public void combineData(int storeIdx)
        {
            for (int i = 0; i < 3; i++)
            {
                int quantity = 0;
                int totalPrice = 0;
                int salePrice = 0;

                foreach (MenuModel menu in MainWindow.menuList)
                {
                    if (menu.category == (Category)i)
                    {
                        List<OrderModel> menuOrderList = orderList.Where(order => menu.idx == order.menuIdx && order.shopIdx == storeIdx).ToList();
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

        private void lbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            orderList.Clear();
            statsList.Clear();

            getAllOrder(lbStore.SelectedIndex + 1);
        }
    }
}
