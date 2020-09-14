
using MindFusion.Diagramming;
using MindFusion.Diagramming.Fluent;
using MindFusion.Diagramming.Lanes;
using System;
using System.Collections.Generic;
using MindFusion.Diagramming.WinForms;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace ElectricProject
{           
    public partial class Form1 : Form
    {
        int mov;        
        int movX;
        int movY;
        public Form1()
        {
            InitializeComponent();
            btn_Restoredown.Visible = false;
            panel_Menu.Visible = false;
            panel_Simulate.Visible = false;
            panel_Lineproperties.Visible = false;
         
        }

        public Form1(string arg)
        {
            InitializeComponent();
            btn_Restoredown.Visible = false;
            panel_Menu.Visible = false;
            panel_Simulate.Visible = false;
            panel_Lineproperties.Visible = false;

                try
                {
                    diagram1.LoadFromFile(arg);

                    this.tb_NameDocument.Text = "Name : ";
                    //this.tb_NameDocument.Text += openFileDialog1.FileName;


                    if (panel_Thuchanh.Visible == true)
                    {
                        InvisibleMenu();
                        return;
                    }
                    else
                    {

                        InvisibleWork();
                        InvisibleMenu();
                        panel_Menu.Visible = true;
                        panel_Thuchanh.Visible = true;
                        panel_workthuchanh.Visible = true;
                        panel_Lineproperties.Visible = true;
                        panel_Simulate.Visible = true;
                    }


                }
                catch
                {
                    MessageBox.Show(this, "Không mở được định dạng này!");
                }
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

        private const int CS_DropShadow = 0x00020000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = CS_DropShadow;
                return cp;

            }
        }
        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_Restoredown_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btn_Fullscreen.Visible = true;
            btn_Restoredown.Visible = false;
        }

        private void btn_Fullscreen_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btn_Restoredown.Visible = true;
            btn_Fullscreen.Visible = false;
        }

        private void btn_Minimized_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.WindowState = FormWindowState.Normal;

            this.workpanel.Controls.Add(this.panel_work2d);        
            this.workpanel.Controls.Add(this.panel_work3d);
            this.workpanel.Controls.Add(this.panel_worklythuyet);
            this.workpanel.Controls.Add(this.panel_workthuchanh);
            this.workpanel.Controls.Add(this.panel_work_baigiang);
            this.panel_workthuchanh.Dock = DockStyle.Fill;
            this.panel_worklythuyet.Dock = DockStyle.Fill;
            this.panel_work3d.Dock = DockStyle.Fill;
            this.panel_work2d.Dock = DockStyle.Fill;
            this.panel_work_baigiang.Dock = DockStyle.Fill;

            diagram1.UndoManager.UndoEnabled = true;
            diagram1.UndoManager.History.Capacity = 20;

            LoadTreeView();
            LoadDocument();
            LoadDocument3D();

        }

        private void btn_trangchu_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Form2().Show();
        }

        private void InvisibleBtnBaiGiang()
        {
            btn_XoaFile.Visible = false;
            btn_ThemFile.Visible = false;
            btn_MoFile.Visible = false;
        }

        private void InvisibleWork()
        {
            panel_work2d.Visible = false;
            panel_work3d.Visible = false;
            panel_worklythuyet.Visible = false;
            panel_workthuchanh.Visible = false;
            panel_Lineproperties.Visible = false ;
            panel_Simulate.Visible = false;
            panel_work_baigiang.Visible = false;

            
        }

        private void InvisibleMenu()
        {
            panel_Menu.Visible = false;
            panel_2D.Visible = false;
            panel_3D.Visible = false;
            panel_lythuyet.Visible = false;
            panel_Thuchanh.Visible = false;
            panel_BaiGiang.Visible = false;
            //displaypanel.Visible = true;
            panel_Lineproperties.Visible = false;



        }

        private void btb_Simulate_Click(object sender, EventArgs e)
        {
            if (panel_Thuchanh.Visible == true)
            {
                InvisibleMenu();
                return;
            }
            else
            {

                InvisibleWork();
                InvisibleMenu();
                panel_Menu.Visible = true;
                panel_Thuchanh.Visible = true;
                panel_workthuchanh.Visible = true;
                panel_Lineproperties.Visible = true;
                panel_Simulate.Visible = true;
            }


        }

        PDFViewerForm pdfForm = new PDFViewerForm();
        private void btn_Document_Click(object sender, EventArgs e)
        {
            
            if (panel_lythuyet.Visible == true)
            {
                InvisibleMenu();
                return;
            }
            else
            {
                
                InvisibleWork();
                InvisibleMenu();
                panel_Menu.Visible = true;
                panel_lythuyet.Visible = true;
                panel_worklythuyet.Visible = true;

            }


            //fill form

            pdfForm.TopLevel = false;
            panel_worklythuyet.Controls.Add(pdfForm);
            pdfForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            pdfForm.Dock = DockStyle.Fill;
            pdfForm.Show();

        }
        private void LoadDocument3D()
        {
            string txtDirectoryPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Thư viện mô hình 3D";
            treeView2.Nodes.Clear();
            toolTip1.ShowAlways = true;
            if (txtDirectoryPath != "" && Directory.Exists(txtDirectoryPath))
                LoadDirectory(txtDirectoryPath,treeView2);
            else
                MessageBox.Show("Select Directory!!");
        }

        private void LoadDocument()
        {
            string txtDirectoryPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Thư viện lí thuyết điện";
            treeView1.Nodes.Clear();
            toolTip1.ShowAlways = true;
            if (txtDirectoryPath != "" && Directory.Exists(txtDirectoryPath))
                LoadDirectory(txtDirectoryPath,treeView1);
            else
                MessageBox.Show("Select Directory!!");
        }

        public void LoadDirectory(string Dir,TreeView tree)
        {
            DirectoryInfo di = new DirectoryInfo(Dir);
            TreeNode tds = tree.Nodes.Add(di.Name);
            tds.Tag = di.FullName;
            tds.StateImageIndex = 0;
            tds.SelectedImageIndex = 0;
            LoadFiles(Dir, tds);
            LoadSubDirectories(Dir, tds);
        }

        private void LoadSubDirectories(string dir, TreeNode td)
        {
            // Get all subdirectories  
            string[] subdirectoryEntries = Directory.GetDirectories(dir);
            // Loop through them to see if they have any other subdirectories  
            foreach (string subdirectory in subdirectoryEntries)
            {

                DirectoryInfo di = new DirectoryInfo(subdirectory);
                TreeNode tds = td.Nodes.Add(di.Name);
                tds.StateImageIndex = 0;
                tds.SelectedImageIndex = 0;
                tds.Tag = di.FullName;
                LoadFiles(subdirectory, tds);
                LoadSubDirectories(subdirectory, tds);
                //UpdateProgress();

            }
        }

        private void LoadFiles(string dir, TreeNode td)
        {
            string[] Files = Directory.GetFiles(dir, "*.*");

            // Loop through them to see files  
            foreach (string file in Files)
            {
                FileInfo fi = new FileInfo(file);
                TreeNode tds = td.Nodes.Add(fi.Name);
                tds.Tag = fi.FullName;
                tds.ImageIndex = 5;
                tds.SelectedImageIndex = 5;
                //UpdateProgress();

            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Uri file = new Uri(e.Node.Tag.ToString());
            if (file.IsFile)
            {
                pdfForm.OpenOffice(e.Node.Tag.ToString());
                
            }

        }

        private void treeView2_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Uri file = new Uri(e.Node.Tag.ToString());
            if (file.IsFile)
            {
                form3D.OpenOffice(e.Node.Tag.ToString());
                form3D.BackColor = Color.White;
                
            }

        }

        private void btn_2D_Click(object sender, EventArgs e)
        {
             if (panel_2D.Visible == true)
            {
                InvisibleMenu();
                return;
            }
            else
            {
                
                InvisibleWork();
                InvisibleMenu();
                panel_Menu.Visible = true;
                panel_2D.Visible = true;
                panel_work2d.Visible = true;
            }

            //fill form
            View2DForm objForm = new View2DForm();
            objForm.TopLevel = false;
            panel_work2d.Controls.Add(objForm);
            objForm.FormBorderStyle = FormBorderStyle.None;
            objForm.Dock = DockStyle.Fill;
            objForm.Show();

        }


        PDFViewerForm form3D = new PDFViewerForm();
        private void btn_3D_Click(object sender, EventArgs e)
        {
            if (panel_3D.Visible == true)
            {
                InvisibleMenu();
                return;
            }
            else
            {

                InvisibleWork();
                InvisibleMenu();
                panel_Menu.Visible = true;
                panel_3D.Visible = true;
                panel_work3d.Visible = true;
            }


            form3D.TopLevel = false;
            panel_work3d.Controls.Add(form3D);
            form3D.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form3D.Dock = DockStyle.Fill;
            form3D.Show();
        }

        private void btn_BaiGiang_Click(object sender, EventArgs e)
        {
            if (panel_BaiGiang.Visible ==true)
            {
                InvisibleMenu();
                return;
            }
            else
            {

                InvisibleWork();
                InvisibleMenu();
                panel_Menu.Visible = true;
                panel_BaiGiang.Visible = true;
                panel_work_baigiang.Visible = true;
                InvisibleBtnBaiGiang();
                return;
            }
        }

        private void btn_Option_Click(object sender, EventArgs e)
        {

        }

    #region Cac cong cu 3d

        private void Display3d(int DeviceIndex)
        {

            switch (DeviceIndex)
            {
                case 1:
                    {
                        
                        break;
                    }
                case 2:
                    {
                        
                        break;
                    }
                case 3:
                    {
                        break;
                    }
                case 4:
                    {
                        
                        break;
                    }
                case 5:
                    {
                        
                        break;
                    }
                case 6:
                    {
                        
                        break;
                    }
                case 7:
                    {
                      
                        break;
                    }
                case 8:
                    {
                       
                        break;
                    }
                case 9:
                    {
                       
                        break;
                    }
                case 10:
                    {
                        
                        break;
                    }

                default:
                    break;
            }
        }

        private void button53_Click(object sender, EventArgs e)
        {

        }

        private void button71_Click(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region thuc hanh
        #region thuc hanh
        public int countDevice = 0;

        private void button7_Click(object sender, EventArgs e)
        {
            Device device = new Device(2);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._1_CauDao1Pha;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.425f*device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.03f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.575f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X, 
                0.03f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.425f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                1.00f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.575f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                1.00f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Device device = new Device(3);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._2_CauDao1PhaDaoChieu;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.05f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.77f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.05f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.87f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.95f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.45f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.95f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.55f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
            //ShapeNode node = diagram1.Factory.CreateShapeNode(20, 20, 40, 40);
            //node.Id = "Cong tac to";
            //node.Image = Properties.Resources._2_CauDao1PhaDaoChieu;
            //node.Transparent = true;
            //node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Device device = new Device(6);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._3_CauDao3Pha;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 6;
            device.port.Add(new PointF(0.325f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.5f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.9125f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.325f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.5f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.9125f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            AllDevice.Add(device);
            //ShapeNode node = diagram1.Factory.CreateShapeNode(20, 20, 40, 40);
            //node.Id = "Role thoi gian";
            //node.Image = Properties.Resources._12_ro_le_thoi_gian;
            //node.Transparent = true;
            //node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Device device = new Device(5);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._4_CauDao3PhaDaoChieu;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.05f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.71f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.05f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.89f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.95f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.41f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.95f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.59f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
            //ShapeNode node = diagram1.Factory.CreateShapeNode(20, 20, 40, 40);
            //node.Id = "Role";
            //node.Image = Properties.Resources._;
            //node.Transparent = true;
            //node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Device device = new Device(8);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._5_Contactor;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 8;
            device.port.Add(new PointF(0.2f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.325f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.45f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.2f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.325f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                 0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.45f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.95f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.33f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.95f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.8125f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
            //ShapeNode node = diagram1.Factory.CreateShapeNode(20, 20, 40, 40);
            //node.Id = "Role nhiet";
            //node.Image = Properties.Resources._15_RoLE_NHIeT;
            //node.Transparent = true;
            //node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Device device = new Device(7);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._6_CongTac;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 2;
            device.port.Add(new PointF(0.125f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.9f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.85f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.9f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
            //    ShapeNode node = diagram1.Factory.CreateShapeNode(20, 20, 40, 40);
            //    node.Id = "Role bao ve qua dong";
            //    node.Image = Properties.Resources._16_role_bao_ve_qua_dong;
            //    node.Transparent = true;
            //    node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Device device = new Device(8);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 20, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._7_CongTacNgat;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 2;
            device.port.Add(new PointF(0.12f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.9f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.9f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.9f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
            //ShapeNode node = diagram1.Factory.CreateShapeNode(20, 20, 40, 40);
            //node.Id = "Role bao ve mat pha";
            //node.Image = Properties.Resources._17_role_bao_ve_mat_pha;
            //node.Transparent = true;
            //node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;
        }



        private void button27_Click(object sender, EventArgs e)
        {
            Device device = new Device(8);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 56);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._17_Aptomat1Pha;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));


            AllDevice.Add(device);
            //ShapeNode node = diagram1.Factory.CreateShapeNode(20, 20, 40, 40);
            //node.Id = "Role bao ve mat pha";
            //node.Image = Properties.Resources._17_role_bao_ve_mat_pha;
            //node.Transparent = true;
            //node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            Device device = new Device(8);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 56);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._18_Aptomat3PhaCongSuatLon;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 6;
            device.port.Add(new PointF(0.3f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.5f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.7f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.3f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.5f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.7f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));


            AllDevice.Add(device);
            //ShapeNode node = diagram1.Factory.CreateShapeNode(20, 20, 40, 40);
            //node.Id = "Role bao ve mat pha";
            //node.Image = Properties.Resources._17_role_bao_ve_mat_pha;
            //node.Transparent = true;
            //node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;
        }

        private void button22_Click(object sender, EventArgs e)
        {

        }

        private void button74_Click(object sender, EventArgs e)
        {
            Device device = new Device(8);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 56);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._22_Role;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));


            AllDevice.Add(device);
        }

        private void button73_Click(object sender, EventArgs e)
        {
            Device device = new Device(8);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 56);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._15_RoLE_NHIeT;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));


            AllDevice.Add(device);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            Device device = new Device(8);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 56);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._12_ro_le_thoi_gian;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));


            AllDevice.Add(device);
        }

        private void button80_Click(object sender, EventArgs e)
        {
            Device device = new Device(10);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._28_MayBienApCamUng;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.1f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.55f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.1f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.675f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.92f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.675f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.92f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.81f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
        }

        private void button79_Click(object sender, EventArgs e)
        {
            Device device = new Device(10);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._29_MayBienAp3Pha;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.1f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.55f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.1f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.675f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.92f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.675f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.92f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.81f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
        }

        private void button75_Click(object sender, EventArgs e)
        {
            Device device = new Device(10);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._27_BienApTuNgau;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.1f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.55f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.1f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.675f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.92f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.675f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.92f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.81f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
        }

        private void button77_Click(object sender, EventArgs e)
        {
            Device device = new Device(8);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 56);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._17_role_bao_ve_mat_pha;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));


            AllDevice.Add(device);
        }

        private void button76_Click(object sender, EventArgs e)
        {
            Device device = new Device(8);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 56);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._21_CAU_CHI;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));


            AllDevice.Add(device);
        }

        private void button53_Click_1(object sender, EventArgs e)
        {
            Device device = new Device(8);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 56);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._16_role_bao_ve_qua_dong;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));


            AllDevice.Add(device);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            Device device = new Device(8);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 56);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._18_role_bao_ve_dien_ap;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));


            AllDevice.Add(device);
        }



        private void button14_Click(object sender, EventArgs e)
        {
            Device device = new Device(9);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._8_CongTacHanhTrinh;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 2;
            device.port.Add(new PointF(0.19f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.9f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.51f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.9f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
            //ShapeNode node = diagram1.Factory.CreateShapeNode(20, 20, 40, 40);
            //node.Id = "Role bao ve dien ap";
            //node.Image = Properties.Resources._18_role_bao_ve_dien_ap;
            //node.Transparent = true;
            //node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;
        }
        private void button13_Click(object sender, EventArgs e)
        {
            Device device = new Device(10);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._9_NutAnXanhKhongNho;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.1f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.55f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.1f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.675f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.92f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.675f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.92f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.81f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
            //ShapeNode node = diagram1.Factory.CreateShapeNode(20, 20, 40, 40);
            //node.Id = "Role bao ve dien ap";
            //node.Image = Properties.Resources._18_role_bao_ve_dien_ap;
            //node.Transparent = true;
            //node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            Device device = new Device(11);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._10_NutAnXanhNho;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 2;
            device.port.Add(new PointF(0 * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.39f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0 * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.64f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
            //ShapeNode node = diagram1.Factory.CreateShapeNode(20, 20, 40, 40);
            //node.Id = "Role bao ve dien ap";
            //node.Image = Properties.Resources._18_role_bao_ve_dien_ap;
            //node.Transparent = true;
            //node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;
        }
        private void button17_Click(object sender, EventArgs e)
        {
            Device device = new Device(12);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._11_NutAnDoKhongNho;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.1f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.55f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.1f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.675f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.92f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.675f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.92f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.81f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
            //ShapeNode node = diagram1.Factory.CreateShapeNode(20, 20, 40, 40);
            //node.Id = "Role bao ve dien ap";
            //node.Image = Properties.Resources._18_role_bao_ve_dien_ap;
            //node.Transparent = true;
            //node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit; 
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Device device = new Device(13);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._12_NutAnDoNho;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 2;
            device.port.Add(new PointF(0.64f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0 * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.86f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0 * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
            //ShapeNode node = diagram1.Factory.CreateShapeNode(20, 20, 40, 40);
            //node.Id = "Role bao ve dien ap";
            //node.Image = Properties.Resources._18_role_bao_ve_dien_ap;
            //node.Transparent = true;
            //node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;
        }

        private void button33_Click(object sender, EventArgs e)
        {
            Device device = new Device(8);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 56);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._20_RoleBaoVeDienAp;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.05f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.55f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.6875f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.95f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));


            AllDevice.Add(device);

        }
        private void button21_Click(object sender, EventArgs e)
        {
            Device device = new Device(13);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._13_NutAnVangKhongNho;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 4;
            device.port.Add(new PointF(0.1f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.55f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.1f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.675f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.92f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.675f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.92f* device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0.81f * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            AllDevice.Add(device);
        }

        //dao cach ly
        private void button19_Click(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {
            Device device = new Device(13);
            ShapeNode node = diagram1.Factory.CreateShapeNode(10, 10, 40, 40);

            device.SetNode(node);

            countDevice += 1;
            node.Id = "" + countDevice;
            node.Image = Properties.Resources._27_nut_an_xanh_Khong_nho;
            node.Transparent = true;
            node.ImageAlign = MindFusion.Drawing.ImageAlign.Fit;


            device.PortCount = 2;
            device.port.Add(new PointF(0.56f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0 * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));
            device.port.Add(new PointF(0.87f * device.shapenode.Bounds.Width + device.shapenode.Bounds.X,
                0 * device.shapenode.Bounds.Height + device.shapenode.Bounds.Y));

            AllDevice.Add(device);
        }

        #endregion

        #endregion


        #region properties

        //ZOOM IN ZOOM OUT
        private void btn_ZoomOut_Click(object sender, EventArgs e)
        {
            float zoom = diagramView1.ZoomFactor;
            if (zoom < 500)
            {
                zoom += 25;
                diagramView1.ZoomFactor = zoom;
                labelZoom.Text = zoom + "%";
            }
            else return;

        }

        private void btn_ZoomIn_Click(object sender, EventArgs e)
        {
            float zoom = diagramView1.ZoomFactor;
            if (zoom > 25)
            {
                zoom -= 25;
                diagramView1.ZoomFactor = zoom;
                labelZoom.Text = zoom + "%";
            }
            else return;
        }

        bool firstClick = true;
        private void TextboxLine_Click(object sender, EventArgs e)
        {
            if (firstClick)
            {
                List<DiagramItem> ListItemDiagram = GetItemSelected();
                if (ListItemDiagram.Count == 1)
                {
                    if (ListItemDiagram[0].GetType() == typeof(DiagramLink))
                    {
                        TextboxLine.Text = ListItemDiagram[0].Text;
                    }
                }
                TextboxLine.SelectAll();
                firstClick = false;
            }

        }
        /// <summary>
        /// get all item seleted in diagram 
        /// </summary>
        /// <returns></returns>
        private List<DiagramItem> GetItemSelected()
        {
            List<DiagramItem> ListItemDiagram = new List<DiagramItem>();
            foreach (var item in diagram1.Items)
            {
                if (item.Selected == true)
                {
                    ListItemDiagram.Add(item);
                }
            }
            return ListItemDiagram;
        }
        private void TextboxLine_Leave(object sender, EventArgs e)
        {
            firstClick = !firstClick;
            List<DiagramItem> ListItemDiagram = GetItemSelected();
            if (ListItemDiagram.Count == 1)
            {
                if (ListItemDiagram[0].GetType() == typeof(DiagramLink))
                {
                    ListItemDiagram[0].Text = TextboxLine.Text;
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("sele");
            List<DiagramItem> ListItemDiagram = GetItemSelected();

            if (ListItemDiagram.Count == 1)
            {
                if (ListItemDiagram[0].GetType() == typeof(DiagramLink))
                {
                    if (comboBox2.SelectedIndex == 0)
                    {
                        ListItemDiagram[0].Pen = new MindFusion.Drawing.Pen(Color.Red, Convert.ToInt32(TextboxWidth.Text));
                    }
                    if (comboBox2.SelectedIndex == 1)
                    {
                        ListItemDiagram[0].Pen = new MindFusion.Drawing.Pen(Color.Blue, Convert.ToInt32(TextboxWidth.Text));
                    }
                    if (comboBox2.SelectedIndex == 2)
                    {
                        ListItemDiagram[0].Pen = new MindFusion.Drawing.Pen(Color.Yellow, Convert.ToInt32(TextboxWidth.Text));
                    }
                }
                
            }

        }

        private void TextboxWidth_Click(object sender, EventArgs e)
        {

        }

        private void TextboxWidth_Leave(object sender, EventArgs e)
        {
            List<DiagramItem> ListItemDiagram = GetItemSelected();
            if (ListItemDiagram.Count == 1)
            {
                if (ListItemDiagram[0].GetType() == typeof(DiagramLink))
                {
                    Color color = ListItemDiagram[0].Pen.Color;
                    ListItemDiagram[0].Pen = new MindFusion.Drawing.Pen(color, Convert.ToInt32(TextboxWidth.Text));
                }
            }
        }

        private void diagram1_LinkSelected(object sender, LinkEventArgs e)
        {

            List<DiagramItem> ListItemDiagram = GetItemSelected();
            if (ListItemDiagram.Count == 1)
            {
                if (ListItemDiagram[0].GetType() == typeof(DiagramLink))
                {
                    try
                    {
                        TextboxLine.Text = ListItemDiagram[0].Text;
                        if (TextboxWidth.Text != null)
                        {
                            TextboxWidth.Text = ListItemDiagram[0].Pen.Width.ToString();
                        }
                        

                        if (ListItemDiagram[0].Pen.Color == Color.Red)
                        {
                            comboBox2.Text = "RED";
                        }
                        else if(ListItemDiagram[0].Pen.Color == Color.Blue)
                        {
                            comboBox2.Text = "BLUE";
                        }
                        else
                        {
                            comboBox2.Text = "YELLOW";
                        }

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }



        #endregion





        #region hiển thị flash
        private void button3_Click(object sender, EventArgs e)
        {
            //axShockwaveFlash1.Visible = true;
            //axShockwaveFlash1.Movie = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\2D\HeThongBoiTronAI9B.swf";
        }
        #endregion

        private void button4_Click(object sender, EventArgs e)
        {


        }

        private void panel_Thuchanh_Paint(object sender, PaintEventArgs e)
        {

        }

        #region Save
        private void btn_Export_Click(object sender, EventArgs e)
        {
            //unselected node
            diagram1.Selection.Items.ToList().ForEach(num => num.Selected = false);
            //save 
            saveFileDialog1.DefaultExt = "png";
            saveFileDialog1.Filter = "PNG files|*.png";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image image = diagram1.CreateImage();
                image.Save(saveFileDialog1.FileName);
                image.Dispose();
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            this.saveFileDialog2.DefaultExt = "elec";
            this.saveFileDialog2.FileName = "Flowchart";
            this.saveFileDialog2.Filter = "Electric documents|*.elec|All files|*.*";
            this.saveFileDialog2.Title = "Save document";

            if (saveFileDialog2.ShowDialog(this) == DialogResult.OK)
            {
                diagram1.SaveToFile(saveFileDialog2.FileName, true);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    diagram1.LoadFromFile(openFileDialog1.FileName);

                    this.tb_NameDocument.Text = "Name : " + System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.FileName).Substring(0,5);
                    //this.tb_NameDocument.Text += openFileDialog1.FileName;


                    if (panel_Thuchanh.Visible == true)
                    {
                        InvisibleMenu();
                        return;
                    }
                    else
                    {

                        InvisibleWork();
                        InvisibleMenu();
                        panel_Menu.Visible = true;
                        panel_Thuchanh.Visible = true;
                        panel_workthuchanh.Visible = true;
                        panel_Lineproperties.Visible = true;
                        panel_Simulate.Visible = true;
                    }


                }
                catch
                {
                    MessageBox.Show(this, "Không mở được định dạng này!");
                }
            }
        }

        private void btn_Grid_Click(object sender, EventArgs e)
        {


            diagram1.EnableLanes = !diagram1.EnableLanes;
            Grid grid = diagram1.LaneGrid;
            grid.RowCount = 400;
            grid.ColumnCount = 400;
            grid.HookHeaders = false;
            grid.HeadersOnTop = false;

        }

        #endregion


        List<Device> AllDevice = new List<Device>();

        public Device FindDevice(DiagramNode node)
        {
            foreach (var item in AllDevice)
            {
                if (item.shapenode.Id == node.Id)
                {
                    return item;
                }

            }
            return null;


        }

        private double GetDistance(PointF point1, PointF point2)
        {
            return Math.Sqrt(Math.Pow((point1.X - point2.X), 2) + Math.Pow((point1.Y - point2.Y), 2));
        }

        private void diagram1_LinkRouted(object sender, LinkEventArgs e)
        {

        }



        private void diagram1_LinkCreating(object sender, LinkValidationEventArgs e)
        {
            e.Link.Pen(new MindFusion.Drawing.Pen(Color.Red, 2));

        }

        private void diagram1_LinkCreated(object sender, LinkEventArgs e)
        {
            e.Link.ZBottom(true);
            if (e.Link.Origin == null || e.Link.Destination == null)
            {

                return;

            }
            

            //tim kiem device goc
            Device origin = FindDevice(e.Link.Origin);
            Device destination = FindDevice(e.Link.Destination);
            if (origin == null || destination == null)
            {
                return;
            }

            // tim kiem diem port
            //port origin
            PointF portOrigin = origin.port[0];
            double distanceOrigin = GetDistance(e.Link.StartPoint, portOrigin);
            for (int i = 1; i < origin.PortCount; i++)
            {
                if (distanceOrigin > GetDistance(e.Link.StartPoint, origin.port[i]))
                {
                    portOrigin = origin.port[i];
                    distanceOrigin = GetDistance(e.Link.StartPoint, origin.port[i]);
                }
            }
            if (distanceOrigin < 5)
            {
                //tim diem gan sat diem start

                PointF f = e.Link.ControlPoints.GetAt(1);
                
                if (e.Link.StartPoint.X == f.X)
                {
                    e.Link.ControlPoints.SetAt(1, new PointF(portOrigin.X, f.Y));
                }

                else if (e.Link.StartPoint.Y == f.Y)
                {
                    e.Link.ControlPoints.SetAt(1, new PointF(f.X, portOrigin.Y));
                }
                else
                {

                }

                e.Link.StartPoint = portOrigin;
                


            }


            //port destination

            PointF portDestination = destination.port[0];
            double distanceDestination = GetDistance(e.Link.EndPoint, portDestination);
            for (int i = 1; i < destination.PortCount; i++)
            {
                if (distanceDestination > GetDistance(e.Link.EndPoint, destination.port[i]))
                {
                    portDestination = destination.port[i];
                    distanceDestination = GetDistance(e.Link.EndPoint, destination.port[i]);
                }
            }
            if (distanceDestination < 5)
            {
                
                int countPoint = e.Link.ControlPoints.Count;
                PointF f = e.Link.ControlPoints.GetAt(countPoint - 2);

                if (e.Link.EndPoint.X == f.X)
                {
                    e.Link.ControlPoints.SetAt(countPoint - 2, new PointF(portDestination.X, f.Y));
                }

                else if (e.Link.EndPoint.Y == f.Y)
                {
                    e.Link.ControlPoints.SetAt(countPoint - 2, new PointF(f.X, portDestination.Y));
                }
                else
                {

                }

                e.Link.EndPoint = portDestination;

            }

            
        }




        public ShapeNode portNode;
        /// <summary>
        /// Khi chuột rê đến một node, phát hiện có rê vào cổng - port của node đó hay không
        /// nếu rê vào thì hiển thị hình tròn tại port đó
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        DiagramLink link;
        int createLink = 0;

        private void diagramView1_MouseMove(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("asdf");
            if (createLink == 1)
            {
                //link.EndPoint(new PointF(e.X, e.Y));

            }

        }

        private void diagram1_LinkSplit(object sender, LinkEventArgs e)
        {



        }
        public PointF SubPointF(PointF p1, PointF p2)
        {
            return new PointF(p1.X - p2.X, p1.Y - p2.Y);
        }
        public PointF TotalPointF(PointF p1, PointF p2)
        {
            return new PointF(p1.X + p2.X, p1.Y + p2.Y);


        }
        public PointF MulPointFtoX(PointF p1, float x)
        {
            return new PointF(p1.X * x, p1.Y * x);
        }


        private void diagram1_NodeModified(object sender, NodeEventArgs e)
        {
            Device device = FindDevice(e.Node);
            //cap nhat lai cac port 
            try {
                for (int i = 0; i < device.PortCount; i++)
                {
                    //cap nhat lai vi tri
                    PointF SubPoint = SubPointF(new PointF(e.Node.Bounds.X, e.Node.Bounds.Y), device.prePos);
                    device.port[i] = TotalPointF(device.port[i], SubPoint);
                    //cap nhat theo width, height
                    //cap nhat theo ti le
                    //
                    //
                    //
                    //
                    //
                    //
                    float fxHeight = e.Node.Bounds.Height / device.height;
                    float fxWidth = e.Node.Bounds.Width / device.width;
                    if (fxHeight != 1 || fxWidth != 1)
                    {
                        device.port[i] = new PointF(device.port[i].X * fxWidth, device.port[i].Y * fxHeight);
                    }
                }
                device.prePos = new PointF(e.Node.Bounds.X, e.Node.Bounds.Y);
                device.width = e.Node.Bounds.Width;
                device.height = e.Node.Bounds.Height;
            }
            catch (Exception) { return; }
        }

        private void diagram1_LinkModified(object sender, LinkEventArgs e)
        {
            //neu link chua connect, va chinh sua diem cuoi cung
            if (!e.Link.IsConnected)
            {
                int count = e.Link.ControlPoints.Count;
                //neu chua di chuyen diem cuoi cung
                if (GetDistance(e.Link.ControlPoints.GetAt(count - 1), e.Link.ControlPoints.GetAt(count - 2)) > 1)
                {

                    //if (e.Link.ControlPoints.GetAt(count - 1).X == e.Link.ControlPoints.GetAt(count - 2).X)
                    //{
                    e.Link.ControlPoints.Add(new PointF(e.Link.ControlPoints.GetAt(count - 1).X
                                        , e.Link.ControlPoints.GetAt(count - 1).Y));
                    //}
                    //else
                    //{
                    //    e.Link.ControlPoints.Add(new PointF(e.Link.ControlPoints.GetAt(count - 1).X
                    //                        , e.Link.ControlPoints.GetAt(count - 1).Y));
                    //}

                    return;
                }
            }
            else
            {
                



                if (e.Link.Origin == null || e.Link.Destination == null)
                {

                    return;
                }
                //tim kiem device goc
                Device origin = FindDevice(e.Link.Origin);
                Device destination = FindDevice(e.Link.Destination);
                if (origin == null || destination == null)
                {
                    return;
                }

                // tim kiem diem port 
                //port origin
                PointF portOrigin = origin.port[0];
                double distanceOrigin = GetDistance(e.Link.StartPoint, portOrigin);
                for (int i = 1; i < origin.PortCount; i++)
                {
                    if (distanceOrigin > GetDistance(e.Link.StartPoint, origin.port[i]))
                    {
                        portOrigin = origin.port[i];
                        distanceOrigin = GetDistance(e.Link.StartPoint, origin.port[i]);
                    }
                }
                if (distanceOrigin < 5)
                {
                    //tim diem gan sat diem start

                    PointF f = e.Link.ControlPoints.GetAt(1);

                    if (e.Link.StartPoint.X == f.X)
                    {
                        e.Link.ControlPoints.SetAt(1, new PointF(portOrigin.X, f.Y));
                    }

                    else if (e.Link.StartPoint.Y == f.Y)
                    {
                        e.Link.ControlPoints.SetAt(1, new PointF(f.X, portOrigin.Y));
                    }
                    else
                    {

                    }

                    e.Link.StartPoint = portOrigin;
                }


                //port destination

                PointF portDestination = destination.port[0];
                double distanceDestination = GetDistance(e.Link.EndPoint, portDestination);
                for (int i = 1; i < destination.PortCount; i++)
                {
                    if (distanceDestination > GetDistance(e.Link.EndPoint, destination.port[i]))
                    {
                        portDestination = destination.port[i];
                        distanceDestination = GetDistance(e.Link.EndPoint, destination.port[i]);
                    }
                }
                if (distanceDestination < 5)
                {

                    int countPoint = e.Link.ControlPoints.Count;
                    PointF f = e.Link.ControlPoints.GetAt(countPoint - 2);

                    if (e.Link.EndPoint.X == f.X)
                    {
                        e.Link.ControlPoints.SetAt(countPoint - 2, new PointF(portDestination.X, f.Y));
                    }

                    else if (e.Link.EndPoint.Y == f.Y)
                    {
                        e.Link.ControlPoints.SetAt(countPoint - 2, new PointF(f.X, portDestination.Y));
                    }
                    else
                    {

                    }

                    e.Link.EndPoint = portDestination;

                }

            }
        }

        private void buttonUndo_Click(object sender, EventArgs e)
        {

            diagram1.UndoManager.Undo();

        }

        private void buttonRedo_Click(object sender, EventArgs e)
        {
            diagram1.UndoManager.Redo();
        }

        private void diagram1_LinkClicked(object sender, LinkEventArgs e)
        {
            //PointF f = e.Link.ControlPoints.GetAt(1);
            //MessageBox.Show("" + f.X + "y : " + f.Y);
        }

        //bai giangr
        #region bai giang
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private void LoadTreeView()
        {
            //lam sach cay bai giag
            treeView_Baigiang.Nodes.Clear();
            string directory = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Baigiang\Baigiang\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory); 
            }

            if (Directory.GetDirectories(directory).Count() == 0)
            {
                return;
            }
            foreach (var item in Directory.GetDirectories(directory))
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(item);
                TreeNode node = new TreeNode();
                node.Name = name;
                node.Text = name;
                node.ImageIndex = 0;
                node.SelectedImageIndex = 0;
                node.Tag = item;
                treeView_Baigiang.Nodes.Add(node);

                treeView_Baigiang.SelectedNode = node;
                //hien thi cac node con
                foreach (var item1 in Directory.GetFiles(item))
                {
                    string name1 = System.IO.Path.GetFileNameWithoutExtension(item1);
                    string fileExtension = System.IO.Path.GetExtension(item1).ToLower();
                    TreeNode node1 = new TreeNode();
                    node1.Name = name1;
                    node1.Text = name1;
                    node1.Tag = item1;
                    switch (fileExtension)
                    {
                        case ".xls":
                        case ".xlsx":
                            node1.ImageIndex = 4;
                            node1.SelectedImageIndex = 4;
                            break;
                        case ".ppt":
                        case ".pttx":
                            node1.ImageIndex = 3;
                            node1.SelectedImageIndex = 3;
                            break;
                        case ".doc":
                        case ".docx":
                            node1.ImageIndex = 2;
                            node1.SelectedImageIndex = 2;
                            break;
                        case ".pdf":
                            node1.ImageIndex = 1;
                            node1.SelectedImageIndex = 1;
                            break;
                        //*.mp4, *.avi,*.3gp,*.mov,*.mp4v,
                        case ".mp4":
                        case ".avi":
                        case ".3gp":
                        case ".mov":
                        case ".mp4v":
                            node1.ImageIndex = 5;
                            node1.SelectedImageIndex = 5;
                            break;

                        case ".elec":
                            node1.ImageIndex = 1;
                            node1.SelectedImageIndex = 1;
                            break;
                        default:
                            break;
                    }

                    treeView_Baigiang.SelectedNode.Nodes.Add(node1);
                }

            }
        }

        private void btn_NewBaigiang_Click(object sender, EventArgs e)
        {
            string value = "Bai giang 1";
            if (Form1.InputBox("Thêm mới bài giảng", "Ten bai giang :", ref value) == DialogResult.OK)
            {
                //tao node bai giang
                //kiem tra bai giang da ton tai chua
                foreach (TreeNode item in treeView_Baigiang.Nodes)
                {
                    if (item.Name == value)
                    {
                        MessageBox.Show("Bài giảng đã tồn tại!");
                        return;
                    }
                }
                
                string dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath) +@"\Baigiang\Baigiang\"+value;
                if (!Directory.Exists(dir))
                {
                    //neu chua thi tao bai giang moi
                    TreeNode node = new TreeNode();
                    node.Name = value;
                    node.Text = value;
                    node.ImageIndex = 0;
                    node.Tag = dir;
                    treeView_Baigiang.Nodes.Add(node);
                    //tao moi thu muc bai giang
                    Directory.CreateDirectory(dir);
                }
                else
                {
                    MessageBox.Show("Bài giảng đã tồn tại!");
                    return;
                }
            }
        }


        #endregion


        private void LoadBaigiangPdf(string file)
        {
            InvisibleWork();
            InvisibleMenu();
            panel_work_baigiang.Visible = true;
            panel_BaiGiang.Visible = true;
            string outfile = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "/file.pdf";
            string extension = System.IO.Path.GetExtension(file).ToLower();
            if (extension == ".doc" || extension == ".docx")
            {
                ConvertWordToPdf(file, outfile);
            }

            if (extension == ".ppt" || extension == ".pptx")
            {
                ConvertPowerPointToPdf(file, outfile);

            }

            if (extension == ".xls" || extension == ".xlsx")
            {
                ConvertExcelToPdf(file, outfile);
            }

            if (extension == ".pdf")
            {
                outfile = file;
            }
            axAcroPDF2.src = outfile;
            pictureBox2.Visible = false;
        }

        private void LoadBaigiangElec(string file)
        {
            diagram1.LoadFromFile(file);
            InvisibleWork();
            //panel_Thuchanh.Visible = true;
            panel_BaiGiang.Visible = true;
            panel_workthuchanh.Visible = true;
            panel_Lineproperties.Visible = true;
            panel_Simulate.Visible = true;

        }
        private void treeView_Baigiang_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            
        }

        private void treeView_Baigiang_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                return;
            }
            string file = e.Node.Tag.ToString();
            string extension = System.IO.Path.GetExtension(file);
            if (extension==".elec" || extension=="ELEC")
            {
                LoadBaigiangElec(file);
            }
            else
            {
                LoadBaigiangPdf(file);
            }
        }

        private void treeView_Baigiang_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }
        //node nào đã được click
        TreeNode nodeSelected;
        private void treeView_Baigiang_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //hien thi cac button
            if (!btn_XoaFile.Visible)
            {
                btn_XoaFile.Visible = true;
                btn_ThemFile.Visible = true;
                btn_MoFile.Visible = true;
            }
            
            nodeSelected = e.Node;
        }

        public void XoaFile()
        {


            DialogResult result = MessageBox.Show("Bạn có thực sự muốn xóa ?", "Cảnh báo", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                if (treeView_Baigiang.SelectedNode.Level == 0)
                {
                    //xoa het file
                    foreach (TreeNode item in treeView_Baigiang.SelectedNode.Nodes)
                    {
                        File.Delete(item.Tag.ToString());
                    }

                    Directory.Delete(treeView_Baigiang.SelectedNode.Tag.ToString());

                    if (treeView_Baigiang.Nodes.Count == 1)
                    {
                        treeView_Baigiang.SelectedNode = treeView_Baigiang.TopNode;
                    }
                    treeView_Baigiang.SelectedNode.Remove();

                }
                else { File.Delete(treeView_Baigiang.SelectedNode.Tag.ToString()); treeView_Baigiang.SelectedNode.Remove(); }



                }
        }

        private void btn_XoaFile_Click(object sender, EventArgs e)
        {

            XoaFile();
        }


        private void ThemFile()
        {
            if (treeView_Baigiang.SelectedNode == null)
            {
                return;
            }

            while (treeView_Baigiang.SelectedNode.Level != 0)
            {
                treeView_Baigiang.SelectedNode = treeView_Baigiang.SelectedNode.Parent;

            }

            OpenFileDialog openfile = new OpenFileDialog()
            {
                DefaultExt = ".pdf",
                Filter = @"Data Files (*.xls, *.xlsx,*.ppt,*.pptx.*.doc,*.docx,*.pdf,*.mp4, *.avi,*.3gp,*.mov,*.mp4v,*.elec)|*.xls; *.xlsx; *.ppt; *.pptx; *.doc; *.docx; *.pdf;*.mp4; *.avi;*.3gp;*.mov;*.mp4v;*.elec|Pdf Files (*.pdf)|*.pdf|Excel Files (*.xls, *.xlsx)|
                    *.xls;*.xlsx|PowerPoint Files (*.ppt, *.pptx)|*.ppt;*.pptx|Word Files (*.doc, *.docx)|*.doc;*.docx|Video Files (*.mp4, *.avi,*.3gp,*.mov,*.mp4v)|*.mp4; *.avi;*.3gp;*.mov;*.mp4v|Electric File(*.elec)|*.elec"
            };
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                foreach (var item in openfile.FileNames)
                {
                    if (treeView_Baigiang.SelectedNode != null)
                    {
                        string filename;
                        filename = System.IO.Path.GetFileNameWithoutExtension(item);
                        string fileExtension = System.IO.Path.GetExtension(item).ToLower();
                        TreeNode node = new TreeNode();
                        node.Name = filename;
                        node.Text = filename;
                        node.Tag = item;
                        switch (fileExtension)
                        {
                            case ".xls":
                            case ".xlsx":
                                node.ImageIndex = 4;
                                node.SelectedImageIndex = 4;
                                break;
                            case ".ppt":
                            case ".pttx":
                                node.ImageIndex = 3;
                                node.SelectedImageIndex = 3;
                                break;
                            case ".doc":
                            case ".docx":
                                node.ImageIndex = 2;
                                node.SelectedImageIndex = 2;
                                break;
                            case ".pdf":
                                node.ImageIndex = 1;
                                node.SelectedImageIndex = 1;
                                break;
                            //*.mp4, *.avi,*.3gp,*.mov,*.mp4v,
                            case ".mp4":
                            case ".avi":
                            case ".3gp":
                            case ".mov":
                            case ".mp4v":
                                node.ImageIndex = 5;
                                node.SelectedImageIndex = 5;
                                break;

                            case ".elec":
                                node.ImageIndex = 1;
                                node.SelectedImageIndex = 1;
                                break;
                            default:
                                break;
                        }

                        //copy file vao parent node
                        string str = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Baigiang\Baigiang\" + treeView_Baigiang.SelectedNode.Name + @"\" + filename + fileExtension;
                        int count = 1;
                        while (File.Exists(str))
                        {
                            str = str + "(" + count++ + ")";
                        }
                        File.Copy(item, str);
                        treeView_Baigiang.SelectedNode.Nodes.Add(node);
                        treeView_Baigiang.SelectedNode = node;
                        treeView_Baigiang.ExpandAll();
                    }
                }
            }
        }

        private void btn_ThemFile_Click(object sender, EventArgs e)
        {
            ThemFile();
        }

        
        private void MoFile()
        {

            if (treeView_Baigiang.SelectedNode.Level == 0)
            {
                return;
            }

            string file = treeView_Baigiang.SelectedNode.Tag.ToString();
            string extension = System.IO.Path.GetExtension(file);
            if (extension == ".elec" || extension == "ELEC")
            {
                LoadBaigiangElec(file);

            }
            else
            {
                LoadBaigiangPdf(file);
            }
        }


        private void btn_MoFile_Click(object sender, EventArgs e)
        {
            MoFile();
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!panel_BaiGiang.Visible)
            {
                return false;
            }
            if (keyData == Keys.Enter)
            {
                MoFile();
                return true;
            }
            if (keyData == Keys.Escape)
            {
                ThemFile();
                return true;
            }
            if (keyData == Keys.Delete)
            {
                XoaFile();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public bool ConvertWordToPdf(string inputFile, string outputfile)
        {

            Microsoft.Office.Interop.Word.Application wordApp =
            new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document wordDoc = null;
            object inputFileTemp = inputFile;

            try
            {
                wordDoc = wordApp.Documents.Open(inputFile);
                wordDoc.ExportAsFixedFormat(outputfile, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
            }
            catch (Exception)
            {
                MessageBox.Show("Co loi xay ra");
                return false;
            }
            finally
            {
                if (wordDoc != null)
                {
                    wordDoc.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
                }
                if (wordApp != null)
                {
                    wordApp.Quit(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
                    wordApp = null;
                }
            }

            return true;
        }

        public static bool ConvertPowerPointToPdf(string inputFile, string outputfile)
        {
            string outputFileName = outputfile;
            Microsoft.Office.Interop.PowerPoint.Application powerPointApp =
            new Microsoft.Office.Interop.PowerPoint.Application();
            Microsoft.Office.Interop.PowerPoint.Presentation presentation = null;
            Microsoft.Office.Interop.PowerPoint.Presentations presentations = null;
            try
            {
                presentations = powerPointApp.Presentations;
                presentation = presentations.Open(inputFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse,
                Microsoft.Office.Core.MsoTriState.msoFalse);

                presentation.ExportAsFixedFormat(outputFileName, Microsoft.Office.Interop.PowerPoint.PpFixedFormatType.ppFixedFormatTypePDF,
                Microsoft.Office.Interop.PowerPoint.PpFixedFormatIntent.ppFixedFormatIntentScreen, Microsoft.Office.Core.MsoTriState.msoFalse,
                Microsoft.Office.Interop.PowerPoint.PpPrintHandoutOrder.ppPrintHandoutVerticalFirst, Microsoft.Office.Interop.PowerPoint.PpPrintOutputType.ppPrintOutputSlides,
                Microsoft.Office.Core.MsoTriState.msoFalse, null, Microsoft.Office.Interop.PowerPoint.PpPrintRangeType.ppPrintAll, string.Empty, false, true, true, true, false,
                Type.Missing);
            }
            catch (Exception)
            {
                MessageBox.Show("Co loi xay ra");
                return false;
            }
            finally
            {
                if (presentation != null)
                {
                    presentation.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(presentation);
                    presentation = null;
                }
                if (powerPointApp != null)
                {
                    powerPointApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(powerPointApp);
                    powerPointApp = null;
                }
            }
            return true;
        }

        public static bool ConvertExcelToPdf(string inputFile, string outputfile)
        {
            string outputFileName = outputfile;
            Microsoft.Office.Interop.Excel.Application excelApp =
            new Microsoft.Office.Interop.Excel.Application();
            excelApp.Visible = false;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            Microsoft.Office.Interop.Excel.Workbooks workbooks = null;
            try
            {
                //ExportAsFixedFormatXlFixedFormatType
                workbooks = excelApp.Workbooks;
                workbook = workbooks.Open(inputFile);
                workbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, outputFileName,
                Microsoft.Office.Interop.Excel.XlFixedFormatQuality.xlQualityStandard, true, true, Type.Missing, Type.Missing, false, Type.Missing);
            }
            catch (Exception)
            {
                MessageBox.Show("Co loi xay ra");
                return false;
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close(Microsoft.Office.Interop.Excel.XlSaveAction.xlDoNotSaveChanges);
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workbook) != 0) { };
                    workbook = null;
                }
                if (workbooks != null)
                {
                    workbooks.Close();
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workbooks) != 0) { };
                    workbooks = null;
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    excelApp.Application.Quit();
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelApp) != 0) { };
                    excelApp = null;
                }
            }

            return true;
        }



        private void button68_Click(object sender, EventArgs e)
        {

        }

        private void button67_Click(object sender, EventArgs e)
        {

        }

        private void button64_Click(object sender, EventArgs e)
        {

        }

        private void button61_Click(object sender, EventArgs e)
        {

        }

        private void button58_Click(object sender, EventArgs e)
        {

        }

        private void button55_Click(object sender, EventArgs e)
        {

        }

        private void button56_Click(object sender, EventArgs e)
        {

        }

        private void button59_Click(object sender, EventArgs e)
        {

        }

        private void button62_Click(object sender, EventArgs e)
        {

        }

        private void button65_Click(object sender, EventArgs e)
        {

        }

        private void button69_Click(object sender, EventArgs e)
        {

        }

        private void button72_Click(object sender, EventArgs e)
        {

        }

        private void button70_Click(object sender, EventArgs e)
        {

        }

        private void button66_Click(object sender, EventArgs e)
        {

        }

        private void button63_Click(object sender, EventArgs e)
        {

        }

        private void button60_Click(object sender, EventArgs e)
        {

        }

        private void button_LoadBaigiang_Click(object sender, EventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                EnsurePathExists = true,
                EnsureFileExists = false,
                AllowNonFileSystemItems = false,
                Multiselect = false,               
                DefaultFileName = "Chọn thư mục bài giảng",
                Title = "Chọn thư mục để thêm bài giảng"
                
            };
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {

                string path = dialog.FileName;
                string foldername = System.IO.Path.GetFileNameWithoutExtension(path);
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Baigiang\Baigiang\" + foldername);
                if (Directory.GetFiles(dialog.FileName).Count() ==0)
                {
                    return;
                }
                    foreach (var item in Directory.GetFiles(path))
                    {
                        string extension = System.IO.Path.GetExtension(item).ToLower();
                    if      (extension == ".xls"||
                            extension == ".xlsx"||
                            extension == ".ppt" ||
                            extension == ".pttx"||
                            extension == ".doc" ||
                            extension == ".docx"||
                            extension == ".pdf" ||
                            extension == ".mp4" ||
                            extension == ".avi" ||
                            extension == ".3gp" ||
                            extension == ".mov" ||
                            extension == ".mp4v"||
                            extension == ".png" ||
                            extension == ".jpg" ||
                            extension == ".jpeg"||
                            extension == ".elec")
                    {
                            File.Copy(item, System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Baigiang\Baigiang\" + foldername +@"\"+ System.IO.Path.GetFileName(item));
                    }
                    }
                LoadTreeView();
                }
            
        }

        private void InfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ứng dụng Electric-Simu có các chức năng nổi bật \n như xem 3D, mở các định dạng file, tạo bài giảng....", "Ứng dụng Electric-Simu", MessageBoxButtons.OK);
        }

        private void button_cadeemu_Click(object sender, EventArgs e)
        {
            
            new Vemachdien().Show();
        }

        private void button88_Click(object sender, EventArgs e)
        {

        }

        private void button96_Click(object sender, EventArgs e)
        {

        }

        private void button95_Click(object sender, EventArgs e)
        {

        }

        private void button94_Click(object sender, EventArgs e)
        {

        }

        private void button85_Click(object sender, EventArgs e)
        {


        }

        private void button64_Click_1(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// vẽ dây
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button24_Click(object sender, EventArgs e)
        {
            if (diagramView1.Behavior == Behavior.Modify)
            {
                diagramView1.Behavior = Behavior.DrawLinks;
            }
            else diagramView1.Behavior = Behavior.Modify;
        }
    }
}
