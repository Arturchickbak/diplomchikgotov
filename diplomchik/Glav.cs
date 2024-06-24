using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using System.Diagnostics;

namespace diplomchik
{
    public partial class Glav : Form
    {
        DB baza = new DB();
        public Client ClientInfo { get; set; }
        public int UserId { get; set; }

        public Glav()
        {
            InitializeComponent();
        }

        // двигать форму
        bool dragging = false;
        Point dragCursorPoint;
        Point dragFormPoint;

        // глобальные цвета. Активные кнопки
        private Color activeBackgroundColor = Color.FromArgb(52, 52, 52);
        private Color activeForegroundColor = Color.FromArgb(30, 144, 255);

        // глобальные цвета. Дефолтные цвета
        private Color defaultBackgroundColor = Color.FromArgb(46, 46, 50);
        private Color defaultForegroundColor = Color.FromArgb(200, 200, 200);

        private Form acriveForm = null;

        private void openForm(Form childForm, int userId)
        {
            if (acriveForm != null)
                acriveForm.Close();
            acriveForm = childForm;
            if (childForm is oplata)
            {
                ((oplata)childForm).UserId = userId;
            }
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel3.Controls.Add(childForm);
            panel3.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void SetButtonColors(IconButton button, Color backColor, Color foreColor)
        {
            // Цвета кнопки
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.IconColor = foreColor;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "коммунальные_услугиDataSet.tbl_login". При необходимости она может быть перемещена или удалена.
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            IconButton activeButton = (IconButton)sender;
            SetButtonColors(activeButton, activeBackgroundColor, activeForegroundColor);

            // Включаем левую подсветку
            Leftpanel1.Visible = true;

            // Сбрасываем цвета у остальных кнопок
            SetButtonColors(iconButton2, defaultBackgroundColor, defaultForegroundColor);
            SetButtonColors(iconButton3, defaultBackgroundColor, defaultForegroundColor);
            SetButtonColors(iconButton8, defaultBackgroundColor, defaultForegroundColor);

            // сброс подсветки
            Leftpanel2.Visible = false;
            Leftpanel3.Visible = false;
            с.Visible = false;
            leftpanel5.Visible = false;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            // открываем форму
            openForm(new tarif(), UserId);

            IconButton activeButton = (IconButton)sender;
            SetButtonColors(activeButton, activeBackgroundColor, activeForegroundColor);

            Leftpanel3.Visible = true;

            // Сбрасываем цвета у остальных кнопок
            SetButtonColors(iconButton2, defaultBackgroundColor, defaultForegroundColor);
            SetButtonColors(iconButton1, defaultBackgroundColor, defaultForegroundColor);
            SetButtonColors(iconButton8, defaultBackgroundColor, defaultForegroundColor);

            // сброс подсветки
            Leftpanel1.Visible = false;
            Leftpanel2.Visible = false;
            с.Visible = false;
            leftpanel5.Visible = false;
        }

        private void iconButton7_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void Leftpanel2_Click(object sender, EventArgs e)
        {
            IconButton activeButton = (IconButton)sender;

            SetButtonColors(activeButton, activeBackgroundColor, activeForegroundColor);

            Leftpanel2.Visible = true;

            // Сбрасываем цвета у остальных кнопок
            SetButtonColors(iconButton1, defaultBackgroundColor, defaultForegroundColor);
            SetButtonColors(iconButton3, defaultBackgroundColor, defaultForegroundColor);
            SetButtonColors(iconButton8, defaultBackgroundColor, defaultForegroundColor);

            // сброс подсветки
            Leftpanel1.Visible = false;
            Leftpanel3.Visible = false;
            с.Visible = false;
            leftpanel5.Visible = false;

            // открываем форму
            openForm(new Lc(), UserId);
            Lc form = new Lc();
            form.UserId = UserId; // Устанавливаем UserId
            openForm(form, UserId); // Передаем UserId в форму
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconButton6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void iconButton8_Click(object sender, EventArgs e)
        {
            avtor frm = new avtor();
            frm.Show();
        }

        private void iconButton8_Click_1(object sender, EventArgs e)
        {
            IconButton activeButton = (IconButton)sender;
            SetButtonColors(activeButton, activeBackgroundColor, activeForegroundColor);

            с.Visible = true;

            // Сбрасываем цвета у остальных кнопок
            SetButtonColors(iconButton1, defaultBackgroundColor, defaultForegroundColor);
            SetButtonColors(iconButton2, defaultBackgroundColor, defaultForegroundColor);
            SetButtonColors(iconButton3, defaultBackgroundColor, defaultForegroundColor);

            // сброс подсветки
            Leftpanel1.Visible = false;
            Leftpanel3.Visible = false;
            Leftpanel2.Visible = false;
            leftpanel5.Visible = false;

            // открываем форму
            openForm(new pokazanie(), UserId);
            pokazanie form = new pokazanie();
            form.UserId = UserId; // Устанавливаем UserId
            openForm(form, UserId); // Передаем UserId в форму
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            IconButton activeButton = (IconButton)sender;

            SetButtonColors(activeButton, activeBackgroundColor, activeForegroundColor);

            leftpanel5.Visible = true;

            // Сбрасываем цвета у остальных кнопок
            SetButtonColors(iconButton1, defaultBackgroundColor, defaultForegroundColor);
            SetButtonColors(iconButton3, defaultBackgroundColor, defaultForegroundColor);
            SetButtonColors(iconButton8, defaultBackgroundColor, defaultForegroundColor);
            SetButtonColors(iconButton2, defaultBackgroundColor, defaultForegroundColor);

            // сброс подсветки
            Leftpanel1.Visible = false;
            Leftpanel3.Visible = false;
            Leftpanel2.Visible = false;
            с.Visible = false;

            // открываем форму
            oplata form = new oplata();
            form.UserId = UserId; // Устанавливаем UserId
            openForm(form, UserId); // Передаем UserId в форму
        }
    }
}
