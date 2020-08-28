using System;
using System.IO;
using System.Windows.Forms;

namespace form_menu
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ReadWriter rd = new ReadWriter();

            //
            if (rd.CheckedLIENCEKEY())
            {                
                Application.Run(new Form1());
            }
            else Application.Run(new Lience());

        }
    }
}
