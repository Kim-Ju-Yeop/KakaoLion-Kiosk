using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using System.Windows;

namespace KakaoLion
{
    public partial class MenuWindow : Window
    {
        public MenuModel menu;

        public string imagePath { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int discount { get; set; }

        public MenuWindow(MenuModel menu)
        {
            InitializeComponent();

            this.menu = menu;
            this.DataContext = this;

            setData();
        }

        public void setData()
        {
            imagePath = menu.imagePath;
            name = menu.name;
            price = menu.price;
            discount = menu.discount;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (discount != menu.discount && discount >= 0 && discount <= 100)
            {
                using (MySqlConnection conn = new MySqlConnection(Constants.CONNSTR))
                {
                    conn.Open();
                    string sql = "UPDATE menu SET discount=" + discount + " WHERE idx=" + menu.idx;

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
                MenuModel menuItem = MainWindow.menuList.Find((item) => item.idx == menu.idx);
                menuItem.discount = discount;

                this.Close();
            } 
            else
            {
                if (discount == menu.discount) MessageBox.Show("이전의 할인율과 동일합니다.");
                else MessageBox.Show("0에서 100사이의 값을 입력해주세요.");
            }
        }
    }
}
