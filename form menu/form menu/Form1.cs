using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace form_menu
{
    public partial class Form1 : Form
    {
        int mov;
        int movX;
        int movY;

        private static string pathKhicudien = Environment.CurrentDirectory + @"\ElectricProject\ElectricProject\ElectricProject\bin\Debug\ElectricProject.exe";
        private static string pathTrangbidien = Environment.CurrentDirectory + @"\TrangBiDien\TrangBiDien\TrangBiDien\bin\Debug\TrangBiDien.exe";
        private static string pathKhinen = Environment.CurrentDirectory + @"\Khinen\Khinen\Khinen\bin\Debug\Khinen.exe";
        private static string pathDoluongdien = Environment.CurrentDirectory + @"\Doluongdien\Doluongdien\Doluongdien\bin\Debug\Doluongdien.exe";
        public Form1()
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

        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label1.Text = "VẼ MẠCH";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            label1.Text = "KHÍ NÉN";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = "ĐO LƯỜNG ĐIỆN";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "TRANG BỊ ĐIỆN";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "KHÍ CỤ ĐIỆN";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            label1.Text = "TRANG BỊ ĐIỆN";
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            label1.Text = "ĐO LƯỜNG ĐIỆN";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string text = label1.Text;
            switch (text)
            {
                case "TRANG BỊ ĐIỆN":
                    string filename1 = Path.Combine(pathTrangbidien);
                    var proc1 = System.Diagnostics.Process.Start(filename1, "tbd");
                    break;
                case "KHÍ CỤ ĐIỆN":
                    ProcessStartInfo startInfo = new ProcessStartInfo(pathKhicudien);
                    startInfo.Arguments = "kcd";
                    startInfo.UseShellExecute = false;
                    System.Diagnostics.Process.Start(startInfo);
                    break;
                case "ĐO LƯỜNG ĐIỆN":
                    string filename2 = Path.Combine(pathDoluongdien);
                    var proc2 = System.Diagnostics.Process.Start(filename2, "dld");
                    break;
                case "VẼ MẠCH":
                    new Vemachdien().Show();
                    break;
                case "KHÍ NÉN":
                    string filename4 = Path.Combine(pathKhinen);
                    var proc4 = System.Diagnostics.Process.Start(filename4, "kn");
                    break;


                default:
                    break;
            }
        }
    }
}
