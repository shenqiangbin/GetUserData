using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GetUserData.dao
{
    public class MySqlRepository
    {
        private static string ConnStr = ConfigurationManager.ConnectionStrings["MySqlStr"].ToString();

        public static int Execute(string sql, Array arr = null)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnStr))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    if (arr != null)
                        cmd.Parameters.AddRange(arr);

                    return cmd.ExecuteNonQuery();

                }
            }
        }
    }
}
