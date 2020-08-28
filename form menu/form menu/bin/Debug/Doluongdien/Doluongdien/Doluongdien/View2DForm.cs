using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doluongdien
{
    public partial class View2DForm : Form
    {

        [DllImport("USER32.DLL")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("USER32.dll")]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);


        public View2DForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog()
            {
                DefaultExt = ".mp4",
                Filter = @"Video Files (*.mp4, *.avi,*.3gp,*.mov,*.mp4v)|*.mp4; *.avi;*.3gp;*.mov;*.mp4v|Flash Files (*.flash, *.swf)|*.flash; *swf"
            };

            if (openfile.ShowDialog() == DialogResult.OK)
            {

                string file = openfile.FileName;
                string outfile = Environment.CurrentDirectory + "/file.pdf";
                string extension = System.IO.Path.GetExtension(file);
                if (extension == ".mp4" || extension == "avi" || extension == "3gp" || extension == "mp4v" || extension == "mov")
                {
                    axWindowsMediaPlayer1.Visible = true;
                    axShockwaveFlash1.Visible = false;
                    axWindowsMediaPlayer1.URL = file;
                }
                if (extension == "swf" || extension == "flash")
                {
                    axWindowsMediaPlayer1.Visible = false;
                    axShockwaveFlash1.Visible = true;
                    axShockwaveFlash1.Movie = file;
                }
            }
        }

        public void SetAxShockWave(string url)
        {
            axShockwaveFlash1.Movie = url;
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {

        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {

        }

    }
}
