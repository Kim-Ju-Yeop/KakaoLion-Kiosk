using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private void storeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = sender as ListBox;
            var store = (StoreModel)item.SelectedItem;

            Console.WriteLine(store.name);
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
    }
}