using KakaoLion.widget;
using MySql.Data.MySqlClient;

namespace KakaoLion.database.util
{
    class Database
    {
        MySqlConnection conn;

        public Database()
        {
            conn = new MySqlConnection(Constants.DATABASE_CONNSTR);
        }

        public MySqlConnection getConnection()
        {
            conn.Open();
            return conn;
        }
    }
}
