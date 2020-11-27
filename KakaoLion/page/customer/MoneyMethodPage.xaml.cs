using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace KakaoLion.pages
{
    public partial class MoneyMethodPage : Page
    {

        public MoneyMethodPage()
        {
            InitializeComponent();
            setOrderInfo();

            dataBox.Focus();
        }

        private void setOrderInfo()
        {
            int totalCount = 0;
            int totalPrice = 0;

            foreach (OrderModel order in OrderPage.orderList)
            {
                totalCount += order.quantity;
                totalPrice += order.totalPrice;
            }

            orderInfo.Text = totalCount + "개 " + totalPrice + "원";
        }

        private void dataBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<UserModel> userList = MainWindow.userList.Where(user => user.barcode == dataBox.Text.ToString()).ToList();

            if (userList.Count != 0)
            {
                statusView.Text = "데이터가 일치합니다.";

                foreach (OrderModel order in OrderPage.orderList)
                {
                    order.userId = userList.FirstOrDefault().id;
                }
                this.NavigationService.Navigate(new ResultPage());
            } 
            else
            {
                statusView.Text = "데이터가 일치하지 않습니다.";
            }
        }

        private void backBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PurchaseMethodPage());
        }
    }
}