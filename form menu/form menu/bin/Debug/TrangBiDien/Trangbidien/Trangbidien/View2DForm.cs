using System;
using System.Windows.Forms;

namespace Trangbidien
{
    public partial class View2DForm : Form
    {

        public View2DForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog()
            {
                DefaultExt = ".mp4",
                Filter = @"Video Files (*.mp4, *.avi,*.3gp,*.mov,*.mp4v)|*.mp4; *.avi;*.3gp;*.mov;*.mp4v"
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


    }
}
