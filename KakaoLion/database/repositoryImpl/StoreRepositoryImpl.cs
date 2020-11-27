using KakaoLion.database.repository;
using KakaoLion.database.util;
using KakaoLion.model;
using KakaoLion.widget.extension;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace KakaoLion.database.repositoryImpl
{
    class StoreRepositoryImpl : StoreRepository
    {
        private Database db = new Database();

        private MySqlConnection conn;
        private MySqlCommand cmd;
        private MySqlDataReader rdr;

        public List<StoreModel> getAllStore()
        {
            conn = db.getConnection();
            List<StoreModel> storeList = new List<StoreModel>();
            using (conn)
            {
                string sql = "SELECT * FROM lion.shop";

                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    bool possible = int.Parse(rdr["possible"].ToString()) == 1;
                    storeList.Add(new StoreModel()
                    {
                        idx = (int)rdr["idx"],
                        name = (string)rdr["name"],
                        lastOrder = (string)rdr["lastOrder"],
                        possible = possible
                    });
                }
            }
            return storeList;
        }

        public void updateStoreLastOrder(int shopIdx)
        {
            conn = db.getConnection();
            using (conn) 
            {
                string lastOrder = DateTImeExtension.dateTimeFormat2(DateTime.Now);
                string sql = "UPDATE shop SET lastOrder=" + "'" + lastOrder + "', possible=" + false + " WHERE idx=" + shopIdx;

                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public void updateStorePossible(int shopIdx)
        {
            conn = db.getConnection();
            using (conn)
            {
                string sql = "UPDATE shop SET possible=" + true + " WHERE idx=" + shopIdx;

                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
