using Guna.UI2.WinForms;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace diplomchik
{
    public partial class Lc : Form
    {
        DB baza = new DB();

        public int UserId { get; set; }

        public Client Client { get; set; }

        public Lc()
        {
            InitializeComponent();
            // Подписываемся на событие клика кнопки
            guna2Button2.Click += guna2Button2_Click;

        }

        private void Lc_Load(object sender, EventArgs e)
        {
            // Загружаем информацию о клиенте при загрузке формы
            Client = new ClientRepos().GetUser(UserId);
            if (Client != null)
            {
                guna2TextBox1.Text = Client.name;
                guna2TextBox2.Text = Client.Familiy;
                guna2TextBox3.Text = Client.firstname;
                guna2TextBox4.Text = Client.date;
                guna2TextBox5.Text = Client.mobile;
                guna2TextBox6.Text = Client.lic_schet;
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {



            // Обновляем данные клиента из текстовых полей
            var name = guna2TextBox1.Text;
            var Familiy = guna2TextBox2.Text;
            var firstname = guna2TextBox3.Text;
            var date = guna2TextBox4.Text;
            var mobile = guna2TextBox5.Text;

            // Реализуем логику обновления данных клиента в базе данных

            baza.OpenConnection();


            SqlCommand command = new SqlCommand( "UPDATE client SET Familiy = @Familiy, name = @name, firstname = @firstname, date = @date, mobile = @mobile WHERE cliet_id = @UserId", baza.GetSqlConnection());
          
            
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@Familiy", Familiy);
                command.Parameters.AddWithValue("@firstname", firstname);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@mobile", mobile);
                command.Parameters.AddWithValue("@UserId", UserId);

               int result =  command.ExecuteNonQuery();
            if (result > 0) 
            { 
                MessageBox.Show("Данные успешно обновлены!");

            } 
            else
            {
                MessageBox.Show($"Ошибка при обновлении данных: ");
            }

                    

        }
    }
}












