using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;

namespace diplomchik
{
    internal class DB
    {
        SqlConnection SqlConnection = new SqlConnection(@"Data Source=DESKTOP-J06U7HI;Initial Catalog=""Коммунальные услуги"";Integrated Security=True");

        public void OpenConnection()
        {

            if (SqlConnection.State == System.Data.ConnectionState.Closed)
            {
                SqlConnection.Open();
            }
        }
        public void CloseConnection()
        {
            if (SqlConnection.State == System.Data.ConnectionState.Open)
            {
                SqlConnection.Close();
            }
        }
        public SqlConnection GetSqlConnection()
        { return SqlConnection;
        }
        

    }

}




