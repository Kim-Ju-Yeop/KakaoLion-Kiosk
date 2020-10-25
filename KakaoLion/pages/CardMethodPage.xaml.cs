using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using System.Windows.Controls;

namespace KakaoLion.pages
{
    public partial class CardMethodPage : Page
    {
        string userQrCode;

        public CardMethodPage()
        {
            InitializeComponent();
            getQrCode();
            setOrderInfo();

            dataBox.Focus();
            webcam.CameraIndex = 0;
        }

        private void getQrCode()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.CONNSTR))
            {
                conn.Open();
                string sql = "SELECT * FROM user WHERE id = " + "'" + "student" + "'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    userQrCode = (string)rdr["qrcode"];
                    userInfo.Text = (string)rdr["name"] + "님의 QR 코드 번호입니다.";
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
            if (userQrCode.Equals(dataBox.Text.ToString()))
            {
                statusView.Text = "데이터가 일치합니다.";
                // Result 페이지 이동
            }
            else
            {
                statusView.Text = "데이터가 일치하지 않습니다.";
                dataBox.Text = "";
            }
        }


        private void backBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PurchaseMethodPage());
        }

        private void webcam_QrDecoded(object sender, string e)
        {
            dataBox.Text = e;
        }
    }
}
