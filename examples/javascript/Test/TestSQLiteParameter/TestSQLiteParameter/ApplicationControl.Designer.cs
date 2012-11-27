using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TestSQLiteParameter
{
    public partial class ApplicationControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.applicationWebService1 = new TestSQLiteParameter.ApplicationWebService();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(396, 295);
            this.listBox1.TabIndex = 0;
            // 
            // ApplicationControl
            // 
            this.Controls.Add(this.listBox1);
            this.Name = "ApplicationControl";
            this.Size = new System.Drawing.Size(396, 295);
            this.Load += new System.EventHandler(this.ApplicationControl_Load);
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            // Note: This jsc project does not support unmanaged resources.
            base.Dispose(disposing);
        }

        private ListBox listBox1;
        private ApplicationWebService applicationWebService1;

    }
}
