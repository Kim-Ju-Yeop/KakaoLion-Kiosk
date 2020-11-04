using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KakaoLion.pages
{
    public partial class AdminPage : Page
    {
        public string time { get; set; }
        TimeSpan timeSpan = DateTime.Now - MainWindow.startOperationTime;

        public AdminPage()
        {
            InitializeComponent();
            setList();

            time = "가동시간 : " + timeSpan.Days + "일 " + timeSpan.Hours + "시간 " + timeSpan.Minutes + "분 " + timeSpan.Seconds + "초";
            this.DataContext = this;
        }

        public void setList()
        {
            List<string> categoryList = new List<string>();
            categoryList.Add("메뉴별 판매 수 및 총액");
            categoryList.Add("카테고리 별 판매 수 및 총액");
            categoryList.Add("지점별 메뉴별  판매수 및 총액");
            categoryList.Add("지점별 카테고리별 판매수 및 총액");
            categoryList.Add("일별 총 매출액");
            categoryList.Add("하루 중 시간대별 총 매출액");
            categoryList.Add("회원별 총 매출액 & 판매수 및 총액");
            categoryListBox.ItemsSource = categoryList.ToList();
        }
    }
}
