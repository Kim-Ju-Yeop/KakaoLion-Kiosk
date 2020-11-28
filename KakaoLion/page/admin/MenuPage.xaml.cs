using KakaoLion.model;
using System.Linq;
using System.Windows.Controls;

namespace KakaoLion.pages.admin
{
    public partial class MenuPage : Page
    {
        public MenuWindow menuWindow;

        public MenuPage()
        {
            InitializeComponent();

            lvResult.ItemsSource = MainWindow.menuList.ToList();
        }

        private void lvResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var menu = (MenuModel)lvResult.SelectedItem;

            if (menu != null)
            {
                if (menuWindow != null)
                {
                    menuWindow.Close();
                }
                menuWindow = new MenuWindow(menu);
                menuWindow.Show();

                lvResult.SelectedItem = null;
            }
        }
    }
}
