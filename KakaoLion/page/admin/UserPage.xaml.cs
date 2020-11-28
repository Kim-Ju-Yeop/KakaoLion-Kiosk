using System.Linq;
using System.Windows.Controls;

namespace KakaoLion.pages.admin
{
    public partial class UserPage : Page
    { 
        public UserPage()
        {
            InitializeComponent();

            lvResult.ItemsSource = MainWindow.userList.ToList();
        }
    }
}
