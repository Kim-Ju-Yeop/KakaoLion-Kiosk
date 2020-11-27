using KakaoLion.model;
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
        public bool person;
        public bool group;

        public List<MessageModel> messageList = new List<MessageModel>();

        public ChatPage()
        {
            InitializeComponent();

            person = true;
            personButton.IsChecked = true;

            lvResult.ItemsSource = messageList;
            this.DataContext = true;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == "")
            {
                MessageBox.Show("값을 입력해주시기 바랍니다.");
            }
            else
            {
                JObject json = new JObject();

                string userId = Properties.Settings.Default.userId.ToString();
                bool isGroup = person == true ? false : true;
                string target = person == true ? "개인" : "그룹";
                string date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

                json.Add("MSGType", 1);
                json.Add("Id", userId);
                json.Add("Content", textBox.Text);
                json.Add("ShopName", "");
                json.Add("OrderNumber", "");
                json.Add("Group", isGroup);
                json.Add("Menus", "");

                messageList.Add(new MessageModel
                {
                    message = textBox.Text,
                    target = target,
                    messageAt = date
                }) ;
                lvResult.ItemsSource = messageList;
                lvResult.Items.Refresh();

                byte[] buffer = new byte[4096];
                string message = JsonConvert.SerializeObject(json);
                buffer = Encoding.UTF8.GetBytes(message);

                App.stream.Write(buffer, 0, buffer.Length);
                textBox.Text = "";
            }
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
