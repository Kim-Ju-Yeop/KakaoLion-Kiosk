using KakaoLion.database.repository;
using KakaoLion.database.repositoryImpl;
using KakaoLion.model;
using KakaoLion.server.repository;
using KakaoLion.server.repositoryImpl;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KakaoLion.pages
{
    public partial class ResultPage : Page
    {
        private int tik = 60;
        private int shopIdx;
        private int lastOrderCount;
        private DispatcherTimer dispatcherTimer;

        private OrderRepository orderRepository;
        private StoreRepository storeRepository;
        private OrderMessageRepository orderMessageRepository;

        public ResultPage()
        {
            InitializeComponent();

            orderRepository = new OrderRepositoryImpl();
            storeRepository = new StoreRepositoryImpl();
            orderMessageRepository = new OrderMessageRepositoryImpl();

            getLastOrderCount();
            setOrder();
        }

        public void getLastOrderCount()
        {
            lastOrderCount = orderRepository.getLastOrderCount();
        }

        public void setOrder()
        {
            orderRepository.insertOrder(lastOrderCount);

            if (OrderPage.orderList[0].shopIdx != null)
            {
                startTimer();
            }
            showData();
            sendMessage();
        }

        public void startTimer()
        {
            shopIdx = (int)OrderPage.orderList[0].shopIdx;
            storeRepository.updateStoreLastOrder(shopIdx);

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler(timer_Tick);
            dispatcherTimer.Start();
        }

        public void stopTimer()
        {
            storeRepository.updateStorePossible(shopIdx);
            dispatcherTimer.Stop();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            if (tik > 0)
            {
                tik--;
            }
            else
            {
                stopTimer();
            }
        }

        public void showData()
        {
            int totalCount = 0;
            int totalPrice = 0;
            string userName = "";
            string userPurchaseMethodId = "";

            foreach (OrderModel order in OrderPage.orderList)
            {
                totalCount += order.quantity;
                totalPrice += order.totalPrice;

                if (MainWindow.userList.Where(user => user.id == order.userId).ToList().Count != 0)
                {
                    UserModel user = MainWindow.userList.Where(u => u.id == order.userId).ToList()[0];
                    userName = user.name;

                    if (order.paymentMethod == false)
                    {
                        userPurchaseMethodId = user.qrcode;
                    }
                    else
                    {
                        userPurchaseMethodId = user.barcode;
                    }
                }
            }
            userInfo.Text = "이름 : " + userName + " | 번호 : " + userPurchaseMethodId;
            orderInfo.Text = totalCount + "개 " + totalPrice + "원";

            if (lastOrderCount + 1 < 10) orderCount.Text = "00" + (lastOrderCount + 1);
            else if (lastOrderCount + 1 < 100) orderCount.Text = "0" + (lastOrderCount + 1);
            else orderCount.Text = "" + (lastOrderCount + 1);
        }

        private void sendMessage()
        {
            if (App.isRunning)
            {
                orderMessageRepository.sendOrderMessage(lastOrderCount);
            }
            else
            {
                bool state = App.checkReconnectServer();
                if (state)
                {
                    orderMessageRepository.sendOrderMessage(lastOrderCount);
                }
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            OrderPage.orderList.Clear();
            this.NavigationService.Navigate(new HomePage());
        }
    }
}
