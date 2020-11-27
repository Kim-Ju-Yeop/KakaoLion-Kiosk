using KakaoLion.database.repository;
using KakaoLion.database.util;
using MySql.Data.MySqlClient;
using System;

namespace KakaoLion.database.repositoryImpl
{
    class ProgramRepositoryImpl : ProgramRepository
    {
        private Database db = new Database();

        private MySqlConnection conn;
        private MySqlCommand cmd;
        private MySqlDataReader rdr;

        public DateTime getOperationTime()
        {
            conn = db.getConnection();
            DateTime dateTIme = new DateTime();
            using (conn)
            {
                string sql = "SELECT * FROM lion.program";

                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string operationTime = (string)rdr["operationTime"];

                    int year = int.Parse(operationTime.Split(' ')[0]);
                    int month = int.Parse(operationTime.Split(' ')[1]);
                    int day = int.Parse(operationTime.Split(' ')[2]);

                    int hour = int.Parse(operationTime.Split(' ')[3]);
                    int minute = int.Parse(operationTime.Split(' ')[4]);
                    int second = int.Parse(operationTime.Split(' ')[5]);

                    dateTIme = new DateTime(year, month, day, hour, minute, second);
                }
            }
            return dateTIme;
        }

        public void updateOperationTime(string operationTime)
        {
            conn = db.getConnection();
            using (conn)
            {
                string sql = "UPDATE program SET operationTime = '" + operationTime + "'";

                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
