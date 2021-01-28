using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FLaunch.Properties;

namespace FLaunch
{
    public partial class FormMain : Form
    {
        bool bRunning = true;
        private bool deleteOnExit = false;

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

        private readonly Font font = new Font("Arial", 12);
        private readonly Brush brush = new SolidBrush(SystemColors.WindowText);
        private readonly Brush brushSel = new SolidBrush(SystemColors.Highlight);
        private readonly Brush brushSelText = new SolidBrush(SystemColors.HighlightText);

        readonly Dictionary<string, Icon> icons = new Dictionary<string, Icon>();
        readonly Queue<string> iconToRead = new Queue<string>();

        Comparison<FLItem> comparison;

        private IntPtr activeOtherWindow = IntPtr.Zero;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public FormMain()
        {
            InitializeComponent();
        }

        private void NotifyIcon1_Click(object sender, EventArgs e)
        {
            if (Visible)
            {
                HideForm();
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

                var activeWindow = GetForegroundWindow();
                if (activeWindow == Handle)
                {
                    activeOtherWindow = IntPtr.Zero;
                }
                else
                {
                    activeOtherWindow = activeWindow;
                    timer1.Start();
                }
            }
        }

        private void UpdateList()
        {
            var condition = tscbFilter.Text.ToLower();
            list = FLData.Get().Where(item => Filter(item, condition)).ToArray();
            Array.Sort(list, comparison);
            UpdateScroll();
            panel1.Refresh();
        }

        private bool Filter(FLItem item, string condition)
        {
            if (condition == "")
            {
                return true;
            }
            if (condition[0] == '#')
            {
                return item.tag.Any(tag => tag.ToLower() == condition.Substring(1));
            }
            return item.name.ToLower().Contains(condition);
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
            // サイズ等の初期設定
            option = new FLOption();
            var width = option.Width;
            var height = option.Height;
            if (width > 0) Width = width;
            if (height > 0) Height = height;
            panel1.SetBounds(0, menuStrip1.Height, ClientSize.Width - vScrollBar1.Width, ClientSize.Height - menuStrip1.Height);
            vScrollBar1.SetBounds(panel1.Width, panel1.Top, vScrollBar1.Width, panel1.Height);

            // 並べ替え
            comparison = ScoreComparison;
            UpdateSortMenu(scoreToolStripMenuItem);

            // 見た目の設定
            Icon = Resources.FLaunch;
            notifyIcon1.Icon = Icon;
            notifyIcon1.Text = Program.Title;
            notifyIcon1.Visible = true;
            fLaunchToolStripMenuItem.Text = Program.Title;
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
                HideForm();
            }
            else
            {
                notifyIcon1.Visible = false;
                if (deleteOnExit)
                {
                    File.Delete(FLOption.FileName);
                    File.Delete(FLData.FileName);
                    if (!Directory.EnumerateFileSystemEntries(Program.UserAppDataPath).Any())
                    {
                        Directory.Delete(Program.UserAppDataPath);
                    }
                    return;
                }
            }
            option.Save();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            HideForm();
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
                    Icon icon = GetIcon(AutoExpandEnvironmentVariables(item.file));
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

        private void Execute(bool runAs = false)
        {
            var selected = Selected;
            if (selected == null) return;
            // この順番でないと自身をパラメータつきで呼び出したときまずい
            HideForm();
            try { Directory.SetCurrentDirectory(AutoExpandEnvironmentVariables(selected.dir)); }
            catch (Exception) { }
            using var process = new Process();
            var psi = process.StartInfo;
            psi.FileName = AutoExpandEnvironmentVariables(selected.file);
            psi.Arguments = AutoExpandEnvironmentVariables(selected.arguments);
            if (runAs)
            {
                psi.Verb = "RunAs";
            }
            try
            {
                process.Start();
                FLData.Score(selected);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    $"{selected.name} は実行できませんでした。",
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            HideForm();
            if (Selected.dir == "")
            {
                MessageBox.Show("作業フォルダがありません。");
                return;
            }
            var dir = AutoExpandEnvironmentVariables(Selected.dir);
            try
            {
                Process.Start(dir);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"作業フォルダを開けませんでした。\n{dir}\n\n{ex.Message}");
            }
        }

