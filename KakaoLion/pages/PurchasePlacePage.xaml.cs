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

        private void storeButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SelectStorePage());
        }

        private void deliveryButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PurchaseMethodPage());
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new OrderPage());
        }
    }
}
