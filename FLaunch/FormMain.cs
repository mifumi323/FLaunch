using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using FLaunch.Properties;

namespace FLaunch
{
    public partial class FormMain : Form
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

        public FormMain()
        {
            InitializeComponent();
        }

        private void NotifyIcon1_Click(object sender, EventArgs e)
        {
            if (Visible)
            {
                Hide();
            }
            else
            {
                UpdateList();
                Location = Cursor.Position;
                if (Right > Screen.PrimaryScreen.WorkingArea.Right) Left -= Width;
                if (Bottom > Screen.PrimaryScreen.WorkingArea.Bottom) Top -= Height;
                if (Left < Screen.PrimaryScreen.WorkingArea.Left) Left = Screen.PrimaryScreen.WorkingArea.Left;
                if (Top < Screen.PrimaryScreen.WorkingArea.Top) Top = Screen.PrimaryScreen.WorkingArea.Top;
                Show();
                Activate();
            }
        }

        private void UpdateList()
        {
            list = FLData.GetArray();
            Array.Sort(list, comparison);
            UpdateScroll();
            panel1.Refresh();
        }

        int ScoreComparison(FLItem x, FLItem y) => y.score.CompareTo(x.score);
        int DateComparison(FLItem x, FLItem y) => y.date.CompareTo(x.date);
        int NameComparison(FLItem x, FLItem y) => x.name.CompareTo(y.name);
        int FileComparison(FLItem x, FLItem y) => x.file.CompareTo(y.file);

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
            comparison = ScoreComparison;
            UpdateSortMenu(scoreToolStripMenuItem);
            notifyIcon1.Icon = Icon;
            panel1.SetBounds(0, menuStrip1.Height, ClientSize.Width - vScrollBar1.Width, ClientSize.Height - menuStrip1.Height);
            vScrollBar1.SetBounds(panel1.Width, panel1.Top, vScrollBar1.Width, panel1.Height);

            Icon = Resources.FLaunch;
            notifyIcon1.Icon = Icon;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bRunning = false;
            Close();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Hide();
            Opacity = 1.0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bRunning && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                notifyIcon1.Visible = false;
            }
            option.Save();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            Hide();
            option.Save();
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            vScrollBar1.Value = Math.Min(Math.Max(
                vScrollBar1.Minimum,
                vScrollBar1.Value - e.Delta),
                vScrollBar1.Maximum - vScrollBar1.LargeChange + 1);
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            int i = (e.Y + vScrollBar1.Value + font.Height) / font.Height - 1;
            if (0 <= i && i < list.Length) Selected = list[i];
            else Selected = null;
            panel1.Refresh();
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            PointF point = new PointF(18.0f, (float)-vScrollBar1.Value);
            float h = font.Height;
            foreach (FLItem item in list)
            {
                if (-h < point.Y && point.Y < panel1.ClientRectangle.Height)
                {
                    if (item == Selected) g.FillRectangle(brushSel, 0.0f, point.Y, ClientRectangle.Width, h);
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

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
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
            Hide();
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

        private void Panel1_MouseLeave(object sender, EventArgs e)
        {
            if (contextMenuStrip1.Visible) return;
            Selected = null;
            panel1.Refresh();
        }

        private void VScrollBar1_ValueChanged(object sender, EventArgs e) => panel1.Refresh();

        private void Panel1_Resize(object sender, EventArgs e)
        {
            UpdateScroll();
            if (option != null)
            {
                option.Width = Width;
                option.Height = Height;
            }
        }

        private void ScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comparison = ScoreComparison;
            UpdateSortMenu(sender);
            UpdateList();
        }
        private void DateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comparison = DateComparison;
            UpdateSortMenu(sender);
            UpdateList();
        }
        private void NameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comparison = NameComparison;
            UpdateSortMenu(sender);
            UpdateList();
        }
        private void FileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comparison = FileComparison;
            UpdateSortMenu(sender);
            UpdateList();
        }

        private void UpdateSortMenu(object sender)
        {
            foreach (var menu in new ToolStripMenuItem[] { scoreToolStripMenuItem, dateToolStripMenuItem, nameToolStripMenuItem, fileToolStripMenuItem })
            {
                menu.Checked = menu == sender;
            }
        }

        private void ExecuteToolStripMenuItem_Click(object sender, EventArgs e) => Execute();

        private void ContextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            //selected = null;
            //panel1.Refresh();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e) => Delete();
        private void Delete()
        {
            if (Selected == null) return;
            FLData.Delete(Selected);
            UpdateList();
        }

        private void OpenDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Selected == null) return;
            Hide();
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

        private void PropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormProperty
            {
                Item = Selected
            }.Show();
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Rectangle rect = new Rectangle(0,0,szIcon.Width, szIcon.Height);
            int count;
            Icon icon = null;
            lock (this) count = iconToRead.Count;
            while (count > 0)
            {
                string file;
                bool read;
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

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Visible) panel1.Refresh();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Application.ProductName + "\n" + Application.ProductVersion);
        }

    }
}