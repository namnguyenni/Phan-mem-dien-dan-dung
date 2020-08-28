using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace form_menu
{
    public partial class Lience : Form
    {
        int mov;
        int movX;
        int movY;
        public Lience()
        {
            InitializeComponent();
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            List<Quantrivien> listquantri = CreateQuantrivien();
            Quantrivien thongtindangnhap = new Quantrivien(textBox1.Text.ToString(), textBox2.Text.ToString());

            ReadWriter rd = new ReadWriter();
            if (textBox1.Text.ToString()== "nguyenvannam"&& textBox2.Text =="123456")
            {
                rd.SaveLienceKey();
                MessageBox.Show("MÁY TÍNH ĐÃ ĐƯỢC CẤP QUYỀN THÀNH CÔNG!!(=_+)");


            }
            else MessageBox.Show("THÔNG TIN TÀI KHOẢN HOẶC MẬT KHẨU KHÔNG CHÍNH XÁC");
        }

        public List<Quantrivien> CreateQuantrivien()
        {
            List<Quantrivien> quantri = new List<Quantrivien>();
            quantri.Add(new Quantrivien("nguyenvannam", "123456"));
            quantri.Add(new Quantrivien("lesytung", "tungdranix123"));
            quantri.Add(new Quantrivien("admin", "phanmemdiendandung123456"));

            return quantri;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.Focused == false)
            {
                textBox1.SelectAll();
            }
        }
    }
}

