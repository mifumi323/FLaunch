namespace FLaunch
{
    partial class FormProperty
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTag = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.lblComment = new System.Windows.Forms.Label();
            this.txtDir = new System.Windows.Forms.TextBox();
            this.lblDir = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.lblFile = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblArguments = new System.Windows.Forms.Label();
            this.txtArguments = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnLink = new System.Windows.Forms.Button();
            this.clbTags = new System.Windows.Forms.CheckedListBox();
            this.txtNewTag = new System.Windows.Forms.TextBox();
            this.lnlNewTag = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblTag, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtComment, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblComment, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtDir, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblDir, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtFile, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblFile, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblArguments, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtArguments, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.clbTags, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtNewTag, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.lnlNewTag, 0, 6);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(306, 208);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblTag
            // 
            this.lblTag.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTag.AutoSize = true;
            this.lblTag.Location = new System.Drawing.Point(30, 148);
            this.lblTag.Name = "lblTag";
            this.lblTag.Size = new System.Drawing.Size(37, 12);
            this.lblTag.TabIndex = 10;
            this.lblTag.Text = "タグ(&T)";
            // 
            // txtComment
            // 
            this.txtComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtComment.Location = new System.Drawing.Point(100, 103);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(203, 19);
            this.txtComment.TabIndex = 9;
            // 
            // lblComment
            // 
            this.lblComment.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(21, 106);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(54, 12);
            this.lblComment.TabIndex = 8;
            this.lblComment.Text = "コメント(&C)";
            // 
            // txtDir
            // 
            this.txtDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDir.Location = new System.Drawing.Point(100, 78);
            this.txtDir.Name = "txtDir";
            this.txtDir.Size = new System.Drawing.Size(203, 19);
            this.txtDir.TabIndex = 7;
            // 
            // lblDir
            // 
            this.lblDir.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblDir.AutoSize = true;
            this.lblDir.Location = new System.Drawing.Point(8, 81);
            this.lblDir.Name = "lblDir";
            this.lblDir.Size = new System.Drawing.Size(80, 12);
            this.lblDir.TabIndex = 6;
            this.lblDir.Text = "作業フォルダ(&D)";
            // 
            // txtFile
            // 
            this.txtFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFile.Location = new System.Drawing.Point(100, 28);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(203, 19);
            this.txtFile.TabIndex = 3;
            this.txtFile.TextChanged += new System.EventHandler(this.TxtFile_TextChanged);
            // 
            // lblFile
            // 
            this.lblFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(9, 31);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(78, 12);
            this.lblFile.TabIndex = 2;
            this.lblFile.Text = "実行ファイル(&F)";
            // 
            // lblName
            // 
            this.lblName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(20, 6);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(57, 12);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "表示名(&N)";
            // 
            // txtName
            // 
            this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtName.Location = new System.Drawing.Point(100, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(203, 19);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.TxtName_TextChanged);
            // 
            // lblArguments
            // 
            this.lblArguments.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblArguments.AutoSize = true;
            this.lblArguments.Location = new System.Drawing.Point(4, 56);
            this.lblArguments.Name = "lblArguments";
            this.lblArguments.Size = new System.Drawing.Size(89, 12);
            this.lblArguments.TabIndex = 4;
            this.lblArguments.Text = "パラメータ引数(&A)";
            // 
            // txtArguments
            // 
            this.txtArguments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtArguments.Location = new System.Drawing.Point(100, 53);
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.Size = new System.Drawing.Size(203, 19);
            this.txtArguments.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(243, 226);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(162, 226);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnLink
            // 
            this.btnLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLink.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnLink.Location = new System.Drawing.Point(12, 226);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(75, 23);
            this.btnLink.TabIndex = 1;
            this.btnLink.Text = "リンク解析";
            this.btnLink.UseVisualStyleBackColor = true;
            this.btnLink.Click += new System.EventHandler(this.BtnLink_Click);
            // 
            // clbTags
            // 
            this.clbTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbTags.FormattingEnabled = true;
            this.clbTags.Location = new System.Drawing.Point(100, 128);
            this.clbTags.MultiColumn = true;
            this.clbTags.Name = "clbTags";
            this.clbTags.Size = new System.Drawing.Size(203, 52);
            this.clbTags.TabIndex = 11;
            // 
            // txtNewTag
            // 
            this.txtNewTag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNewTag.Location = new System.Drawing.Point(100, 186);
            this.txtNewTag.Name = "txtNewTag";
            this.txtNewTag.Size = new System.Drawing.Size(203, 19);
            this.txtNewTag.TabIndex = 13;
            this.txtNewTag.Leave += new System.EventHandler(this.txtNewTag_Leave);
            // 
            // lnlNewTag
            // 
            this.lnlNewTag.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnlNewTag.AutoSize = true;
            this.lnlNewTag.Location = new System.Drawing.Point(3, 189);
            this.lnlNewTag.Name = "lnlNewTag";
            this.lnlNewTag.Size = new System.Drawing.Size(91, 12);
            this.lnlNewTag.TabIndex = 12;
            this.lnlNewTag.Text = "一覧にないタグ(&N)";
            // 
            // FormProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 261);
            this.Controls.Add(this.btnLink);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormProperty";
            this.Text = "腑呂派亭";
            this.Load += new System.EventHandler(this.FormProperty_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblTag;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox txtDir;
        private System.Windows.Forms.Label lblDir;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblArguments;
        private System.Windows.Forms.TextBox txtArguments;
        private System.Windows.Forms.Button btnLink;
        private System.Windows.Forms.CheckedListBox clbTags;
        private System.Windows.Forms.TextBox txtNewTag;
        private System.Windows.Forms.Label lnlNewTag;
    }
}
