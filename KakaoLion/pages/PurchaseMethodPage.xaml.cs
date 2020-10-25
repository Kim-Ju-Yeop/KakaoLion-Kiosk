using KakaoLion.model;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace KakaoLion.pages
{
    public partial class PurchaseMethodPage : Page
    {
        public PurchaseMethodPage()
        {
            InitializeComponent();
        }

        private void moneyBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (OrderModel order in OrderPage.orderList)
            {
                order.paymentMethod = true;
            }
            this.NavigationService.Navigate(new MoneyMethodPage());
        }

        private void cardBtn_click(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (OrderModel order in OrderPage.orderList)
            {
                order.paymentMethod = false;
            }
            this.NavigationService.Navigate(new CardMethodPage());
        }

        private void backBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (OrderModel order in OrderPage.orderList)
            {
                order.paymentMethod = null;
            }
            this.NavigationService.Navigate(new PurchasePlacePage());
        }
    }
}
