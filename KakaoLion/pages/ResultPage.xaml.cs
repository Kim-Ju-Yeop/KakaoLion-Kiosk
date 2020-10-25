using KakaoLion.model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KakaoLion.pages
{
    public partial class ResultPage : Page
    {
        int tik = 60;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public ResultPage()
        {
            InitializeComponent();
            startTimer();

            // 전달받은 orders 데이터에서 userId, purchaseAt 데이터를 추가하여 DB에 주문목록을 넣는다.
        }

        public void startTimer()
        {
            // DB 접근 후 특정 지점 possible 값을 false로 변경
            // DB 접근 후 특정 지점 lastOrder 값을 변경 (HHmmss)
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler(timer_Tick);
            dispatcherTimer.Start();
        }

        public void stopTimer()
        {
            dispatcherTimer.Stop();
            // DB 접근 후 특정 지점 possible 값을 true로 변경
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            if (tik > 0) tik--;
            else stopTimer();
        }
    }
}