        private void PropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormProperty
            {
                AllTags = list.SelectMany(item => item.tag).Distinct().OrderBy(tag => tag).ToArray(),
                Item = Selected,
            }.Show();
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
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
            var message = $"{Program.Title}\nVersion: {Application.ProductVersion}\n{Program.Copyright}";
            var title = $"{Program.Title}のバージョン情報";
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var helpFile = Path.Combine(Application.StartupPath, @"FLaunch.txt");
            if (!File.Exists(helpFile))
            {
                var message = "取扱説明書が見つかりませんでした。\nがんばって気合いで使い方をマスターしてください＞＜";
                var title = $"{Program.Title}の取扱説明書";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Process.Start(helpFile);
        }

        private void RunAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Execute(true);
        }

        private void OpenFileDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Selected == null) return;
            HideForm();
            try
            {
                var dir = Path.GetDirectoryName(AutoExpandEnvironmentVariables(Selected.file));
                Process.Start(dir);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ファイルの場所を開けませんでした。\n{Selected.file}\n\n{ex.Message}");
            }
        }

        private void ExpandEnvironmentVariablesToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            expandEnvironmentVariablesOnToolStripMenuItem.Checked = option.ExpandEnvironmentVariables;
            expandEnvironmentVariablesOffToolStripMenuItem.Checked = !option.ExpandEnvironmentVariables;
            expandEnvironmentVariablesOnToolStripMenuItem.ToolTipText = option.ExpandEnvironmentVariables ?
                "環境変数の展開は現在有効です" :
                "環境変数の展開を有効にします";
            expandEnvironmentVariablesOffToolStripMenuItem.ToolTipText = !option.ExpandEnvironmentVariables ?
                "環境変数の展開は現在無効です" :
                "環境変数の展開を無効にします";
        }

        private void ExpandEnvironmentVariablesOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            option.ExpandEnvironmentVariables = true;
        }

        private void ExpandEnvironmentVariablesOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            option.ExpandEnvironmentVariables = false;
        }

        private string AutoExpandEnvironmentVariables(string path)
        {
            return option.ExpandEnvironmentVariables ?
                Environment.ExpandEnvironmentVariables(path)
                : path;
        }

        private void ExitAndDeleteDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string text = "データを消して終了します。\nリストの内容はなくなり、元に戻せませんが、よろしいですか？";
            const string caption = "データを消して終了";
            if (MessageBox.Show(this, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            bRunning = false;
            deleteOnExit = true;
            Close();
        }

        private void ExportListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog()
            {
                DefaultExt = "tsv",
                FileName = Path.GetFileName(FLData.FileName),
                Filter = "タブ区切りテキスト|*.tsv|その他|*.*",
                Title = "エクスポート",
            };
            if (sfd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            try
            {
                File.Copy(FLData.FileName, sfd.FileName, true);
            }
            catch (Exception)
            {
                MessageBox.Show(this, "エクスポートに失敗しました。", "エクスポート", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog()
            {
                FileName = Path.GetFileName(FLData.FileName),
                Filter = "タブ区切りテキスト|*.tsv|その他|*.*",
                Title = "インポート",
            };
            if (ofd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            var merge = false;
            if (FLData.Get().Any())
            {
                const string text = "既にあるリストと統合しますか？\n"
                    + "[はい] 今あるリストを残したまま新しいリストを追加します。\n"
                    + "[いいえ] 今あるリストを消して新しいリストに入れ替えます。\n"
                    + "[キャンセル] インポートを取り消し、今あるリストは何も変わりません。";
                const string caption = "インポート";
                var result = MessageBox.Show(this, text, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    return;
                }
                merge = result == DialogResult.Yes;
            }
            try
            {
                if (merge)
                {
                    FLData.Merge(ofd.FileName);
                }
                else
                {
                    File.Copy(ofd.FileName, FLData.FileName, true);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this, "インポートに失敗しました。", "インポート", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            exportListToolStripMenuItem.Enabled = FLData.Get().Any();
        }

        private void TscbFilter_TextChanged(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog()
            {
                Filter = "実行ファイル(*.exe)|*.exe|ショートカット(*.lnk)|*.lnk|すべてのファイル(*.*)|*.*",
            };
            if (ofd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            var item = FLData.Add(ofd.FileName);
            new FormProperty
            {
                Item = item
            }.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Visible || ActiveForm != null)
            {
                timer1.Stop();
                return;
            }
            if (GetForegroundWindow() != activeOtherWindow)
            {
                HideForm();
            }
        }

        private void HideForm()
        {
                timer1.Stop();
                Hide();
                activeOtherWindow = IntPtr.Zero;
        }
    }
}
