using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FLaunch.Properties;
using util;

namespace FLaunch
{
    public partial class FormProperty : Form
    {
        private FLItem myItem;

        public FLItem Item
        {
            set
            {
                myItem = value;
                txtName.Text = value.name;
                txtFile.Text = value.file;
                txtArguments.Text = value.arguments;
                txtDir.Text = value.dir;
                txtComment.Text = value.comment;
                txtTag.Text = value.Tag;
            }
        }
	
        public FormProperty()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            FLItem newItem = new FLItem(txtName.Text, txtFile.Text, txtDir.Text, txtArguments.Text, txtComment.Text, txtTag.Text);
            newItem.score = myItem.score;
            newItem.date = myItem.date;
            FLData.Replace(myItem, newItem);
            Close();
        }

        private void txtFile_TextChanged(object sender, EventArgs e)
        {
            btnLink.Enabled = txtFile.Text.EndsWith(".lnk", StringComparison.CurrentCultureIgnoreCase);
        }

        private void btnLink_Click(object sender, EventArgs e)
        {
            try
            {
                ShellLink sl = new ShellLink(txtFile.Text);
                txtFile.Text = sl.Target;
                txtArguments.Text = sl.Arguments;
                txtDir.Text = sl.WorkingDirectory;
                if (txtComment.Text == "" && sl.Description != "") txtComment.Text = sl.Description;
                return;
            }
            catch (Exception) { }
        }

        private void FormProperty_Load(object sender, EventArgs e)
        {
            Icon = Resources.FLaunch;
        }
    }
}