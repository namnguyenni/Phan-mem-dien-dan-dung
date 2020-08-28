using System;
using System.Windows.Forms;

namespace Khinen
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args )
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0)
            {
                if (args[0] == "kn")
                {
                    Application.Run(new Form1());
                }
                else
                {
                    Application.Run(new Info());
                }
            }
            else Application.Run(new Info());


        }
    }
}
