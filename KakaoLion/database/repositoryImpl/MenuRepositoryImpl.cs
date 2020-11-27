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
        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlDataReader rdr;

        public MenuRepositoryImpl()
        {
            Database db = new Database();
            conn = db.getConnection();
        }

        public List<MenuModel> getAllMenu()
        {
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
                        case Category.Small:
                            imagePath = "/resources/image/small/" + rdr["name"] + ".jpg";
                            break;
                        case Category.Medium:
                            imagePath = "/resources/image/medium/" + rdr["name"] + ".jpg";
                            break;
                        case Category.Big:
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
                        case Category.Small:
                            imagePath = "/resources/image/small/" + rdr["name"] + ".jpg";
                            break;
                        case Category.Medium:
                            imagePath = "/resources/image/medium/" + rdr["name"] + ".jpg";
                            break;
                        case Category.Big:
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
    }
}
