using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using FLaunch.Properties;

namespace FLaunch
{
    public partial class Form1 : Form
    {
        bool bRunning = true;

        FLItem[] list;
        private FLItem mySelected;
        public FLItem Selected
        {
            get { return mySelected; }
            set
            {
                string tt = (mySelected = value) == null ? "" : value.comment;
                if (toolTip1.GetToolTip(panel1) != tt) toolTip1.SetToolTip(panel1, tt);
            }
        }

        FLOption option = null;

        Font font = new Font("Arial", 12);
        Brush brush = new SolidBrush(SystemColors.WindowText);
        Brush brushSel = new SolidBrush(SystemColors.Highlight);
        Brush brushSelText = new SolidBrush(SystemColors.HighlightText);

        Dictionary<string, Icon> icons = new Dictionary<string, Icon>();
        Queue<string> iconToRead = new Queue<string>();
        Size szIcon = new Size(16, 16);

        Comparison<FLItem> comparison;

        public Form1()
        {
            InitializeComponent();
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.Hide();
            }
            else
            {
                UpdateList();
                this.Location = Cursor.Position;
                if (this.Right > Screen.PrimaryScreen.WorkingArea.Right) this.Left -= this.Width;
                if (this.Bottom > Screen.PrimaryScreen.WorkingArea.Bottom) this.Top -= this.Height;
                if (this.Left < Screen.PrimaryScreen.WorkingArea.Left) this.Left = Screen.PrimaryScreen.WorkingArea.Left;
                if (this.Top < Screen.PrimaryScreen.WorkingArea.Top) this.Top = Screen.PrimaryScreen.WorkingArea.Top;
                this.Show();
                this.Activate();
            }
        }

        private void UpdateList()
        {
            list = FLData.GetArray();
            Array.Sort(list, comparison);
            UpdateScroll();
            panel1.Refresh();
        }

        int scoreComparison(FLItem x, FLItem y) { return y.score.CompareTo(x.score); }
        int dateComparison(FLItem x, FLItem y) { return y.date.CompareTo(x.date); }
        int nameComparison(FLItem x, FLItem y) { return x.name.CompareTo(y.name); }
        int fileComparison(FLItem x, FLItem y) { return x.file.CompareTo(y.file); }

        private void UpdateScroll()
        {
            try
            {
                float h = font.Height;
                vScrollBar1.Maximum = (int)(h * list.Length);
                vScrollBar1.SmallChange = 1;
                vScrollBar1.LargeChange = Math.Max(panel1.ClientRectangle.Height - 18, 1);
            }
            catch (Exception) { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            option = new FLOption();
            if (option.Width > 0) Width = option.Width;
            if (option.Height > 0) Height = option.Height;
            comparison = scoreComparison;
            notifyIcon1.Icon = this.Icon;
            panel1.SetBounds(0, menuStrip1.Height, this.ClientSize.Width - vScrollBar1.Width, this.ClientSize.Height - menuStrip1.Height);
            vScrollBar1.SetBounds(panel1.Width, panel1.Top, vScrollBar1.Width, panel1.Height);

            Icon = Resources.FLaunch;
            notifyIcon1.Icon = Icon;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bRunning = false;
            this.Close();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
            this.Opacity = 1.0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bRunning && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                notifyIcon1.Visible = false;
            }
            option.Save();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            this.Hide();
            option.Save();
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            vScrollBar1.Value = Math.Min(Math.Max(
                vScrollBar1.Minimum,
                vScrollBar1.Value - e.Delta),
                vScrollBar1.Maximum - vScrollBar1.LargeChange + 1);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            int i = (e.Y + vScrollBar1.Value + font.Height) / font.Height - 1;
            if (0 <= i && i < list.Length) Selected = list[i];
            else Selected = null;
            panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            PointF point = new PointF(18.0f, (float)-vScrollBar1.Value);
            float h = font.Height;
            foreach (FLItem item in list)
            {
                if (-h < point.Y && point.Y < panel1.ClientRectangle.Height)
                {
                    if (item == Selected) g.FillRectangle(brushSel, 0.0f, point.Y, (float)this.ClientRectangle.Width, h);
                    Icon icon = GetIcon(item.file);
                    if (icon != null) g.DrawIcon(icon, new Rectangle(1, (int)point.Y + 1, 16, 16));
                    g.DrawString(item.name, font, item != Selected ? brush : brushSelText, point);
                }
                point.Y += h;
            }
        }

        private Icon GetIcon(string file)
        {
            lock (this)
            {
                if (icons.ContainsKey(file)) return icons[file];
                iconToRead.Enqueue(file);
                if (!backgroundWorker1.IsBusy) backgroundWorker1.RunWorkerAsync();
                return null;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) Execute();
            else if (e.Button == MouseButtons.Right) ShowMenu();
        }

        private void ShowMenu()
        {
            if (Selected == null) return;
            contextMenuStrip1.Show(Cursor.Position);
        }

        private void Execute()
        {
            if (Selected == null) return;
            // この順番でないと自身をパラメータつきで呼び出したときまずい
            this.Hide();
            if (!File.Exists(Selected.file))
            {
                MessageBox.Show(
                    Selected.name + " のファイル\r\n " + Selected.file + "\r\nが見つかりませんでした。",
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FLData.Score(Selected);
            try { Directory.SetCurrentDirectory(Selected.dir); }
            catch (Exception) { }
            Process.Start(Selected.file, Selected.arguments);
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            if (contextMenuStrip1.Visible) return;
            Selected = null;
            panel1.Refresh();
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        { panel1.Refresh(); }

        private void panel1_Resize(object sender, EventArgs e)
        {
            UpdateScroll();
            if (option != null)
            {
                option.Width = Width;
                option.Height = Height;
            }
        }

        private void scoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comparison = scoreComparison;
            UpdateList();
        }
        private void dateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comparison = dateComparison;
            UpdateList();
        }
        private void nameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comparison = nameComparison;
            UpdateList();
        }
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comparison = fileComparison;
            UpdateList();
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        { Execute(); }

        private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            //selected = null;
            //panel1.Refresh();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        { Delete(); }
        private void Delete()
        {
            if (Selected == null) return;
            FLData.Delete(Selected);
            UpdateList();
        }

        private void openDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Selected == null) return;
            this.Hide();
            if (Selected.dir == "")
            {
                MessageBox.Show("作業フォルダがありません。");
                return;
            }
            try
            {
                Process.Start(Selected.dir);
            }
            catch (Exception ex)
            {
                MessageBox.Show("作業フォルダを開けませんでした。\n" + Selected.dir + "\n\n" + ex.Message);
            }
        }

        private void propertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProperty fp = new FormProperty();
            fp.Item = Selected;
            fp.Show();
        }

        private void optionToolStripMenuItem_Click(object sender, EventArgs e)
        { MessageBox.Show("未実装！！"); }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Rectangle rect = new Rectangle(0,0,szIcon.Width, szIcon.Height);
            string file;
            int count;
            bool read;
            Icon icon = null;
            lock (this) count = iconToRead.Count;
            while (count > 0)
            {
                lock (this)
                {
                    file = iconToRead.Dequeue();
                    read = !icons.ContainsKey(file);
                }
                if (read)
                {
                    try { icon = Icon.ExtractAssociatedIcon(file); }
                    catch (Exception) { icon = null; }
                }
                lock (this)
                {
                    if (read) icons.Add(file, icon);
                    count = iconToRead.Count;
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.Visible) panel1.Refresh();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Application.ProductName + "\n" + Application.ProductVersion);
        }

    }
}