using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using System.Windows.Controls;

namespace KakaoLion.pages
{
    public partial class MoneyMethodPage : Page
    {
        string userBarcode;

        public MoneyMethodPage()
        {
            InitializeComponent();

            getBarcode();
            setOrderInfo();

            dataBox.Focus();
        }

        private void getBarcode()
        {
            using(MySqlConnection conn = new MySqlConnection(Constants.CONNSTR))
            {
                conn.Open();
                string sql = "SELECT * FROM user WHERE id = " + "'" + "student1" + "'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    userBarcode = (string) rdr["barcode"];
                    userInfo.Text = (string)rdr["name"] + "님의 바코드 번호입니다.";
                }
            }
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
            if (userBarcode.Equals(dataBox.Text.ToString()))
            {
                statusView.Text = "데이터가 일치합니다.";
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