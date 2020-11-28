using KakaoLion.database.repository;
using KakaoLion.database.util;
using KakaoLion.dto.model;
using KakaoLion.model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace KakaoLion.database.repositoryImpl
{
    class MenuRepositoryImpl : MenuRepository
    {
        private Database db = new Database();

        private MySqlConnection conn;
        private MySqlCommand cmd;
        private MySqlDataReader rdr;

        public List<MenuModel> getAllMenu()
        {
            conn = db.getConnection();
            List<MenuModel> menuList = new List<MenuModel>();
            using (conn)
            {
                string sql = "SELECT * FROM lion.menu";

                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string imagePath = "";
                    bool stock = int.Parse(rdr["stock"].ToString()) == 1;

                    switch ((Category)rdr["category"])
                    {
                        case Category.SMALL:
                            imagePath = "/resources/image/small/" + rdr["name"] + ".jpg";
                            break;
                        case Category.MEDIUM:
                            imagePath = "/resources/image/medium/" + rdr["name"] + ".jpg";
                            break;
                        case Category.BIG:
                            imagePath = "/resources/image/big/" + rdr["name"] + ".jpg";
                            break;
                    }

                    menuList.Add(new MenuModel
                    {
                        idx = (int)rdr["idx"],
                        page = (int)rdr["page"],
                        category = (Category)rdr["category"],
                        name = (string)rdr["name"],
                        price = (int)rdr["price"],
                        discount = (int)rdr["discount"],
                        stock = stock,
                        imagePath = imagePath
                    });
                }
            }
            return menuList;
        }

        public List<MenuModel> getAllDisocuntMenu()
        {
            conn = db.getConnection();
            List<MenuModel> menuList = new List<MenuModel>();
            using (conn)
            {
                string sql = "SELECT * FROM lion.menu";

                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string imagePath = "";
                    int discount = (int)rdr["discount"];
                    int price = (int)rdr["price"];
                    bool stock = int.Parse(rdr["stock"].ToString()) == 1;

                    if (discount > 0)
                    {
                        price -= (price * discount / 100);
                    }

                    switch ((Category)rdr["category"])
                    {
                        case Category.SMALL:
                            imagePath = "/resources/image/small/" + rdr["name"] + ".jpg";
                            break;
                        case Category.MEDIUM:
                            imagePath = "/resources/image/medium/" + rdr["name"] + ".jpg";
                            break;
                        case Category.BIG:
                            imagePath = "/resources/image/big/" + rdr["name"] + ".jpg";
                            break;
                    }

                    menuList.Add(new MenuModel
                    {
                        idx = (int)rdr["idx"],
                        page = (int)rdr["page"],
                        category = (Category)rdr["category"],
                        name = (string)rdr["name"],
                        price = price,
                        discount = discount,
                        stock = stock,
                        imagePath = imagePath
                    });
                }
            }
            return menuList;
        }

        public void updateMenu(int discount, int stock, int menuIdx)
        {
            conn = db.getConnection();
            using (conn)
            {
                string sql = "UPDATE menu SET discount=" + discount + ", stock=" + stock + " WHERE idx=" + menuIdx;

                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
