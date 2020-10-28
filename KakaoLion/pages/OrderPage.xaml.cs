using KakaoLion.dto.model;
using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KakaoLion.pages
{
    public partial class OrderPage : Page
    {
        public int pageCount = 1;

        public static List<MenuModel> menuList = new List<MenuModel>();
        public static ObservableCollection<OrderModel> orderList = new ObservableCollection<OrderModel>();

        public OrderPage()
        {
            InitializeComponent();

            menuList.Clear();
            getAllMenu();

            Loaded += OrderPage_Loaded;
            orderList.CollectionChanged += OrderList_CollectionChanged;
        }

        private void OrderPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DataContext = this;
            previous.IsEnabled = false;
            lbCategory.SelectedIndex = 0;

            orderCount.Content = "0개";
            orderPrice.Content = "0원";

            lvResult.ItemsSource = orderList.ToList();
            OrderList_CollectionChanged(null, null);
        }

        private void getAllMenu()
        {
            using(MySqlConnection conn = new MySqlConnection(Constants.CONNSTR))
            {
                conn.Open();
                string sql = "SELECT * FROM menu";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while(rdr.Read())
                {
                    string imagePath = "";

                    switch ((Category) rdr["category"])
                    {
                        case Category.Small:
                            imagePath = "/resources/image/small/" + rdr["name"] + ".jpg";
                            break;
                        case Category.Medium:
                            imagePath = "/resources/image/medium/" + rdr["name"] + ".jpg";
                            break;
                        case Category.Big:
                            imagePath = "/resources/image/big/" + rdr["name"] + ".jpg";
                            break;
                    }

                    menuList.Add(new MenuModel
                    {
                        idx = (int)rdr["idx"],
                        page = (int)rdr["page"],
                        category = (Category) rdr["category"],
                        name = (string)rdr["name"],
                        price = (int)rdr["price"],
                        discount = (int)rdr["discount"],
                        imagePath = imagePath
                    });
                }
            }
        }

        private void lbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pageCount = 1;
            previous.IsEnabled = false;

            if (lbCategory.SelectedIndex == 2) next.IsEnabled = false;
            else next.IsEnabled = true;

            lbMenus.ItemsSource = PageChange();
        }

        private void previous_Click(object sender, RoutedEventArgs e)
        {
            pageCount--;
            if (pageCount == 1) previous.IsEnabled = false;

            next.IsEnabled = true;
            lbMenus.ItemsSource = PageChange();
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            pageCount++;
            if (pageCount == 3) next.IsEnabled = false;

            previous.IsEnabled = true;
            lbMenus.ItemsSource = PageChange();
        }

        public List<MenuModel> PageChange()
        {
            Category category = (Category)lbCategory.SelectedIndex;
            return menuList.Where(menu => menu.page == pageCount && menu.category == category).ToList();
        }

        private void lbMenus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var menu = (MenuModel) lbMenus.SelectedItem;

            if (menu != null)
            {
                OrderModel orderModel = orderList.Where(order => order.menuIdx == menu.idx).FirstOrDefault();

                if (orderModel == null)
                {
                    orderList.Add(new OrderModel()
                    {
                        idx = null,
                        orderCount = null,
                        menuIdx = menu.idx,
                        quantity = 1,
                        totalPrice = menu.price,
                        userId = "user",
                        purchaseAt = null,
                        paymentPlace = null,
                        paymentMethod = null,
                        shopIdx = null
                    });
                }
                else
                {
                    int index = orderList.IndexOf(orderModel);
                    orderList[index].quantity += 1;
                    orderList[index].totalPrice = orderList[index].quantity * menu.price;
                }

                lvResult.ItemsSource = orderList.ToList();
                OrderList_CollectionChanged(null, null);
                lbMenus.UnselectAll();
            }
        }
 
        private void Up(object sender, RoutedEventArgs e)
        {
            var orderModel = (sender as Button).DataContext as OrderModel;
            List<MenuModel> menuModel = menuList.Where(menu => menu.idx.Equals(orderModel.menuIdx)).ToList();

            int index = orderList.IndexOf(orderModel);
            orderList[index].quantity += 1;
            orderList[index].totalPrice = orderList[index].quantity * menuModel[0].price;

            lvResult.ItemsSource = orderList.ToList();
            OrderList_CollectionChanged(null, null);
        }   

        private void Down(object sender, RoutedEventArgs e)
        {
            var orderModel = (sender as Button).DataContext as OrderModel;
            List<MenuModel> menuModel = menuList.Where(menu => menu.idx.Equals(orderModel.menuIdx)).ToList();

            int index = orderList.IndexOf(orderModel);

            if (orderList[index].quantity == 1)
            {
                orderList.Remove(orderList[index]);
            } 
            else
            {
                orderList[index].quantity -= 1;
                orderList[index].totalPrice = orderList[index].quantity * menuModel[0].price;
            }
            lvResult.ItemsSource = orderList.ToList();
            OrderList_CollectionChanged(null, null);
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            var orderModel = (sender as Button).DataContext as OrderModel;

            int index = orderList.IndexOf(orderModel);
            orderList.Remove(orderList[index]);
            
            lvResult.ItemsSource = orderList.ToList();
        }

        private void OrderList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            int totalCount = 0;
            int totalPrice = 0;

            foreach (OrderModel order in orderList)
            {
                totalCount += order.quantity;
                totalPrice += order.totalPrice;
            }
            orderCount.Content = totalCount + "개";
            orderPrice.Content = totalPrice + "원";

            if (orderList.Count == 0)
            {
                resetButton.IsEnabled = false;
                orderButton.IsEnabled = false;
            } else
            {
                resetButton.IsEnabled = true;
                orderButton.IsEnabled = true;
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("주문목록을 초기화하시겠습니까?\n(주문 목록이 삭제됩니다.)", "초기화", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                orderList.Clear();
                lvResult.ItemsSource = orderList.ToList();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (orderList.Count == 0)
            {
                this.NavigationService.Navigate(new HomePage());
            } 
            else if (MessageBox.Show("정말로 이전화면으로 돌아가시겠습니까?\n(주문 목록이 삭제됩니다.)", "이전으로", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                orderList.Clear();
                this.NavigationService.Navigate(new HomePage());
            }
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PurchasePlacePage());
        }
    }
}
