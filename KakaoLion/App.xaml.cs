using KakaoLion.database.repository;
using KakaoLion.database.repositoryImpl;
using KakaoLion.model;
using KakaoLion.server.repository;
using KakaoLion.server.repositoryImpl;
using KakaoLion.widget;
using KakaoLion.widget.extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

/**
 * 쓰레드 자동 종료 기능 구현 해야함
 */

namespace KakaoLion
{
    public partial class App : Application
    {
        public static string userId;
        public static bool isRunning;
        public static bool isLoginWindowClosed = false;
        public static bool isMainWindowClosed = false;

        public static NetworkStream stream;

        public static TcpClient client;
        public static List<MenuModel> menuList = new List<MenuModel>();
        public static List<OrderModel> orderList = new List<OrderModel>();

        private static MenuRepository menuRepository;
        private static OrderRepository orderRepository;
        private static GeneralMessageRepository generalMessageRepository;

        public App()
        {
            menuRepository = new MenuRepositoryImpl();
            orderRepository = new OrderRepositoryImpl();
            generalMessageRepository = new GeneralMessageRepositoryImpl();

            connectServer();
            getAllMenu();
        }

        public static void connectServer()
        { 
            isRunning = true;

            try
            {
                client = new TcpClient();
                client.Connect(Constants.SERVER_CONNSTR, Constants.PORT);
                stream = client.GetStream();

                Thread thread = new Thread(new ThreadStart(messageThread));
                thread.Start();
            } 
            catch (Exception e)
            {
                isRunning = false;
                client.Close();
            }
        }

        public static void messageThread()
        {
            byte[] buffer = new byte[1024];
            string msg;

            while (isRunning)
            {
               if (client.Client.Receive(buffer, SocketFlags.Peek) != 0)
               {
                    try
                    {
                        stream.Read(buffer, 0, buffer.Length);
                        msg = Encoding.UTF8.GetString(buffer);

                        if (msg.Contains("총매출액")) getTodayAllOrder();
                        MessageBox.Show(msg);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                }
                else
                {
                    MessageBox.Show("서버와 연결이 유실되었습니다.");
                    break;
                }
            }
            isRunning = false;

            client.Close();
            stream.Close();
        }

        public static bool checkReconnectServer()
        {
            while (true)
            {
                if (MessageBox.Show("서버와 연결이 유실되었습니다.\n(서버와 재연결을 하시겠습니까?)", "서버 재연결", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    connectServer();

                    if (isRunning)
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public static void getAllMenu()
        {
            menuList.Clear();
            menuList = menuRepository.getAllMenu();
        }

        public static void getTodayAllOrder()
        {
            orderList.Clear();
            orderList = orderRepository.getTodayAllOrder(DateTImeExtension.dateTimeFormat4(DateTime.Now));

            getTotalPrice();
        }

        public static void getTotalPrice()
        {
            int totalPrice = 0;

            foreach (MenuModel menu in menuList)
            {
                List<OrderModel> menuOrderList = orderList.Where(order => menu.idx == order.menuIdx).ToList();
                foreach (OrderModel order in menuOrderList)
                {
                    totalPrice += menu.price * order.quantity;
                }
            }
            getNetProfit(totalPrice);
        }

        public static void getNetProfit(int totalPrice)
        {
            int totalNetProfitPrice = 0;

            foreach (OrderModel order in orderList)
            {
                totalNetProfitPrice += order.totalPrice;
            }
            sendMessage(totalPrice, totalNetProfitPrice);
        }

        public static void sendMessage(int totalPrice, int totalNetProfitPrice)
        {
            string content = "총 금액 : " + totalPrice + " 순수 이익 : " + totalNetProfitPrice;

            generalMessageRepository.sendGeneralMessage2(userId, content);
        }

        public static void LoginWindow_CloseAction(bool isClosed)
        {
            isLoginWindowClosed = isClosed;
        }

        public static void MainWindow_ClosedAction(bool isClosed)
        {
            isMainWindowClosed = isClosed;
        }
    }
}
