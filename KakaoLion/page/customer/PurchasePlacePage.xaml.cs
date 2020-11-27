using KakaoLion.model;
using System.Windows;
using System.Windows.Controls;

namespace KakaoLion.pages
{
    public partial class PurchasePlacePage : Page
    {
        public PurchasePlacePage()
        {
            InitializeComponent();
        }

        private void deliveryButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (OrderModel order in OrderPage.orderList)
            {
                order.paymentPlace = false;
            }
            this.NavigationService.Navigate(new PurchaseMethodPage());
        }

        private void storeButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (OrderModel order in OrderPage.orderList)
            {
                order.paymentPlace = true;
            }
            this.NavigationService.Navigate(new SelectStorePage());
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (OrderModel order in OrderPage.orderList)
            {
                order.paymentPlace = null;
            }
            this.NavigationService.Navigate(new OrderPage());
        }
    }
}
