using KakaoLion.model;
using KakaoLion.server.repository;
using KakaoLion.server.repositoryImpl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace KakaoLion.pages.admin
{
    public partial class ChatPage : Page
    {
        private bool person;
        private bool group;

        private LoginMessageRepository loginMessageRepository;
        private GeneralMessageRepository generalMessageRepository;
        private List<MessageModel> messageList = new List<MessageModel>();

        public ChatPage()
        {
            InitializeComponent();

            loginMessageRepository = new LoginMessageRepositoryImpl();
            generalMessageRepository = new GeneralMessageRepositoryImpl();

            person = true;
            personButton.IsChecked = true;

            lvResult.ItemsSource = messageList;
            DataContext = true;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == "")
            {
                MessageBox.Show("값을 입력해주시기 바랍니다.");
            }
            else
            {
                if (App.isRunning)
                {
                    if (!Properties.Settings.Default.isLogin)
                    {
                        loginMessageRepository.sendLoginMessage(Properties.Settings.Default.userId.ToString());
                    }
                    sendMessage();
                } 
                else
                {
                    bool state = App.checkReconnectServer();
                    if (state)
                    {
                        if (!Properties.Settings.Default.isLogin)
                        {
                            loginMessageRepository.sendLoginMessage(Properties.Settings.Default.userId.ToString());
                        }
                        loginMessageRepository.sendLoginMessage(Properties.Settings.Default.userId.ToString());
                        sendMessage();
                    }
                }
                textBox.Text = "";
            }
        }

        private void sendMessage()
        {
            string userId = Properties.Settings.Default.userId.ToString();
            bool isGroup = person == true ? false : true;

            generalMessageRepository.sendGeneralMessage(userId, textBox.Text, isGroup);

            string target = person == true ? "개인" : "그룹";
            string date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

            messageList.Add(new MessageModel
            {
                message = textBox.Text,
                target = target,
                messageAt = date
            });
            lvResult.ItemsSource = messageList;
            lvResult.Items.Refresh();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;

            if (button.Content.ToString().Equals("개인"))
            {
                person = true;
                group = false;
            } 
            else
            {
                person = false;
                group = true;
            }
        }
    }
}
