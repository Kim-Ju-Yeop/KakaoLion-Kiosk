using System.Collections.Generic;
using System.Management;
using System.Windows.Controls;

namespace KakaoLion.pages
{
    public partial class PurchaseMethodPage : Page
    {
        public List<Menu> myMenu { get; set; }

        public PurchaseMethodPage()
        {
            InitializeComponent();

            myMenu = new List<Menu>();

            Menu menu1 = new Menu();
            menu1.Name = "미니 페이스 라이언";//"res.name";
            menu1.count = 1;
            menu1.Price = 5500;

            Menu menu2 = new Menu();
            menu2.Name = "미니 페이스 리틀 라이언";//"res.name";
            menu2.count = 2;
            menu2.Price = 5500;

            Menu menu3 = new Menu();
            menu3.Name = "마린 마스코트 키체인 라이언";//"res.name";
            menu3.count = 4;
            menu3.Price = 5500;

            Menu statistics = new Menu();
            statistics.Name = "총 개수, 수량";
            statistics.count = menu1.count + menu2.count + menu3.count;
            statistics.Price = menu1.Price + menu2.Price + menu3.Price;

            myMenu.Add(menu1);
            myMenu.Add(menu2);
            myMenu.Add(menu3);

            DataContext = this;

        }

        private void moneyBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PurchaseQRcodePage());
        }

        private void cardBtn_click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
