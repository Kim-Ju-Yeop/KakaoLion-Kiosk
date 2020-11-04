using System;
using System.Collections.Generic;
using System.Windows;
using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;

namespace KakaoLion.pages
{
    public partial class loginWIndow : Window
    {
        private static List<UserModel> userList = new List<UserModel>();

        public loginWIndow()
        {
            InitializeComponent();
            Loaded += LoginWIndow_Loaded;
        }

        private void LoginWIndow_Loaded(object sender, RoutedEventArgs e)
        {
            getUserInfo();
        }

        private void getUserInfo()
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
                        qrcode = (string)rdr["qrcode"],
                    });
                }
            }
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (UserModel u in userList)
            {
                

                if (u.id != textBoxId.Text.ToString() || u.pw != textBoxPw.Text.ToString())
                {
                    MessageBox.Show("로그인 정보가 올바르지 않습니다.", "KAKAO");
                } else
                {
                    var result = MessageBox.Show("로그인 성공 :)", "KAKAO");
                    if (result == MessageBoxResult.OK)
                    {

                        MainWindow MainWindow = new MainWindow();
                        MainWindow.Show();
                        this.Close();
                    }
                }
            }
        }
    }
}
