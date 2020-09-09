using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ElectricProject
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

            MoveWindow(process.MainWindowHandle, 0, -28, panel1.Width + 10, panel1.Height + 37, true);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.StartInfo.CreateNoWindow = true;

        }
    }
}
