using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace diplomchik
{
    public partial class avtor : Form
    {
        public avtor()
        {
            InitializeComponent();
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DB dataBases = new DB();

            var loginuser = guna2TextBox6.Text;
            var passUser = guna2TextBox5.Text;
            var schet = guna2TextBox4.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = "SELECT cliet_id, Login, Password, lic_schet FROM client WHERE Login = @loginuser AND Password = @passUser AND lic_schet = @schet";

            SqlCommand command = new SqlCommand(querystring, dataBases.GetSqlConnection());
            command.Parameters.AddWithValue("@loginuser", loginuser);
            command.Parameters.AddWithValue("@passUser", passUser);
            command.Parameters.AddWithValue("@schet", schet);
           

            adapter.SelectCommand = command;
            adapter.Fill(table); 

            if (table.Rows.Count == 1)
            {
          
                MessageBox.Show("Вход выполнен");
                Glav frm1 = new Glav();
                frm1.UserId = (int) table.Rows[0]["cliet_id"]; 
                frm1.ShowDialog();
                this.Show();
                this.Hide();


            }
            else
                MessageBox.Show("Неверный логин или пароль");

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DB dataBases = new DB();

            var login = guna2TextBox1.Text;
            var password = guna2TextBox2.Text;
            var schet = guna2TextBox3.Text;
            var balance = 0;

            // Use parameterized query to prevent SQL injection
            string querystring = "INSERT INTO client (Login, Password, lic_schet, Baalance) " +
                                 "OUTPUT INSERTED.cliet_id " +
                                 "VALUES (@login, @password, @schet, @balance)";

            SqlCommand command = new SqlCommand(querystring, dataBases.GetSqlConnection());
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@schet", schet);
            command.Parameters.AddWithValue("@balance", balance);

            dataBases.OpenConnection();

            try
            {
                var result = command.ExecuteScalar();
                if (result != null)
                {
                    int newClientId = (int)result;
                    MessageBox.Show("Account successfully created!");

                    Glav frmlgn = new Glav();
                    frmlgn.UserId = newClientId;
                    this.Hide();
                    frmlgn.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Account creation failed.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                dataBases.CloseConnection();
            }
        }

        private void guna2TextBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked)
            {
                guna2TextBox2.UseSystemPasswordChar = false;
            }
            else
            {
                guna2TextBox2.UseSystemPasswordChar = true;
            }
        }

        private void avtor_Load(object sender, EventArgs e)
        {

        }
    }
}

