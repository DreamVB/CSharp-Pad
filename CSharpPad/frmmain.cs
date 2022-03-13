using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using System.Diagnostics;
using System.IO;
using System.Resources;

namespace CSharpPad
{
    public partial class frmmain : Form
    {

        private bool HasChanged { get; set; }
        private string DebugExe { get; set; }

        public frmmain()
        {
            InitializeComponent();
        }

        private string GetOpenFilename()
        {
            OpenFileDialog od = new OpenFileDialog();
            string lzFile = string.Empty;

            od.Title = "Open";
            od.Filter = "CS Files(*.cs)|*.cs";
            if (od.ShowDialog().Equals(DialogResult.OK))
            {
                lzFile = od.FileName;
            }
            od.Dispose();
            return lzFile;
        }

        private string GetSaveFilename()
        {
            SaveFileDialog sd = new SaveFileDialog();
            string lzFile = string.Empty;

            sd.Title = "Save As";
            sd.Filter = "CS Files(*.cs)|*.cs";
            if (sd.ShowDialog().Equals(DialogResult.OK))
            {
                lzFile = sd.FileName;
            }
            sd.Dispose();
            return lzFile;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Tag = "";
            tools.LoadCompilerPath();
            HasChanged = false;
        }

        private void fastColoredTextBox1_Load(object sender, EventArgs e)
        {

        }

        private void fastColoredTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!HasChanged)
            {
                HasChanged = true;
            }

            e.ChangedRange.ClearFoldingMarkers();
            e.ChangedRange.SetFoldingMarkers("{", "}");
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            string lzFile = string.Empty;

            if (HasChanged)
            {
                DialogResult dr = MessageBox.Show("The document has changed do you wish to save now.",
                    Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dr != DialogResult.Yes)
                {
                    txtEd.Text = Properties.Resources.Template;
                    this.Tag = "";
                    HasChanged = false;
                }
                else
                {
                    //Save
                    string tmpfile = this.Tag.ToString();
                    if (tmpfile.Length > 0)
                    {
                        //Save here
                        txtEd.SaveToFile(tmpfile, Encoding.Default);
                        txtEd.Text = Properties.Resources.Template;
                        this.Tag = "";
                        HasChanged = false;
                    }
                    else
                    {
                        lzFile = GetSaveFilename();
                        if (lzFile.Length != 0)
                        {
                            txtEd.SaveToFile(lzFile, Encoding.Default);
                            this.Tag = "";
                            txtEd.Text = Properties.Resources.Template;
                            HasChanged = false;
                        }
                    }

                }
            }
            else
            {
                txtEd.Text = Properties.Resources.Template;
                HasChanged = false;
                this.Tag = "";
            }
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            string lzFile = string.Empty;

            if (HasChanged)
            {

                DialogResult dr = MessageBox.Show("The document has changed do you want to save now.",
                    Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    lzFile = this.Tag.ToString();

                    if (lzFile.Length != 0)
                    {
                        //Save
                        txtEd.SaveToFile(lzFile, Encoding.Default);
                        //Open file.
                        lzFile = GetOpenFilename();
                        if (lzFile.Length > 0)
                        {
                            txtEd.OpenFile(lzFile, Encoding.Default);
                            this.Tag = lzFile;
                            HasChanged = false;
                        }
                    }
                    else
                    {
                        lzFile = GetSaveFilename();
                        if (lzFile.Length != 0)
                        {
                            txtEd.SaveToFile(lzFile, Encoding.Default);
                            lzFile = GetOpenFilename();

                            if (lzFile.Length > 0)
                            {
                                txtEd.OpenFile(lzFile, Encoding.Default);
                            }

                            this.Tag = lzFile;
                            HasChanged = false;
                        }
                    }
                }
                else if (dr == DialogResult.No)
                {
                    lzFile = GetOpenFilename();
                    if (lzFile.Length != 0)
                    {
                        txtEd.OpenFile(lzFile, Encoding.Default);
                        this.Tag = lzFile;
                        HasChanged = false;
                    }
                }
            }
            else
            {
                //Open
                lzFile = GetOpenFilename();
                if (lzFile.Length > 0)
                {
                    txtEd.OpenFile(lzFile, Encoding.Default);
                    this.Tag = lzFile;
                    HasChanged = false;
                }
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            string lzfile = this.Tag.ToString();

            if (lzfile.Length > 0)
            {
                txtEd.SaveToFile(lzfile, Encoding.Default);
                HasChanged = false;
            }
            else
            {
                lzfile = GetSaveFilename();
                if (lzfile.Length > 0)
                {
                    txtEd.SaveToFile(lzfile, Encoding.Default);
                    HasChanged = false;
                    this.Tag = lzfile;
                }
            }
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            string lzfile = GetSaveFilename();

            if (lzfile.Length != 0)
            {
                txtEd.OpenFile(lzfile, Encoding.Default);
                this.Tag = lzfile;
                HasChanged = false;
            }
        }

        private void mnuUndo_Click(object sender, EventArgs e)
        {
            txtEd.Undo();
        }

        private void mnuRedo_Click(object sender, EventArgs e)
        {

        }

        private void mnuCut_Click(object sender, EventArgs e)
        {
            txtEd.Cut();
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            txtEd.Copy();
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            txtEd.Paste();
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            txtEd.SelectedText = string.Empty;
            txtEd.Focus();
        }

        private void mnuSelectAll_Click(object sender, EventArgs e)
        {
            txtEd.SelectAll();
        }

        private void mnuOptions_Click(object sender, EventArgs e)
        {
            frmOptions frm = new frmOptions();
            frm.ShowDialog();
        }

        private void mnuCompile_Click(object sender, EventArgs e)
        {
            try
            {
                string fName = string.Empty;
                string fDebug = string.Empty;
                StringBuilder sb = new StringBuilder();

                //Check that there is a filename loaded.
                FileInfo fi = new FileInfo((string)this.Tag);
                fName = fi.Name.Substring(0, fi.Name.Length - 2) + "exe";
                //Create a debug folder.
                fDebug = tools.FixPath(fi.Directory.ToString()) + "debug\\";
                DebugExe = fDebug + fName;

                try
                {
                    if (!Directory.Exists(fDebug))
                    {
                        Directory.CreateDirectory(fDebug);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //Check for compiler path
                if (!File.Exists(tools.CompilerPath))
                {
                    MessageBox.Show("Cannot find compiler path:\n" + tools.CompilerPath,
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    //Compile the file
                    Process p = new Process();

                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.FileName = tools.CompilerPath;
                    p.StartInfo.Arguments = " /target:exe /out:" + tools.QString(DebugExe) + " " + tools.QString(fi.FullName) + " /nologo";
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();

                    while (!p.StandardOutput.EndOfStream)
                    {
                        string line = p.StandardOutput.ReadLine();
                        sb.AppendLine(line);
                    }
                    if(sb.Length > 0)
                        MessageBox.Show(sb.ToString(),
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("You must save the file before compileing.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void mnuRun_Click(object sender, EventArgs e)
        {
            if (File.Exists(DebugExe))
            {
                Process p = new Process();
                p.StartInfo.FileName = DebugExe;
                p.Start();
            }
        }

        private void mnuNewWin_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "CSharpPad.exe";
            p.Start();
        }

        private void mnuFind_Click(object sender, EventArgs e)
        {
            txtEd.ShowFindDialog();
        }

        private void mnuReplace_Click(object sender, EventArgs e)
        {
            txtEd.ShowReplaceDialog();
        }

        private void mnuGoto_Click(object sender, EventArgs e)
        {
            txtEd.ShowGoToDialog();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
