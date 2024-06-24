using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace diplomchik
{
    public class Client
    {
        internal static object guna2TextBox2;

        public static int cliet_id { get; private set; }
        public  string Familiy { get; private set; }
        public  string name { get; private set; }
        public  string firstname { get; private set; }
        public  string date { get; private set; }
        public  string mobile { get; private set; }
        public  string lic_schet { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public int Baalance { get; private set; }

        public bool SetPersonalData(string login, string password)
        {
            Console.WriteLine(login);
            Console.WriteLine(password);

            var db = new DB();

            string sqlExpression = (@"Data Source=DESKTOP-J06U7HI;Initial Catalog=""Коммунальные услуги"";Integrated Security=True");

            string query = "SELECT * FROM client WHERE Login=@Login AND Password=@Password";

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlExpression))
                {
                    connection.Open();
                    Console.WriteLine("Connection opened successfully.");

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);
                        Console.WriteLine("Executing query...");

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                cliet_id = (int)reader["cliet_id"];
                                Login = reader["Login"].ToString();
                                Password = reader["Password"].ToString();
                                mobile = reader["mobile"].ToString();
                                Familiy = reader["Familiy"].ToString();
                                name = reader["name"].ToString();
                                firstname = reader["firstname"].ToString();
                                date = DateTime.Parse(reader["date"].ToString()).ToString("yyyy-MM-dd");
                                lic_schet = reader["lic_schet"].ToString();
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("No rows found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return false;

            
        }
    }
}
    


