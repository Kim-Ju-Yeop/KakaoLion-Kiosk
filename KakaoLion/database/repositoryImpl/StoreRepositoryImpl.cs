using KakaoLion.database.repository;
using KakaoLion.database.util;
using KakaoLion.model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace KakaoLion.database.repositoryImpl
{
    class StoreRepositoryImpl : StoreRepository
    {
        private MySqlConnection conn;
        private MySqlCommand cmd;
        private MySqlDataReader rdr;

        public StoreRepositoryImpl()
        {
            Database db = new Database();
            conn = db.getConnection();
        }

        public List<StoreModel> getAllStore()
        {
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
    }
}
