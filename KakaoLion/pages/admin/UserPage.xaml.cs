using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace KakaoLion.pages.admin
{
    public partial class UserPage : Page
    {
        public List<UserModel> userList = new List<UserModel>();

        public UserPage()
        {
            InitializeComponent();
            getAllUser();
        }

        public void getAllUser()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.CONNSTR))
            {
                conn.Open();
                string sql = "SELECT * FROM lion.user";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    userList.Add(new UserModel
                    {
                        id = (string)rdr["id"],
                        pw = (string)rdr["pw"],
                        name = (string)rdr["name"],
                        address = (string)rdr["address"],
                        barcode = (string)rdr["barcode"],
                        qrcode = (string)rdr["qrcode"]
                    });
                }
            }
            lvResult.ItemsSource = userList.ToList();
        }
    }
}
