using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using KakaoLion.database.repository;
using KakaoLion.database.repositoryImpl;
using KakaoLion.model;
using KakaoLion.server.repository;
using KakaoLion.server.repositoryImpl;
using KakaoLion.widget;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KakaoLion
{
    public partial class LoginWindow : Window
    {
        private bool isCheck = false;
        private List<UserModel> userList = new List<UserModel>();

        private LoginRepository loginRepository;
        private UserRepository userRepository;

        public LoginWindow()
        {
            InitializeComponent();

            loginRepository = new LoginRepositoryImpl();
            userRepository = new UserRepositoryImpl();

            checkAutoLogin();
        }

        private void checkAutoLogin()
        {
            bool isAutoLogin = Properties.Settings.Default.isAutoLogin;
            if (isAutoLogin)
            {
                string userId = Properties.Settings.Default.userId;

                loginRepository.sendLoginMessage(userId);
                App.userId = userId;

                MainWindow MainWindow = new MainWindow();
                MainWindow.Show();
                this.Close();
            } 
            else
            {
                getAllUser();
            }
        }

        private void getAllUser()
        {
            userList.Clear();
            userList = userRepository.getAllUser();
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
                }

                Properties.Settings.Default.userId = userId;
                Properties.Settings.Default.Save();

                loginRepository.sendLoginMessage(userId);
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
