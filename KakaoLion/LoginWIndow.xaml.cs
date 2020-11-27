using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using KakaoLion.model;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KakaoLion
{
    public partial class LoginWindow : Window
    {
        private bool isCheck = false;
        private static List<UserModel> userList = new List<UserModel>();

        public LoginWindow()
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
                string userId = Properties.Settings.Default.userId;

                JObject json = new JObject();
                json.Add("MSGType", 0);
                json.Add("Id", userId);
                json.Add("Content", "");
                json.Add("ShopName", "");
                json.Add("OrderNumber", "");
                json.Add("Group", false);
                json.Add("Menus", "");

                byte[] buffer = new byte[4096];
                string message = JsonConvert.SerializeObject(json);
                buffer = Encoding.UTF8.GetBytes(message);

                App.stream.Write(buffer, 0, buffer.Length);
                App.userId = userId;

                MainWindow MainWindow = new MainWindow();
                MainWindow.Show();
                this.Close();
            }
        }

        private void getUserInfo()
        {
            using (MySqlConnection conn = new MySqlConnection(Constants.DATABASE_CONNSTR))
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
            string userId = "";
            bool isSuccess = false;

            foreach (UserModel user in userList)
            {
                if (user.id == textBoxId.Text.ToString() && user.pw == textBoxPw.Text.ToString())
                {
                    userId = user.id;
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

                Properties.Settings.Default.userId = userId;
                Properties.Settings.Default.Save();

                JObject json = new JObject();
                json.Add("MSGType", 0);
                json.Add("Id", userId);
                json.Add("Content", "");
                json.Add("ShopName", "");
                json.Add("OrderNumber", "");
                json.Add("Group", false);
                json.Add("Menus", "");

                byte[] buffer = new byte[4096];
                string message = JsonConvert.SerializeObject(json);
                buffer = Encoding.UTF8.GetBytes(message);

                App.stream.Write(buffer, 0, buffer.Length);
                App.userId = userId;

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
        private void Window_Closed(object sender, EventArgs e)
        {
            App.LoginWindow_CloseAction(true);
        }
    }
}
