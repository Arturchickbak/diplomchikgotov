using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace diplomchik
{
    public partial class oplata : Form
    {
        private string connectionString = "Data Source=DESKTOP-J06U7HI;Initial Catalog=Коммунальные услуги;Integrated Security=True;";
        public Client ClientInfo { get; set; }
        public int UserId { get; set; }
        private Dictionary<string, Image> bankImages = new Dictionary<string, Image>
        {
            { "1234", Properties.Resources.Visa },
            { "5678", Properties.Resources.mir },
            { "5106", Properties.Resources.mastercard },
        };

        public oplata()
        {
            InitializeComponent();
            textBoxCardNumber.TextChanged += TextBoxCardNumber_TextChanged;
        }

        private void TextBoxCardNumber_TextChanged(object sender, EventArgs e)
        {
            string cardNumber = textBoxCardNumber.Text;

            if (cardNumber.Length >= 4)
            {
                string firstFourDigits = cardNumber.Substring(0, 4);

                if (bankImages.TryGetValue(firstFourDigits, out Image bankImage))
                {
                    pictureBoxBank.Image = bankImage;
                }
                else
                {
                    pictureBoxBank.Image = null;
                }
            }
            else
            {
                pictureBoxBank.Image = null;
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string cardNumber = textBoxCardNumber.Text;
            string monthText = guna2TextBox2.Text;
            string yearText = guna2TextBox3.Text;
            string balanceText = guna2TextBox4.Text;
            string moneyText = guna2TextBox4.Text;
            string cvvText = guna2TextBox1.Text;

            if (!IsValidCardNumber(cardNumber))
            {
                MessageBox.Show("Неправильный номер карты. Пожалуйста, введите корректный номер карты из 16 цифр.");
                return;
            }

            if (!IsValidMonth(monthText, out int month))
            {
                MessageBox.Show("Неправильный месяц истечения. Пожалуйста, введите корректный месяц (от 1 до 12).");
                return;
            }

            if (!IsValidYear(yearText, out int year))
            {
                MessageBox.Show("Неправильный год истечения. Пожалуйста, введите корректный будущий год.");
                return;
            }

            if (!int.TryParse(balanceText, out int balance))
            {
                MessageBox.Show("Неправильная сумма баланса. Пожалуйста, введите корректную сумму.");
                return;
            }

            if (!int.TryParse(moneyText, out int money))
            {
                MessageBox.Show("Неправильная сумма денег. Пожалуйста, введите корректную сумму.");
                return;
            }

            if (!IsValidCVV(cvvText))
            {
                MessageBox.Show("Неправильный CVV. Пожалуйста, введите корректный CVV из 3 цифр.");
                return;
            }

            if (SaveCardInfo(cardNumber, month, year, balance, money, cvvText))
            {
                MessageBox.Show("Информация о карте и клиенте успешно сохранена.");
            }
            else
            {
                MessageBox.Show("Не удалось сохранить информацию о карте и клиенте.");
            }
        }

        private bool IsValidCardNumber(string cardNumber)
        {
            return cardNumber.Length == 16 && cardNumber.All(char.IsDigit);
        }

        private bool IsValidMonth(string monthText, out int month)
        {
            return int.TryParse(monthText, out month) && month >= 1 && month <= 12;
        }

        private bool IsValidYear(string yearText, out int year)
        {
            return int.TryParse(yearText, out year) && year >= DateTime.Now.Year;
        }

        private bool IsValidCVV(string cvvText)
        {
            return cvvText.Length == 3 && cvvText.All(char.IsDigit);
        }

        private bool SaveCardInfo(string cardNumber, int month, int year, int balance, int money, string cvv)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        string sqlCards = "INSERT INTO Cards (CardNumber, ExpiryMonth, ExpiryYear, Balance, CVV) VALUES (@CardNumber, @ExpiryMonth, @ExpiryYear, @Balance, @CVV)";
                        using (SqlCommand cmdCards = new SqlCommand(sqlCards, conn, transaction))
                        {
                            cmdCards.Parameters.AddWithValue("@CardNumber", cardNumber);
                            cmdCards.Parameters.AddWithValue("@ExpiryMonth", month);
                            cmdCards.Parameters.AddWithValue("@ExpiryYear", year);
                            cmdCards.Parameters.AddWithValue("@Balance", balance);
                            cmdCards.Parameters.AddWithValue("@CVV", cvv);
                            cmdCards.ExecuteNonQuery();
                        }

                        string sqlClient = "UPDATE client SET Baalance = Baalance + @Money WHERE cliet_id = @UserId";
                        using (SqlCommand cmdClient = new SqlCommand(sqlClient, conn, transaction))
                        {
                            cmdClient.Parameters.AddWithValue("@Money", money);
                            cmdClient.Parameters.AddWithValue("@UserId", UserId);
                            cmdClient.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
                return false;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Ваш код для другой кнопки
        }

        private void textBoxCardNumber_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
