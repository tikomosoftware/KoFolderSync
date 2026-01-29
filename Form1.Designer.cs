namespace FolderSync
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelSource = new Label();
            textBoxSource = new TextBox();
            buttonBrowseSource = new Button();
            labelDestination = new Label();
            textBoxDestination = new TextBox();
            buttonBrowseDestination = new Button();
            groupBoxOptions = new GroupBox();
            checkBoxDryRun = new CheckBox();
            checkBoxDeleteExtra = new CheckBox();
            checkBoxAlwaysOnTop = new CheckBox();
            buttonSync = new Button();
            buttonCancel = new Button();
            progressBar = new ProgressBar();
            labelLog = new Label();
            buttonClearLog = new Button();
            textBoxLog = new TextBox();
            groupBoxOptions.SuspendLayout();
            SuspendLayout();
            // 
            // labelSource
            // 
            labelSource.AutoSize = true;
            labelSource.Location = new Point(23, 25);
            labelSource.Name = "labelSource";
            labelSource.Size = new Size(106, 19);
            labelSource.TabIndex = 0;
            labelSource.Text = "ソースフォルダ (A):";
            // 
            // textBoxSource
            // 
            textBoxSource.AllowDrop = true;
            textBoxSource.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxSource.Location = new Point(23, 57);
            textBoxSource.Margin = new Padding(3, 4, 3, 4);
            textBoxSource.Name = "textBoxSource";
            textBoxSource.ReadOnly = true;
            textBoxSource.Size = new Size(388, 26);
            textBoxSource.TabIndex = 1;
            textBoxSource.DragDrop += textBoxSource_DragDrop;
            textBoxSource.DragEnter += textBoxSource_DragEnter;
            // 
            // buttonBrowseSource
            // 
            buttonBrowseSource.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonBrowseSource.Location = new Point(417, 57);
            buttonBrowseSource.Margin = new Padding(3, 4, 3, 4);
            buttonBrowseSource.Name = "buttonBrowseSource";
            buttonBrowseSource.Size = new Size(137, 32);
            buttonBrowseSource.TabIndex = 2;
            buttonBrowseSource.Text = "参照...";
            buttonBrowseSource.UseVisualStyleBackColor = true;
            buttonBrowseSource.Click += buttonBrowseSource_Click;
            // 
            // labelDestination
            // 
            labelDestination.AutoSize = true;
            labelDestination.Location = new Point(23, 108);
            labelDestination.Name = "labelDestination";
            labelDestination.Size = new Size(117, 19);
            labelDestination.TabIndex = 3;
            labelDestination.Text = "同期先フォルダ (B):";
            // 
            // textBoxDestination
            // 
            textBoxDestination.AllowDrop = true;
            textBoxDestination.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxDestination.Location = new Point(23, 139);
            textBoxDestination.Margin = new Padding(3, 4, 3, 4);
            textBoxDestination.Name = "textBoxDestination";
            textBoxDestination.ReadOnly = true;
            textBoxDestination.Size = new Size(388, 26);
            textBoxDestination.TabIndex = 4;
            textBoxDestination.DragDrop += textBoxDestination_DragDrop;
            textBoxDestination.DragEnter += textBoxDestination_DragEnter;
            // 
            // buttonBrowseDestination
            // 
            buttonBrowseDestination.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonBrowseDestination.Location = new Point(417, 135);
            buttonBrowseDestination.Margin = new Padding(3, 4, 3, 4);
            buttonBrowseDestination.Name = "buttonBrowseDestination";
            buttonBrowseDestination.Size = new Size(137, 32);
            buttonBrowseDestination.TabIndex = 5;
            buttonBrowseDestination.Text = "参照...";
            buttonBrowseDestination.UseVisualStyleBackColor = true;
            buttonBrowseDestination.Click += buttonBrowseDestination_Click;
            // 
            // groupBoxOptions
            // 
            groupBoxOptions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxOptions.Controls.Add(checkBoxDryRun);
            groupBoxOptions.Controls.Add(checkBoxDeleteExtra);
            groupBoxOptions.Location = new Point(23, 175);
            groupBoxOptions.Name = "groupBoxOptions";
            groupBoxOptions.Size = new Size(531, 90);
            groupBoxOptions.TabIndex = 14;
            groupBoxOptions.TabStop = false;
            groupBoxOptions.Text = "オプション";
            // 
            // checkBoxAlwaysOnTop
            // 
            checkBoxAlwaysOnTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkBoxAlwaysOnTop.AutoSize = true;
            checkBoxAlwaysOnTop.Location = new Point(444, 25);
            checkBoxAlwaysOnTop.Name = "checkBoxAlwaysOnTop";
            checkBoxAlwaysOnTop.Size = new Size(110, 23);
            checkBoxAlwaysOnTop.TabIndex = 15;
            checkBoxAlwaysOnTop.Text = "最前面表示";
            checkBoxAlwaysOnTop.UseVisualStyleBackColor = true;
            checkBoxAlwaysOnTop.CheckedChanged += checkBoxAlwaysOnTop_CheckedChanged;
            // 
            // checkBoxDryRun
            // 
            checkBoxDryRun.AutoSize = true;
            checkBoxDryRun.ForeColor = Color.DarkBlue;
            checkBoxDryRun.Location = new Point(15, 55);
            checkBoxDryRun.Margin = new Padding(3, 4, 3, 4);
            checkBoxDryRun.Name = "checkBoxDryRun";
            checkBoxDryRun.Size = new Size(340, 23);
            checkBoxDryRun.TabIndex = 1;
            checkBoxDryRun.Text = "テストモード（実際のファイル操作を行わない）";
            checkBoxDryRun.UseVisualStyleBackColor = true;
            // 
            // checkBoxDeleteExtra
            // 
            checkBoxDeleteExtra.AutoSize = true;
            checkBoxDeleteExtra.Location = new Point(15, 25);
            checkBoxDeleteExtra.Margin = new Padding(3, 4, 3, 4);
            checkBoxDeleteExtra.Name = "checkBoxDeleteExtra";
            checkBoxDeleteExtra.Size = new Size(340, 23);
            checkBoxDeleteExtra.TabIndex = 0;
            checkBoxDeleteExtra.Text = "同期先にのみ存在するファイルを削除する（完全同期）";
            checkBoxDeleteExtra.UseVisualStyleBackColor = true;
            // 
            // buttonSync
            // 
            buttonSync.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            buttonSync.BackColor = Color.FromArgb(0, 150, 136);
            buttonSync.FlatStyle = FlatStyle.Flat;
            buttonSync.Font = new Font("Yu Gothic UI", 11F, FontStyle.Bold);
            buttonSync.ForeColor = Color.White;
            buttonSync.Location = new Point(23, 275);
            buttonSync.Margin = new Padding(3, 4, 3, 4);
            buttonSync.Name = "buttonSync";
            buttonSync.Size = new Size(400, 51);
            buttonSync.TabIndex = 6;
            buttonSync.Text = "同期開始";
            buttonSync.UseVisualStyleBackColor = false;
            buttonSync.Click += buttonSync_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonCancel.BackColor = SystemColors.Control;
            buttonCancel.Enabled = false;
            buttonCancel.FlatStyle = FlatStyle.Flat;
            buttonCancel.Font = new Font("Yu Gothic UI", 11F, FontStyle.Bold);
            buttonCancel.ForeColor = SystemColors.ControlText;
            buttonCancel.Location = new Point(429, 275);
            buttonCancel.Margin = new Padding(3, 4, 3, 4);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(125, 51);
            buttonCancel.TabIndex = 12;
            buttonCancel.Text = "停止";
            buttonCancel.UseVisualStyleBackColor = false;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(23, 344);
            progressBar.Margin = new Padding(3, 4, 3, 4);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(531, 32);
            progressBar.TabIndex = 7;
            // 
            // labelLog
            // 
            labelLog.AutoSize = true;
            labelLog.Location = new Point(23, 395);
            labelLog.Name = "labelLog";
            labelLog.Size = new Size(33, 19);
            labelLog.TabIndex = 8;
            labelLog.Text = "ログ:";
            // 
            // buttonClearLog
            // 
            buttonClearLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonClearLog.BackColor = Color.FromArgb(59, 130, 246);
            buttonClearLog.FlatStyle = FlatStyle.Flat;
            buttonClearLog.ForeColor = Color.White;
            buttonClearLog.Location = new Point(463, 388);
            buttonClearLog.Margin = new Padding(3, 4, 3, 4);
            buttonClearLog.Name = "buttonClearLog";
            buttonClearLog.Size = new Size(91, 32);
            buttonClearLog.TabIndex = 11;
            buttonClearLog.Text = "クリア";
            buttonClearLog.UseVisualStyleBackColor = false;
            buttonClearLog.Click += buttonClearLog_Click;
            // 
            // textBoxLog
            // 
            textBoxLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxLog.Location = new Point(23, 428);
            textBoxLog.Margin = new Padding(3, 4, 3, 4);
            textBoxLog.Multiline = true;
            textBoxLog.Name = "textBoxLog";
            textBoxLog.ReadOnly = true;
            textBoxLog.ScrollBars = ScrollBars.Vertical;
            textBoxLog.Size = new Size(531, 73);
            textBoxLog.TabIndex = 9;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 520);
            Controls.Add(checkBoxAlwaysOnTop);
            Controls.Add(buttonClearLog);
            Controls.Add(groupBoxOptions);
            Controls.Add(textBoxLog);
            Controls.Add(labelLog);
            Controls.Add(progressBar);
            Controls.Add(buttonCancel);
            Controls.Add(buttonSync);
            Controls.Add(buttonBrowseDestination);
            Controls.Add(textBoxDestination);
            Controls.Add(labelDestination);
            Controls.Add(buttonBrowseSource);
            Controls.Add(textBoxSource);
            Controls.Add(labelSource);
            LoadIconSafely();
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = true;
            MinimumSize = new Size(600, 560);
            Name = "Form1";
            SizeGripStyle = SizeGripStyle.Show;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "KoFolderSync v1.0.1";
            Load += Form1_Load;
            Resize += Form1_Resize;
            groupBoxOptions.ResumeLayout(false);
            groupBoxOptions.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelSource;
        private TextBox textBoxSource;
        private Button buttonBrowseSource;
        private Label labelDestination;
        private TextBox textBoxDestination;
        private Button buttonBrowseDestination;
        private GroupBox groupBoxOptions;
        private CheckBox checkBoxDeleteExtra;
        private CheckBox checkBoxDryRun;
        private CheckBox checkBoxAlwaysOnTop;
        private Button buttonSync;
        private Button buttonCancel;
        private ProgressBar progressBar;
        private TextBox textBoxLog;
        private Label labelLog;
        private Button buttonClearLog;
    }
}
