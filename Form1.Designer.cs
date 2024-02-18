namespace Snowrunner_Patcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            ModVersionLabel = new Label();
            menuStrip1 = new MenuStrip();
            configToolStripMenuItem = new ToolStripMenuItem();
            changeModPathToolStripMenuItem = new ToolStripMenuItem();
            openConfigFileToolStripMenuItem = new ToolStripMenuItem();
            openModDirectoryToolStripMenuItem = new ToolStripMenuItem();
            modPakToolStripMenuItem = new ToolStripMenuItem();
            advancedPatchingToolStripMenuItem = new ToolStripMenuItem();
            openBackupsToolStripMenuItem = new ToolStripMenuItem();
            createBackupToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            deleteAllToolStripMenuItem = new ToolStripMenuItem();
            replaceBackupToolStripMenuItem = new ToolStripMenuItem();
            lastBackupToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            deleteModPakToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            forceInstallToolStripMenuItem = new ToolStripMenuItem();
            logsToolStripMenuItem = new ToolStripMenuItem();
            openCurrentLogToolStripMenuItem = new ToolStripMenuItem();
            openFolderLogsToolStripMenuItem = new ToolStripMenuItem();
            StatusInfo = new StatusStrip();
            VersionAppLabel = new ToolStripStatusLabel();
            toolStripStatusInfo = new ToolStripStatusLabel();
            toolStripStatusLabelInfoPatch = new ToolStripStatusLabel();
            ProgressBar = new ToolStripProgressBar();
            groupBox1 = new GroupBox();
            CurrentVersionLabel = new Label();
            LastVersionLabel = new Label();
            UpdateModButton = new Button();
            menuStrip1.SuspendLayout();
            StatusInfo.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // ModVersionLabel
            // 
            ModVersionLabel.AutoSize = true;
            ModVersionLabel.Location = new Point(6, 19);
            ModVersionLabel.Name = "ModVersionLabel";
            ModVersionLabel.Size = new Size(122, 15);
            ModVersionLabel.TabIndex = 0;
            ModVersionLabel.Text = "Last Version Installed: ";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { configToolStripMenuItem, modPakToolStripMenuItem, logsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(359, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // configToolStripMenuItem
            // 
            configToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { changeModPathToolStripMenuItem, openConfigFileToolStripMenuItem, openModDirectoryToolStripMenuItem });
            configToolStripMenuItem.Name = "configToolStripMenuItem";
            configToolStripMenuItem.Size = new Size(55, 20);
            configToolStripMenuItem.Text = "Config";
            // 
            // changeModPathToolStripMenuItem
            // 
            changeModPathToolStripMenuItem.Name = "changeModPathToolStripMenuItem";
            changeModPathToolStripMenuItem.Size = new Size(182, 22);
            changeModPathToolStripMenuItem.Text = "Change Mod Path";
            changeModPathToolStripMenuItem.Click += changeModPathToolStripMenuItem_Click;
            // 
            // openConfigFileToolStripMenuItem
            // 
            openConfigFileToolStripMenuItem.Name = "openConfigFileToolStripMenuItem";
            openConfigFileToolStripMenuItem.Size = new Size(182, 22);
            openConfigFileToolStripMenuItem.Text = "Open Config File";
            openConfigFileToolStripMenuItem.Click += openConfigFileToolStripMenuItem_Click;
            // 
            // openModDirectoryToolStripMenuItem
            // 
            openModDirectoryToolStripMenuItem.Name = "openModDirectoryToolStripMenuItem";
            openModDirectoryToolStripMenuItem.Size = new Size(182, 22);
            openModDirectoryToolStripMenuItem.Text = "Open Mod Directory";
            openModDirectoryToolStripMenuItem.Click += openModDirectoryToolStripMenuItem_Click;
            // 
            // modPakToolStripMenuItem
            // 
            modPakToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { advancedPatchingToolStripMenuItem, openBackupsToolStripMenuItem, toolStripSeparator1, deleteModPakToolStripMenuItem, toolStripSeparator2, forceInstallToolStripMenuItem });
            modPakToolStripMenuItem.Name = "modPakToolStripMenuItem";
            modPakToolStripMenuItem.Size = new Size(63, 20);
            modPakToolStripMenuItem.Text = "ModPak";
            // 
            // advancedPatchingToolStripMenuItem
            // 
            advancedPatchingToolStripMenuItem.Name = "advancedPatchingToolStripMenuItem";
            advancedPatchingToolStripMenuItem.Size = new Size(180, 22);
            advancedPatchingToolStripMenuItem.Text = "Advanced Patching";
            advancedPatchingToolStripMenuItem.Click += advancedPatchingToolStripMenuItem_Click;
            // 
            // openBackupsToolStripMenuItem
            // 
            openBackupsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { createBackupToolStripMenuItem, openToolStripMenuItem, deleteAllToolStripMenuItem, replaceBackupToolStripMenuItem });
            openBackupsToolStripMenuItem.Name = "openBackupsToolStripMenuItem";
            openBackupsToolStripMenuItem.Size = new Size(180, 22);
            openBackupsToolStripMenuItem.Text = "Backups";
            // 
            // createBackupToolStripMenuItem
            // 
            createBackupToolStripMenuItem.Name = "createBackupToolStripMenuItem";
            createBackupToolStripMenuItem.Size = new Size(186, 22);
            createBackupToolStripMenuItem.Text = "Create Backup";
            createBackupToolStripMenuItem.Click += createBackupToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(186, 22);
            openToolStripMenuItem.Text = "Open Backups Folder";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // deleteAllToolStripMenuItem
            // 
            deleteAllToolStripMenuItem.Name = "deleteAllToolStripMenuItem";
            deleteAllToolStripMenuItem.Size = new Size(186, 22);
            deleteAllToolStripMenuItem.Text = "Delete All Backups";
            deleteAllToolStripMenuItem.Click += deleteAllToolStripMenuItem_Click;
            // 
            // replaceBackupToolStripMenuItem
            // 
            replaceBackupToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { lastBackupToolStripMenuItem });
            replaceBackupToolStripMenuItem.Name = "replaceBackupToolStripMenuItem";
            replaceBackupToolStripMenuItem.Size = new Size(186, 22);
            replaceBackupToolStripMenuItem.Text = "Replace Backup";
            // 
            // lastBackupToolStripMenuItem
            // 
            lastBackupToolStripMenuItem.Name = "lastBackupToolStripMenuItem";
            lastBackupToolStripMenuItem.Size = new Size(180, 22);
            lastBackupToolStripMenuItem.Text = "Last Backup";
            lastBackupToolStripMenuItem.Click += lastBackupToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // deleteModPakToolStripMenuItem
            // 
            deleteModPakToolStripMenuItem.Name = "deleteModPakToolStripMenuItem";
            deleteModPakToolStripMenuItem.Size = new Size(180, 22);
            deleteModPakToolStripMenuItem.Text = "Delete ModPak";
            deleteModPakToolStripMenuItem.Click += deleteModPakToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(177, 6);
            // 
            // forceInstallToolStripMenuItem
            // 
            forceInstallToolStripMenuItem.Enabled = false;
            forceInstallToolStripMenuItem.Name = "forceInstallToolStripMenuItem";
            forceInstallToolStripMenuItem.Size = new Size(180, 22);
            forceInstallToolStripMenuItem.Text = "Force Install";
            forceInstallToolStripMenuItem.Click += forceInstallToolStripMenuItem_Click;
            // 
            // logsToolStripMenuItem
            // 
            logsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openCurrentLogToolStripMenuItem, openFolderLogsToolStripMenuItem });
            logsToolStripMenuItem.Name = "logsToolStripMenuItem";
            logsToolStripMenuItem.Size = new Size(44, 20);
            logsToolStripMenuItem.Text = "Logs";
            // 
            // openCurrentLogToolStripMenuItem
            // 
            openCurrentLogToolStripMenuItem.Name = "openCurrentLogToolStripMenuItem";
            openCurrentLogToolStripMenuItem.Size = new Size(169, 22);
            openCurrentLogToolStripMenuItem.Text = "Open Current Log";
            openCurrentLogToolStripMenuItem.Click += openCurrentLogToolStripMenuItem_Click;
            // 
            // openFolderLogsToolStripMenuItem
            // 
            openFolderLogsToolStripMenuItem.Name = "openFolderLogsToolStripMenuItem";
            openFolderLogsToolStripMenuItem.Size = new Size(169, 22);
            openFolderLogsToolStripMenuItem.Text = "Open Folder Logs";
            openFolderLogsToolStripMenuItem.Click += openFolderLogsToolStripMenuItem_Click;
            // 
            // StatusInfo
            // 
            StatusInfo.Items.AddRange(new ToolStripItem[] { VersionAppLabel, toolStripStatusInfo, toolStripStatusLabelInfoPatch, ProgressBar });
            StatusInfo.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            StatusInfo.Location = new Point(0, 166);
            StatusInfo.Name = "StatusInfo";
            StatusInfo.Size = new Size(359, 22);
            StatusInfo.TabIndex = 2;
            // 
            // VersionAppLabel
            // 
            VersionAppLabel.Name = "VersionAppLabel";
            VersionAppLabel.Size = new Size(0, 17);
            // 
            // toolStripStatusInfo
            // 
            toolStripStatusInfo.Name = "toolStripStatusInfo";
            toolStripStatusInfo.Size = new Size(0, 17);
            // 
            // toolStripStatusLabelInfoPatch
            // 
            toolStripStatusLabelInfoPatch.Name = "toolStripStatusLabelInfoPatch";
            toolStripStatusLabelInfoPatch.Size = new Size(0, 17);
            // 
            // ProgressBar
            // 
            ProgressBar.Alignment = ToolStripItemAlignment.Right;
            ProgressBar.Name = "ProgressBar";
            ProgressBar.RightToLeft = RightToLeft.No;
            ProgressBar.Size = new Size(100, 16);
            ProgressBar.Step = 1000;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(CurrentVersionLabel);
            groupBox1.Controls.Add(LastVersionLabel);
            groupBox1.Controls.Add(ModVersionLabel);
            groupBox1.Location = new Point(12, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(335, 76);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "ModPak Info";
            // 
            // CurrentVersionLabel
            // 
            CurrentVersionLabel.AutoSize = true;
            CurrentVersionLabel.Location = new Point(6, 35);
            CurrentVersionLabel.Name = "CurrentVersionLabel";
            CurrentVersionLabel.Size = new Size(144, 15);
            CurrentVersionLabel.TabIndex = 2;
            CurrentVersionLabel.Text = "Current Version Installed:  ";
            // 
            // LastVersionLabel
            // 
            LastVersionLabel.AutoSize = true;
            LastVersionLabel.Location = new Point(6, 51);
            LastVersionLabel.Name = "LastVersionLabel";
            LastVersionLabel.Size = new Size(127, 15);
            LastVersionLabel.TabIndex = 1;
            LastVersionLabel.Text = "Last Release Available: ";
            // 
            // UpdateModButton
            // 
            UpdateModButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            UpdateModButton.Enabled = false;
            UpdateModButton.Location = new Point(114, 140);
            UpdateModButton.Name = "UpdateModButton";
            UpdateModButton.Size = new Size(129, 23);
            UpdateModButton.TabIndex = 4;
            UpdateModButton.Text = "Up do Date";
            UpdateModButton.UseVisualStyleBackColor = true;
            UpdateModButton.Click += UpdateModButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(359, 188);
            Controls.Add(UpdateModButton);
            Controls.Add(StatusInfo);
            Controls.Add(menuStrip1);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            MaximumSize = new Size(375, 227);
            MinimumSize = new Size(375, 227);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Snowrunner Patcher";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            StatusInfo.ResumeLayout(false);
            StatusInfo.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label ModVersionLabel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem configToolStripMenuItem;
        private ToolStripMenuItem changeModPathToolStripMenuItem;
        private ToolStripMenuItem openConfigFileToolStripMenuItem;
        private ToolStripStatusLabel VersionAppLabel;
        private ToolStripProgressBar ProgressBar;
        private GroupBox groupBox1;
        private Button UpdateModButton;
        private Label LastVersionLabel;
        private ToolStripMenuItem openModDirectoryToolStripMenuItem;
        private ToolStripMenuItem modPakToolStripMenuItem;
        private ToolStripMenuItem advancedPatchingToolStripMenuItem;
        private ToolStripMenuItem deleteModPakToolStripMenuItem;
        private ToolStripMenuItem openBackupsToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem deleteAllToolStripMenuItem;
        private ToolStripMenuItem replaceBackupToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusInfo;
        private ToolStripMenuItem lastBackupToolStripMenuItem;
        private ToolStripMenuItem forceInstallToolStripMenuItem;
        private ToolStripMenuItem createBackupToolStripMenuItem;
        public StatusStrip StatusInfo;
        public ToolStripStatusLabel toolStripStatusLabelInfoPatch;
        private ToolStripMenuItem logsToolStripMenuItem;
        private ToolStripMenuItem openCurrentLogToolStripMenuItem;
        private ToolStripMenuItem openFolderLogsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private Label CurrentVersionLabel;
    }
}