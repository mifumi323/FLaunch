using System;
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

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            FLItem newItem = new FLItem(txtName.Text, txtFile.Text, txtDir.Text, txtArguments.Text, txtComment.Text, txtTag.Text)
            {
                score = myItem.score,
                date = myItem.date
            };
            FLData.Replace(myItem, newItem);
            Close();
        }

        private void TxtFile_TextChanged(object sender, EventArgs e)
        {
            btnLink.Enabled = txtFile.Text.EndsWith(".lnk", StringComparison.CurrentCultureIgnoreCase);
        }

        private void BtnLink_Click(object sender, EventArgs e)
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
