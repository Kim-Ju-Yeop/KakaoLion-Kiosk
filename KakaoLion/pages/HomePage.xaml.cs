using KakaoLion.pages;
using System.Windows;
using System.Windows.Controls;

namespace KakaoLion
{ 
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void orderButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PurchaseMethodPage());
        }
    }
}
