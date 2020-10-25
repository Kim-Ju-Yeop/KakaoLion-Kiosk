using KakaoLion.model;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace KakaoLion.pages
{
    public partial class CardMethodPage : Page
    {
        public CardMethodPage()
        {
            InitializeComponent();
        }

        private void backBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PurchaseMethodPage());
        }
    }
}
