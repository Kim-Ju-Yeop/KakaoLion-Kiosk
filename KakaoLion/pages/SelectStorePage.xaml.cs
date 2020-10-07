using KakaoLion.model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace KakaoLion.pages
{
    public partial class SelectStorePage : Page
    {
        public SelectStorePage()
        {
            InitializeComponent();
            storeListBox.ItemsSource = storeList.ToList();
        }

        private List<StoreModel> storeList = new List<StoreModel>()
        {
            new StoreModel() { idx = 1, name = "동대구점", recentlyAt="12:23:14", possible = true },
            new StoreModel() { idx = 2, name = "성서점", recentlyAt="12:23:14", possible = false },
            new StoreModel() { idx = 3, name = "구지점", recentlyAt="12:23:14", possible = true },
            new StoreModel() { idx = 4, name = "대곡점", recentlyAt="12:23:14", possible = false },
            new StoreModel() { idx = 5, name = "계명대점", recentlyAt="12:23:14", possible = true },
            new StoreModel() { idx = 6, name = "구지점", recentlyAt="12:23:14", possible = false },
            new StoreModel() { idx = 7, name = "다사점", recentlyAt="12:23:14", possible = true },
            new StoreModel() { idx = 8, name = "신천점", recentlyAt="12:23:14", possible = false },
            new StoreModel() { idx = 9, name = "월성점", recentlyAt="12:23:14", possible = true }
        }; 
    }
}
