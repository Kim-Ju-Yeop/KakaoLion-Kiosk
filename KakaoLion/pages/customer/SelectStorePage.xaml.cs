using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KakaoLion.pages
{
    public partial class SelectStorePage : Page
    {
        public SelectStorePage()
        {
            InitializeComponent();
            getAllStore();
        }

        private void getAllStore()
        {
            List<StoreModel> storeList = new List<StoreModel>();

            using (MySqlConnection conn = new MySqlConnection(Constants.CONNSTR))
            {
                conn.Open();
                string sql = "SELECT * FROM shop";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    bool possible = int.Parse(rdr["possible"].ToString()) == 1;
                    storeList.Add(new StoreModel() { idx = (int)rdr["idx"], name = (string)rdr["name"], lastOrder = (string)rdr["lastOrder"], possible = possible });
                }

                storeListBox.ItemsSource = storeList.ToList();
            }
        }

        private void storeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = sender as ListBox;
            var store = (StoreModel)item.SelectedItem;

            foreach (OrderModel order in OrderPage.orderList)
            {
                order.shopIdx = store.idx;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            foreach (OrderModel order in OrderPage.orderList)
            {
                order.shopIdx = null;
            }
            this.NavigationService.Navigate(new PurchasePlacePage());
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (OrderPage.orderList[0].shopIdx == null)
            {
                MessageBox.Show("지점을 선택해주시기 바랍니다.");
            }
            else
            {
                this.NavigationService.Navigate(new PurchaseMethodPage());
            }
        }
    }
}