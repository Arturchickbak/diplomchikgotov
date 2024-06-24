using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using Document = Microsoft.Office.Interop.Word.Document;

namespace diplomchik
{
    public partial class pokazanie : Form
    {
        public Client ClientInfo { get; set; }
        public int UserId { get; set; }
        private Dictionary<string, double> tariffRates = new Dictionary<string, double>();

        // Обновленная строка подключения для SQL Server
        private string connectionString = "Data Source=DESKTOP-J06U7HI;Initial Catalog=Коммунальные услуги;Integrated Security=True;";

        public pokazanie()
        {
            InitializeComponent();
            InitializeTariffRates();
        }

        private void InitializeTariffRates()
        {
            // Добавьте тарифные ставки для разных видов услуг
            tariffRates.Add("Г.Вода", 12);
            tariffRates.Add("Х.Вода", 9);
            tariffRates.Add("Газ", 4);
            tariffRates.Add("Электричество", 7);
            // и т.д.
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string selectedService = guna2ComboBox1.SelectedItem?.ToString();
            if (selectedService != null && tariffRates.ContainsKey(selectedService))
            {
                try
                {
                    // Получение начальных и конечных показаний от пользователя
                    int startReading = Convert.ToInt32(guna2TextBox1.Text);
                    int endReading = Convert.ToInt32(guna2TextBox2.Text);

                    // Вычисление разницы между начальными и конечными показаниями счетчика
                    int consumption = endReading - startReading;

                    // Получение тарифной ставки для выбранного вида услуги
                    double tariffRate = tariffRates[selectedService];

                    // Расчет суммы к оплате
                    double totalPayment = consumption * tariffRate;

                    // Отображение суммы к оплате в label5
                    label5.Text = $"Сумма к оплате: {totalPayment} рублей";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        // Получение текущего баланса пользователя
                        string sqlGetBalance = "SELECT Baalance FROM client WHERE cliet_id = @UserId";
                        decimal currentBalance;
                        using (SqlCommand cmdGetBalance = new SqlCommand(sqlGetBalance, conn))
                        {
                            cmdGetBalance.Parameters.AddWithValue("@UserId", UserId);
                            currentBalance = (decimal)cmdGetBalance.ExecuteScalar();
                        }

                        // Проверка наличия достаточного баланса для оплаты
                        if (currentBalance >= (decimal)totalPayment)
                        {
                            // Обновление баланса пользователя в базе данных
                            string sqlUpdateBalance = "UPDATE client SET Baalance = Baalance - @TotalPayment WHERE cliet_id = @UserId";
                            using (SqlCommand cmdUpdateBalance = new SqlCommand(sqlUpdateBalance, conn))
                            {
                                cmdUpdateBalance.Parameters.AddWithValue("@TotalPayment", (decimal)totalPayment);
                                cmdUpdateBalance.Parameters.AddWithValue("@UserId", UserId);
                                cmdUpdateBalance.ExecuteNonQuery();
                            }

                            // Сохранение текущих показаний в базу данных
                            string sqlSaveReading = "INSERT INTO MeterReadings (UserId, ServiceType, StartReading, EndReading, ReadingDate) VALUES (@UserId, @ServiceType, @StartReading, @EndReading, @ReadingDate)";
                            using (SqlCommand cmdSaveReading = new SqlCommand(sqlSaveReading, conn))
                            {
                                cmdSaveReading.Parameters.AddWithValue("@UserId", UserId);
                                cmdSaveReading.Parameters.AddWithValue("@ServiceType", selectedService);
                                cmdSaveReading.Parameters.AddWithValue("@StartReading", startReading);
                                cmdSaveReading.Parameters.AddWithValue("@EndReading", endReading);
                                cmdSaveReading.Parameters.AddWithValue("@ReadingDate", DateTime.Now);
                                cmdSaveReading.ExecuteNonQuery();
                            }

                            // Обновление отображаемого баланса на форме
                            label4.Text = $"{currentBalance - (decimal)totalPayment} рублей";

                            // Обновление предыдущих показаний
                            guna2TextBox1.Text = endReading.ToString();

                            // Вывод результата пользователю
                            MessageBox.Show($"К оплате за {selectedService}: {totalPayment} рублей. Баланс обновлен.");
                        }
                        else
                        {
                            MessageBox.Show("Недостаточно средств на балансе для оплаты.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при расчете оплаты: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите вид услуги и укажите корректные показания счетчика.");
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string inputFilePath = $@"{System.Windows.Forms.Application.StartupPath}\chek\Chek.docx";
            string outputFilePath = $@"{System.Windows.Forms.Application.StartupPath}\chek\receipt {DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss")}.pdf";

            // Получение данных из TextBox и ComboBox
            string startReading = guna2TextBox1.Text;
            string endReading = guna2TextBox2.Text;
            string selectedService = guna2ComboBox1.SelectedItem?.ToString() ?? "Не выбрано";

            var replacements = new Dictionary<string, string>
            {
                { "<Num>", "465837" },
                { "<Cashier>", "Eва Эльфи" },
                { "<InfoServicel>", "Молоко домашнее" },  { "<quantityl>", "2шт." },  { "<Costl>", "150руб." },
                { "<InfoService2>", "Колбаса варенка" },  { "<Quantity2>", "3шт." },  { "<Cost2>", "550руб." },
                { "<InfoService3>", "Мука" },              { "<Quantity3>", "1шт." },  { "<Cost3>", "90руб." },
                { "<NumFD>", "77237" },  {  "<NumFP>", "37558" },
                { "<NumCash>", "12" },  { "<NumComing>", "П-30-61" },
                { "<NDS>", "18" },
                { "<DateTime>", $"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}" },
                { "<Sum>", "800" },
                { "<StartReading>", startReading },
                { "<EndReading>", endReading },
                { "<SelectedService>", selectedService }
            };

            if (ReplaceTags(inputFilePath, outputFilePath, replacements))
            {
                MessageBox.Show("Чек успешно сформирован.", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static bool ReplaceTags(string inputFilePath, string outputFilePath, Dictionary<string, string> replacements)
        {
            Application wordApp = new Application();

            try
            {
                Document doc = wordApp.Documents.Open(inputFilePath, ReadOnly: true);

                Range range = doc.Content;
                Document newDoc = wordApp.Documents.Add();
                range.Copy();
                newDoc.Range().Paste();

                foreach (var replacement in replacements)
                {
                    newDoc.Content.Find.Execute(FindText: replacement.Key, ReplaceWith: replacement.Value, Replace: WdReplace.wdReplaceAll);
                }

                newDoc.SaveAs2(outputFilePath, WdSaveFormat.wdFormatPDF);
                newDoc.Close(SaveChanges: false);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при формировании чека: " + ex.Message, "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            finally
            {
                wordApp.Quit();
            }
        }

        private void pokazanie_Load(object sender, EventArgs e)
        {
            LoadBalance();
            LoadPreviousReading();
        }

        private void LoadBalance()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT Baalance FROM client WHERE cliet_id = @UserId";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            decimal balance = reader.GetDecimal(0);
                            label4.Text = $"{balance} рублей";
                        }
                        else
                        {
                            label4.Text = "Баланс не найден.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке баланса: " + ex.Message);
            }
        }

        private void LoadPreviousReading()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT TOP 1 EndReading FROM MeterReadings WHERE UserId = @UserId ORDER BY ReadingDate DESC";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            int previousReading = reader.GetInt32(0);
                            guna2TextBox1.Text = previousReading.ToString();
                        }
                        else
                        {
                            guna2TextBox1.Text = "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке предыдущих показаний: " + ex.Message);
            }
        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
