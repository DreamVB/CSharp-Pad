using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CSharpPad
{
    public partial class frmOptions : Form
    {
        public frmOptions()
        {
            InitializeComponent();
        }

        private string GetOpenFilename()
        {
            OpenFileDialog od = new OpenFileDialog();
            string lzFile = string.Empty;

            od.Title = "Select File";
            od.Filter = "Program Files(*.exe)|*.exe";
            if (od.ShowDialog().Equals(DialogResult.OK))
            {
                lzFile = od.FileName;
            }
            od.Dispose();
            return lzFile;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            tools.CompilerPath = txtPath.Text;
            tools.SaveCompilerPath();
            Close();
        }

        private void lblSelectExe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtPath.Text = GetOpenFilename();
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            txtPath.Text = tools.CompilerPath;
        }

        private void lblSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string sPath = @"C:\Windows\Microsoft.NET\Framework\";

            string[] Files = Directory.GetFiles(sPath, "*.exe",SearchOption.AllDirectories);

            foreach (string f in Files)
            {
                FileInfo fi = new FileInfo(f);
                string exe = fi.Name.ToLower();

                if("csc.exe".Equals(exe,StringComparison.OrdinalIgnoreCase)){
                    txtPath.Text = fi.FullName;
                    break;
                }
            }
            Array.Clear(Files, 0, Files.Length);
        }
    }
}
