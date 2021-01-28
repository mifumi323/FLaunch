using System;
using System.Windows.Forms;
using FLaunch.Properties;

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
                foreach (var tag in value.tag)
                {
                    CheckTag(tag);
                }
            }
        }

        public string[] AllTags
        {
            set
            {
                clbTags.Items.Clear();
                clbTags.Items.AddRange(value);
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
            var tags = "";
            foreach (string tag in clbTags.CheckedItems)
            {
                tags = tags.Length > 0 ? $"{tags}{FLItem.sepalator}{tag}" : tag;
            }
            var newItem = new FLItem(txtName.Text, txtFile.Text, txtDir.Text, txtArguments.Text, txtComment.Text, tags)
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
                var sl = new ShortcutLink(txtFile.Text);
                txtFile.Text = sl.TargetPath;
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

        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            Text = $"{txtName.Text} のプロパティ";
        }

        private void txtNewTag_Leave(object sender, EventArgs e)
        {
            foreach (var tag in new FLItem("", "", "", "", "", txtNewTag.Text).tag)
            {
                CheckTag(tag);
            }
            txtNewTag.Text = "";
        }

        private void CheckTag(string tag)
        {
            var index = clbTags.Items.IndexOf(tag);
            if (index >= 0)
            {
                clbTags.SetItemChecked(index, true);
            }
            else
            {
                clbTags.Items.Add(tag, true);
            }
        }
    }
}
