namespace Khinen
{
    partial class PDFViewerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PDFViewerForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_PrintOffice = new System.Windows.Forms.Button();
            this.button_SaveOffice = new System.Windows.Forms.Button();
            this.button_OpenOffice = new System.Windows.Forms.Button();
            this.axAcroPDF1 = new AxAcroPDFLib.AxAcroPDF();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(30)))));
            this.panel1.Controls.Add(this.button_PrintOffice);
            this.panel1.Controls.Add(this.button_SaveOffice);
            this.panel1.Controls.Add(this.button_OpenOffice);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 35);
            this.panel1.TabIndex = 4;
            // 
            // button_PrintOffice
            // 
            this.button_PrintOffice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(126)))), ((int)(((byte)(197)))));
            this.button_PrintOffice.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_PrintOffice.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(30)))));
            this.button_PrintOffice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_PrintOffice.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_PrintOffice.ForeColor = System.Drawing.Color.White;
            this.button_PrintOffice.Location = new System.Drawing.Point(118, 0);
            this.button_PrintOffice.Name = "button_PrintOffice";
            this.button_PrintOffice.Size = new System.Drawing.Size(59, 35);
            this.button_PrintOffice.TabIndex = 2;
            this.button_PrintOffice.Text = "&Print";
            this.button_PrintOffice.UseVisualStyleBackColor = false;
            this.button_PrintOffice.Click += new System.EventHandler(this.button_PrintOffice_Click);
            // 
            // button_SaveOffice
            // 
            this.button_SaveOffice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(126)))), ((int)(((byte)(197)))));
            this.button_SaveOffice.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_SaveOffice.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(30)))));
            this.button_SaveOffice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_SaveOffice.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_SaveOffice.ForeColor = System.Drawing.Color.White;
            this.button_SaveOffice.Location = new System.Drawing.Point(59, 0);
            this.button_SaveOffice.Name = "button_SaveOffice";
            this.button_SaveOffice.Size = new System.Drawing.Size(59, 35);
            this.button_SaveOffice.TabIndex = 1;
            this.button_SaveOffice.Text = "&Save";
            this.button_SaveOffice.UseVisualStyleBackColor = false;
            this.button_SaveOffice.Click += new System.EventHandler(this.button_SaveOffice_Click);
            // 
            // button_OpenOffice
            // 
            this.button_OpenOffice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(126)))), ((int)(((byte)(197)))));
            this.button_OpenOffice.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_OpenOffice.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(30)))));
            this.button_OpenOffice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_OpenOffice.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_OpenOffice.ForeColor = System.Drawing.Color.White;
            this.button_OpenOffice.Location = new System.Drawing.Point(0, 0);
            this.button_OpenOffice.Name = "button_OpenOffice";
            this.button_OpenOffice.Size = new System.Drawing.Size(59, 35);
            this.button_OpenOffice.TabIndex = 0;
            this.button_OpenOffice.Text = "&Open";
            this.button_OpenOffice.UseVisualStyleBackColor = false;
            this.button_OpenOffice.Click += new System.EventHandler(this.button_OpenOffice_Click);
            this.button_OpenOffice.MouseLeave += new System.EventHandler(this.button_OpenOffice_MouseLeave);
            this.button_OpenOffice.MouseHover += new System.EventHandler(this.button_OpenOffice_MouseHover);
            // 
            // axAcroPDF1
            // 
            this.axAcroPDF1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axAcroPDF1.Enabled = true;
            this.axAcroPDF1.Location = new System.Drawing.Point(0, 35);
            this.axAcroPDF1.Name = "axAcroPDF1";
            this.axAcroPDF1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAcroPDF1.OcxState")));
            this.axAcroPDF1.Size = new System.Drawing.Size(800, 666);
            this.axAcroPDF1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 35);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 666);
            this.panel2.TabIndex = 6;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::Khinen.Properties.Resources.Asset_1_4x;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 666);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // PDFViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 701);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.axAcroPDF1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PDFViewerForm";
            this.Text = "PDFViewerForm";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_PrintOffice;
        private System.Windows.Forms.Button button_SaveOffice;
        private System.Windows.Forms.Button button_OpenOffice;
        private AxAcroPDFLib.AxAcroPDF axAcroPDF1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}