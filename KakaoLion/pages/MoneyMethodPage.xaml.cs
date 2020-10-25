using KakaoLion.model;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace KakaoLion.pages
{
    public partial class MoneyMethodPage : Page
    {
        public MoneyMethodPage()
        {
            InitializeComponent();  
        }

        private void backBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PurchaseMethodPage());
        }
    }
}
