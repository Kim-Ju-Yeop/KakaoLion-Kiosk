﻿using KakaoLion.model;
using System.Collections.Generic;
using System.Windows.Controls;

namespace KakaoLion.pages
{ 
    public partial class OrderPage : Page
    {
        private List<MenuModel> _menuList = new List<MenuModel>();
        public List<MenuModel> MenuList 
        {
            get => _menuList;
            set => _menuList = value;
        }

        public OrderPage()
        {
            InitializeComponent();
            Loaded += OrderPage_Loaded;
        }

        private void OrderPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DataContext = this;
            setMenuList();
        }

        public void setMenuList()
        {
            MenuModel menuModel1 = new MenuModel();
            menuModel1.idx = 0;
            menuModel1.category = 1;
            menuModel1.name = "라이언 치즈볼 쿠션 팩";
            menuModel1.price = 12000;
            menuModel1.discount = 0;
            menuModel1.imagePath = "/resources/image/small/라이언 치즈볼 쿠션 팩.jpg";

            MenuModel menuModel2 = new MenuModel();
            menuModel2.idx = 1;
            menuModel2.category = 1;
            menuModel2.name = "레몬 테라스 미니키체인 라이언";
            menuModel2.price = 12000;
            menuModel2.discount = 0;
            menuModel2.imagePath = "/resources/image/small/레몬 테라스 미니키체인 라이언.jpg";

            MenuModel menuModel3 = new MenuModel();
            menuModel3.idx = 2;
            menuModel3.category = 1;
            menuModel3.name = "리본 라이언";
            menuModel3.price = 12000;
            menuModel3.discount = 0;
            menuModel3.imagePath = "/resources/image/small/리본 라이언.jpg";

            MenuModel menuModel4 = new MenuModel();
            menuModel4.idx = 3;
            menuModel4.category = 1;
            menuModel4.name = "리틀 프렌즈 핑거퍼펫 세트 v1";
            menuModel4.price = 12000;
            menuModel4.discount = 0;
            menuModel4.imagePath = "/resources/image/small/리틀 프렌즈 핑거퍼펫 세트 v1.jpg";

            MenuModel menuModel5 = new MenuModel();
            menuModel5.idx = 4;
            menuModel5.category = 1;
            menuModel5.name = "마린 마스코트 키체인 라이언";
            menuModel5.price = 12000;
            menuModel5.discount = 0;
            menuModel5.imagePath = "/resources/image/small/마린 마스코트 키체인 라이언.jpg";

            MenuModel menuModel6 = new MenuModel();
            menuModel6.idx = 5;
            menuModel6.category = 1;
            menuModel6.name = "마린 페이스 키체인 라이언";
            menuModel6.price = 12000;
            menuModel6.discount = 0;
            menuModel6.imagePath = "/resources/image/small/마린 페이스 키체인 라이언.jpg";

            MenuModel menuModel7 = new MenuModel();
            menuModel7.idx = 6;
            menuModel7.category = 1;
            menuModel7.name = "미니 라이언 위드 아이앱 스튜디오";
            menuModel7.price = 12000;
            menuModel7.discount = 0;
            menuModel7.imagePath = "/resources/image/small/미니 라이언 위드 아이앱 스튜디오.jpg";

            MenuModel menuModel8 = new MenuModel();
            menuModel8.idx = 7;
            menuModel8.category = 1;
            menuModel8.name = "미니 인형 플라밍고 라이언";
            menuModel8.price = 12000;
            menuModel8.discount = 0;
            menuModel8.imagePath = "/resources/image/small/미니 인형 플라밍고 라이언.jpg";

            MenuModel menuModel9 = new MenuModel();
            menuModel9.idx = 8;
            menuModel9.category = 1;
            menuModel9.name = "미니페이스 리틀라이언";
            menuModel9.price = 12000;
            menuModel9.discount = 0;
            menuModel9.imagePath = "/resources/image/small/미니페이스 리틀라이언.jpg";

            MenuList.Add(menuModel1);
            MenuList.Add(menuModel2);
            MenuList.Add(menuModel3);
            MenuList.Add(menuModel4);
            MenuList.Add(menuModel5);
            MenuList.Add(menuModel6);
            MenuList.Add(menuModel7);
            MenuList.Add(menuModel8);
            MenuList.Add(menuModel9);
        }
    }
}