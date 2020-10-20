using KakaoLion.dto.model;
using KakaoLion.model;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KakaoLion.pages
{
    public partial class OrderPage : Page
    {
        public int pageCount = 1;
        public List<MenuModel> MenuList = new List<MenuModel>()
        {
            new MenuModel() {
                idx = 0,
                category = Category.Small,
                name="라이언 치즈볼 쿠션 팩",
                price = 12000,
                discount = 0,
                imagePath = "/resources/image/small/라이언 치즈볼 쿠션 팩.jpg"
            },
            new MenuModel() {
                idx = 1,
                category = Category.Small,
                name="레몬 테라스 미니키체인 라이언",
                price = 12000,
                discount = 0,
                imagePath = "/resources/image/small/레몬 테라스 미니키체인 라이언.jpg"
            },
            new MenuModel() {
                idx = 2,
                category = Category.Small,
                name="리본 라이언",
                price = 12000,
                discount = 0,
                imagePath = "/resources/image/small/리본 라이언.jpg"
            },
            new MenuModel() {
                idx = 3,
                category = Category.Small,
                name="리틀 프렌즈 핑거퍼펫 세트 v1",
                price = 12000,
                discount = 0,
                imagePath = "/resources/image/small/리틀 프렌즈 핑거퍼펫 세트 v1.jpg"
            },
            new MenuModel() {
                idx = 4,
                category = Category.Small,
                name="마린 마스코트 키체인 라이언",
                price = 12000,
                discount = 0,
                imagePath = "/resources/image/small/마린 마스코트 키체인 라이언.jpg"
            },
            new MenuModel() {
                idx = 5,
                category = Category.Small,
                name="리틀 프렌즈 핑거퍼펫 세트 v1",
                price = 12000,
                discount = 0,
                imagePath = "/resources/image/small/리틀 프렌즈 핑거퍼펫 세트 v1.jpg"
            },
            new MenuModel() {
                idx = 6,
                category = Category.Small,
                name="리틀 프렌즈 핑거퍼펫 세트 v1",
                price = 12000,
                discount = 0,
                imagePath = "/resources/image/small/리틀 프렌즈 핑거퍼펫 세트 v1.jpg"
            },
            new MenuModel() {
                idx = 7,
                category = Category.Small,
                name="리틀 프렌즈 핑거퍼펫 세트 v1",
                price = 12000,
                discount = 0,
                imagePath = "/resources/image/small/리틀 프렌즈 핑거퍼펫 세트 v1.jpg"
            },
             new MenuModel() {
                idx = 8,
                category = Category.Small,
                name="리틀 프렌즈 핑거퍼펫 세트 v1",
                price = 12000,
                discount = 0,
                imagePath = "/resources/image/small/리틀 프렌즈 핑거퍼펫 세트 v1.jpg"
            },
            new MenuModel() {
                idx = 9,
                category = Category.Small,
                name="리틀 프렌즈 핑거퍼펫 세트 v1",
                price = 12000,
                discount = 0,
                imagePath = "/resources/image/small/리틀 프렌즈 핑거퍼펫 세트 v1.jpg"
            },
            new MenuModel() {
                idx = 10,
                category = Category.Small,
                name="리틀 프렌즈 핑거퍼펫 세트 v1",
                price = 12000,
                discount = 0,
                imagePath = "/resources/image/small/리틀 프렌즈 핑거퍼펫 세트 v1.jpg"
            },
            new MenuModel() {
                idx = 11,
                category = Category.Small,
                name="리틀 프렌즈 핑거퍼펫 세트 v1",
                price = 12000,
                discount = 0,
                imagePath = "/resources/image/small/리틀 프렌즈 핑거퍼펫 세트 v1.jpg"
            },


            new MenuModel() {
                idx = 0,
                category = Category.Medium,
                name="25cm인형 치즈볼 라이언",
                price = 8000,
                discount = 0,
                imagePath = "/resources/image/medium/25cm인형 치즈볼 라이언.jpg"
            },
            new MenuModel() {
                idx = 1,
                category = Category.Medium,
                name="꿀잠 베이비 필로우 라이언",
                price = 8000,
                discount = 0,
                imagePath = "/resources/image/medium/꿀잠 베이비 필로우 라이언.jpg"
            },
            new MenuModel() {
                idx = 2,
                category = Category.Medium,
                name="레몬 테라스 얼굴 쿠션 라이언",
                price = 8000,
                discount = 0,
                imagePath = "/resources/image/medium/레몬 테라스 얼굴 쿠션 라이언.jpg"
            },
            new MenuModel() {
                idx = 3,
                category = Category.Medium,
                name="레몬 테라스 향기 인형 라이언",
                price = 8000,
                discount = 0,
                imagePath = "/resources/image/medium/레몬 테라스 향기 인형 라이언.jpg"
            },


            new MenuModel() {
                idx = 0,
                category = Category.Big,
                name="롱 바디 필로우 라이언",
                price = 5000,
                discount = 0,
                imagePath = "/resources/image/big/롱 바디 필로우 라이언.jpg"
            },
            new MenuModel() {
                idx = 1,
                category = Category.Big,
                name="리틀 바디 필로우 라이언 인형",
                price = 5000,
                discount = 0,
                imagePath = "/resources/image/big/리틀 바디 필로우 라이언 인형.jpg"
            },
            new MenuModel() {
                idx = 2,
                category = Category.Big,
                name="말랑 허그 바디 라이언 쿠션",
                price = 5000,
                discount = 0,
                imagePath = "/resources/image/big/말랑 허그 바디 라이언 쿠션.jpg"
            },
            new MenuModel() {
                idx = 3,
                category = Category.Big,
                name="메가 바디 필로우 리틀 라이언",
                price = 5000,
                discount = 0,
                imagePath = "/resources/image/big/메가 바디 필로우 리틀 라이언.jpg"
            }

        };
        public List<OrderModel> orderList = new List<OrderModel>();

        public OrderPage()
        {
            InitializeComponent();
            previous.IsEnabled = false;
            Category category = (Category)lbCategory.SelectedIndex;
            for (int i = 0; i < MenuList.Count; i++)
            {
                if (MenuList[i].idx < 9)
                {
                    MenuList[i].page = 1;
                }
                else if (MenuList[i].idx > 8 && MenuList[i].idx < 18)
                {
                    MenuList[i].page = 2;
                }
            }
            Loaded += OrderPage_Loaded;
        }
        private void OrderPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbCategory.SelectedIndex = 0;
        }
        public List<MenuModel> PageChange(int pageCount)
        {
            Category category = (Category)lbCategory.SelectedIndex;
            return (List<MenuModel>)MenuList.Where(x => x.page == pageCount && x.category == category).ToList();
        }
        private void previous_Click(object sender, RoutedEventArgs e)
        {

            --this.pageCount;
            lbMenus.ItemsSource = PageChange(this.pageCount);
            if (this.pageCount - 1 <= 0)
                previous.IsEnabled = false;
            next.IsEnabled = true;
        }
        private void next_Click(object sender, RoutedEventArgs e)
        {

            ++this.pageCount;
            lbMenus.ItemsSource = PageChange(this.pageCount);
            previous.IsEnabled = true;
            for (int i = 0; i < MenuList.Count; i++)
            {
                if (this.pageCount == MenuList[i].page)
                {
                    next.IsEnabled = false;
                }
            }

        }
        private void Up(object sender, RoutedEventArgs e)
        {

            if (lbMenus.SelectedItem != null)
            {
                var selectedMenu = lbMenus.SelectedItem as MenuModel;
                foreach (var temp in orderList)
                {
                    if (temp.menuindx == selectedMenu.idx)
                    {
                        temp.quantity++;
                        temp.totalPrice = selectedMenu.price * temp.quantity;
                    }
                }
                lvResult.ItemsSource = orderList.ToList();
            }
            
        }
        private void Down(object sender, RoutedEventArgs e)
        {
            if(lbMenus.SelectedItem != null)
            {
                var selectedMenu = lbMenus.SelectedItem as MenuModel;
                foreach (var temp in orderList)
                {
                    if (temp.menuindx == selectedMenu.idx)
                    {
                        if(temp.quantity > 0)
                        {
                            temp.quantity--;
                            temp.totalPrice = selectedMenu.price * temp.quantity;
                        }
                        
                    }
                }
                lvResult.ItemsSource = orderList.ToList();
            }
        }
        private void lbMenus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbMenus.SelectedItem != null)
            {
                MenuModel menu = (MenuModel)lbMenus.SelectedItem;
                OrderModel model = orderList.Where((order) => order.menuindx== menu.idx).FirstOrDefault();

                if (model == null)
                {
                    orderList.Add(new OrderModel() { idx = null, menuindx = menu.idx, quantity = 1, totalPrice = menu.price, userId = "user", purchaseAt = "2020-10-16 09:50:00", paymentPlace = true, paymentMethod = true, shopIdx = 1 });
                } else
                {
                    model.quantity++;
                }
                
                var selectedMenu = lbMenus.SelectedItem as MenuModel;
                lvResult.ItemsSource = orderList.ToList();
                foreach (var temp in orderList)
                {
                    if(temp.menuindx == selectedMenu.idx)
                    {
                        temp.totalPrice = selectedMenu.price * temp.quantity;
                    }
                }
            }
        }
        private void lbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.pageCount = 1;
            if (lbCategory.SelectedIndex == -1) return;
            Category category = (Category)lbCategory.SelectedIndex;
            lbMenus.ItemsSource = PageChange(this.pageCount);
        }

    }
}
