using KakaoLion.database.repository;
using KakaoLion.database.util;
using KakaoLion.model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace KakaoLion.database.repositoryImpl
{
    class UserRepositoryImpl : UserRepository
    {
        private MySqlConnection conn;
        private MySqlCommand cmd;
        private MySqlDataReader rdr;

        public UserRepositoryImpl()
        {
            Database db = new Database();
            conn = db.getConnection();
        }

        public List<UserModel> getAllUser()
        {
            List<UserModel> userList = new List<UserModel>();
            using (conn) 
            {
                string sql = "SELECT * FROM lion.user";

                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    userList.Add(new UserModel
                    {
                        id = (string)rdr["id"],
                        pw = (string)rdr["pw"],
                        name = (string)rdr["name"],
                        address = (string)rdr["address"],
                        barcode = (string)rdr["barcode"],
                        qrcode = (string)rdr["qrcode"]
                    });
                }
            }
            return userList;
        }
    }
}
