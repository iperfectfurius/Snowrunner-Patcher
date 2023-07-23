namespace Snowrunner_Parcher
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
            ModVersionLabel = new Label();
            menuStrip1 = new MenuStrip();
            configToolStripMenuItem = new ToolStripMenuItem();
            changeModPathToolStripMenuItem = new ToolStripMenuItem();
            openConfigFileToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            VersionAppLabel = new ToolStripStatusLabel();
            toolStripProgressBar1 = new ToolStripProgressBar();
            groupBox1 = new GroupBox();
            LastVersionLabel = new Label();
            UpdateModButton = new Button();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // ModVersionLabel
            // 
            ModVersionLabel.AutoSize = true;
            ModVersionLabel.Location = new Point(6, 19);
            ModVersionLabel.Name = "ModVersionLabel";
            ModVersionLabel.Size = new Size(126, 15);
            ModVersionLabel.TabIndex = 0;
            ModVersionLabel.Text = "Mod Version Installed: ";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { configToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(359, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // configToolStripMenuItem
            // 
            configToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { changeModPathToolStripMenuItem, openConfigFileToolStripMenuItem });
            configToolStripMenuItem.Name = "configToolStripMenuItem";
            configToolStripMenuItem.Size = new Size(55, 20);
            configToolStripMenuItem.Text = "Config";
            // 
            // changeModPathToolStripMenuItem
            // 
            changeModPathToolStripMenuItem.Name = "changeModPathToolStripMenuItem";
            changeModPathToolStripMenuItem.Size = new Size(170, 22);
            changeModPathToolStripMenuItem.Text = "Change Mod Path";
            // 
            // openConfigFileToolStripMenuItem
            // 
            openConfigFileToolStripMenuItem.Name = "openConfigFileToolStripMenuItem";
            openConfigFileToolStripMenuItem.Size = new Size(170, 22);
            openConfigFileToolStripMenuItem.Text = "Open Config File";
            openConfigFileToolStripMenuItem.Click += openConfigFileToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { VersionAppLabel, toolStripProgressBar1 });
            statusStrip1.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            statusStrip1.Location = new Point(0, 166);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(359, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // VersionAppLabel
            // 
            VersionAppLabel.Name = "VersionAppLabel";
            VersionAppLabel.Size = new Size(0, 17);
            // 
            // toolStripProgressBar1
            // 
            toolStripProgressBar1.Alignment = ToolStripItemAlignment.Right;
            toolStripProgressBar1.Name = "toolStripProgressBar1";
            toolStripProgressBar1.RightToLeft = RightToLeft.No;
            toolStripProgressBar1.Size = new Size(100, 16);
            toolStripProgressBar1.Step = 250;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(LastVersionLabel);
            groupBox1.Controls.Add(ModVersionLabel);
            groupBox1.Location = new Point(12, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(335, 66);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Info";
            // 
            // LastVersionLabel
            // 
            LastVersionLabel.AutoSize = true;
            LastVersionLabel.Location = new Point(6, 40);
            LastVersionLabel.Name = "LastVersionLabel";
            LastVersionLabel.Size = new Size(126, 15);
            LastVersionLabel.TabIndex = 1;
            LastVersionLabel.Text = "Last Update Available: ";
            // 
            // UpdateModButton
            // 
            UpdateModButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            UpdateModButton.Enabled = false;
            UpdateModButton.Location = new Point(114, 140);
            UpdateModButton.Name = "UpdateModButton";
            UpdateModButton.Size = new Size(129, 23);
            UpdateModButton.TabIndex = 4;
            UpdateModButton.Text = "Update";
            UpdateModButton.UseVisualStyleBackColor = true;
            UpdateModButton.Click += UpdateModButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(359, 188);
            Controls.Add(UpdateModButton);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(groupBox1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
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
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel VersionAppLabel;
        private ToolStripProgressBar toolStripProgressBar1;
        private GroupBox groupBox1;
        private Button UpdateModButton;
        private Label LastVersionLabel;
    }
}