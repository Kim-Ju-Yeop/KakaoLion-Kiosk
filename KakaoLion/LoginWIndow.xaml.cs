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
        private bool isCheck = false;
        private static List<UserModel> userList = new List<UserModel>();

        public loginWIndow()
        {
            InitializeComponent();
            checkAutoLogin();
            getUserInfo();
        }

        private void checkAutoLogin()
        {
            bool isAutoLogin = Properties.Settings.Default.isAutoLogin;
            if (isAutoLogin)
            {
                MainWindow MainWindow = new MainWindow();
                MainWindow.Show();
                this.Close();
            }
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

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            bool isSuccess = false;

            foreach (UserModel user in userList)
            {
                if (user.id == textBoxId.Text.ToString() && user.pw == textBoxPw.Text.ToString())
                {
                    isSuccess = true;
                    break;
                }
            }

            if (isSuccess)
            {
                if (isCheck)
                {
                    Properties.Settings.Default.isAutoLogin = true;
                    Properties.Settings.Default.Save();
                }
                MainWindow MainWindow = new MainWindow();
                MainWindow.Show();
                this.Close();
            } 
            else
            {
                MessageBox.Show("로그인 정보가 올바르지 않습니다.", "KAKAO");
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            isCheck = true;
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            isCheck = false;
        }
    }
}
