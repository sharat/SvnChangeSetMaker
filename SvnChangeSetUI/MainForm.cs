using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibSvnChangeSet;

namespace SvnChangeSet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            listView1.Columns.Add("Path", 600);
        }


        private void buttonCheckModifications_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            SvnHelper helper = new SvnHelper();
            string localPath = @"";
            helper.getModifiedListAsync(localPath, cb_progress, cb_Completed);

        }

        void cb_progress(object sender, ProgressEventArgs e)
        {
            listView1.Items.Add(e.FileName);
        }

        void cb_Completed(object sender, CompletedEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void buttonZip_Click(object sender, EventArgs e)
        {
            SvnHelper helper = new SvnHelper();
            string localPath = @"";
            List<string> files = helper.getModifiedFilePaths(localPath);
            helper.createChangeList(files,localPath, @"C:\temp\cs");
            SvnChangeSetHelper.zipChangeSetDir(@"C:\temp\cs", @"C:\temp\cs\ChangeSet.zip");
        }
    }
}
