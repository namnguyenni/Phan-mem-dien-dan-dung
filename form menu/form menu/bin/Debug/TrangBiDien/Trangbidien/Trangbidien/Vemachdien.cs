using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trangbidien
{
    public partial class Vemachdien : Form
    {

        [DllImport("USER32.DLL")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("USER32.dll")]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        public Vemachdien()
        {
            InitializeComponent();
        }

        private void Vemachdien_Load(object sender, EventArgs e)
        {
            Process process = Process.Start(Environment.CurrentDirectory + "\\CADe-SIMU PT-BR\\CADe_SIMU.exe");

            IntPtr ptr = IntPtr.Zero;

            while ((ptr = process.MainWindowHandle) == IntPtr.Zero) ;

            SetParent(process.MainWindowHandle, this.panel1.Handle);

            //MessageBox.Show(process.MainWindowHandle.ToString());

            MoveWindow(process.MainWindowHandle, 0, -30, panel1.Width + 20, panel1.Height + 20, true);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.StartInfo.CreateNoWindow = true;
        }

        private void Vemachdien_FormClosed(object sender, FormClosedEventArgs e)
        {           
            
        }
    }
}
