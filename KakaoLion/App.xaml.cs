using KakaoLion.widget;
using System;
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
        private static TcpClient client = new TcpClient();
        public static NetworkStream stream;

        private static bool isRunning = true;
        private static bool isLoginWindowClosed = false;
        private static bool isMainWindowClosed = false;

        public App()
        { 
            client.Connect(Constants.SERVER_CONNSTR, Constants.PORT);
            stream = client.GetStream();

            Thread thread = new Thread(new ThreadStart(messageThread));
            thread.Start();
        }

        public static void LoginWindow_CloseAction(bool isClosed)
        {
            isLoginWindowClosed = isClosed;
        }

        public static void MainWindow_ClosedAction(bool isClosed)
        {
            isMainWindowClosed = isClosed;
        }

        public void messageThread()
        {
            byte[] buffer = new byte[1024];
            string msg;

            while (isRunning)
            {
               try
               {
                  stream.Read(buffer, 0, buffer.Length);
                  msg = Encoding.UTF8.GetString(buffer);

                  MessageBox.Show(msg);
               }
               catch (Exception e)
               {
                 Console.WriteLine(e.Message);
               }
            }
            client.Close();
            stream.Close();
        }
    }
}
