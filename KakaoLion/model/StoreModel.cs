using Prism.Mvvm;
using System;
using System.Windows.Threading;

namespace KakaoLion.model
{
    public class StoreModel : BindableBase
    {
        public int idx { get; set; }

        public string name { get; set; }

        public string lastOrder { get; set; }

        private bool _possible;
        public bool possible
        {
            get { return _possible; }
            set
            {
                SetProperty(ref _possible, value);
                if (value == false)
                {
                    var now = String.Format("{0:HHmmss}", DateTime.Now);
                    var last = lastOrder;

                    int nowHour = int.Parse(now.Substring(0, 2));
                    int lastHour = int.Parse(last.Substring(0, 2));

                    int nowSeconds = int.Parse(now.Substring(2, 2)) * 60 + int.Parse(now.Substring(4));
                    int lastSeconds = int.Parse(last.Substring(2, 2)) * 60 + int.Parse(last.Substring(4));

                    if (nowHour == lastHour && nowSeconds > lastSeconds && nowSeconds - lastSeconds < 60)
                    {
                        tik = 60 - (nowSeconds - lastSeconds);
                        timer = (tik--).ToString();
                    } else
                    {
                        possible = true;
                    }
                }
            }
        }

        private bool timerCheck = false;
        private string _timer;
        public string timer
        {
            get { return _timer; }
            set
            {
                SetProperty(ref _timer, value);
                if (timerCheck == false)
                {
                    startTimer();
                    timerCheck = true;
                }
            }
        }

        public int tik = 0;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        private void startTimer()
        {
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler(timer_Tick);
            dispatcherTimer.Start();
        }
        private void stopTimer()
        {
            dispatcherTimer.Stop();
        }
        public void timer_Tick(object sender, EventArgs e)
        {
            if (tik > 0) timer = (tik--).ToString();
            else
            {
                stopTimer();
                timer = null;
                possible = true;
            }
        }
    }
}