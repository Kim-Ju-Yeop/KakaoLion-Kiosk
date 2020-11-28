using KakaoLion.model;
using System.Collections.Generic;

namespace KakaoLion.database.repository
{
    interface MenuRepository
    {
        List<MenuModel> getAllMenu();

        List<MenuModel> getAllDisocuntMenu();

        void updateMenu(int discount, int stock, int menuIdx);
    }
}