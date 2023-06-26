namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.UpgradeServicePanel = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.UpgradeProgressBar = new System.Windows.Forms.ProgressBar();
            this.UpgradeButton = new System.Windows.Forms.Button();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.UgFileTextBox = new System.Windows.Forms.TextBox();
            this.IDConfigPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.CANIDConfigButton = new System.Windows.Forms.Button();
            this.ConfigProgressBar = new System.Windows.Forms.ProgressBar();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.FromFanText = new System.Windows.Forms.TextBox();
            this.Global2FanText = new System.Windows.Forms.TextBox();
            this.Specific2FanText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SoftwareConfigPanel = new System.Windows.Forms.Panel();
            this.OldVersionButton = new System.Windows.Forms.RadioButton();
            this.LastestVersionButton = new System.Windows.Forms.RadioButton();
            this.BaudrateComboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.UpgradeServicePanel.SuspendLayout();
            this.IDConfigPanel.SuspendLayout();
            this.SoftwareConfigPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpgradeServicePanel
            // 
            this.UpgradeServicePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UpgradeServicePanel.Controls.Add(this.label10);
            this.UpgradeServicePanel.Controls.Add(this.UpgradeProgressBar);
            this.UpgradeServicePanel.Controls.Add(this.UpgradeButton);
            this.UpgradeServicePanel.Controls.Add(this.BrowseButton);
            this.UpgradeServicePanel.Controls.Add(this.UgFileTextBox);
            this.UpgradeServicePanel.Location = new System.Drawing.Point(9, 352);
            this.UpgradeServicePanel.Name = "UpgradeServicePanel";
            this.UpgradeServicePanel.Size = new System.Drawing.Size(879, 198);
            this.UpgradeServicePanel.TabIndex = 13;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(7, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(239, 27);
            this.label10.TabIndex = 29;
            this.label10.Text = "CAN upgrade service";
            // 
            // UpgradeProgressBar
            // 
            this.UpgradeProgressBar.Location = new System.Drawing.Point(65, 118);
            this.UpgradeProgressBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.UpgradeProgressBar.Name = "UpgradeProgressBar";
            this.UpgradeProgressBar.Size = new System.Drawing.Size(634, 32);
            this.UpgradeProgressBar.TabIndex = 26;
            this.UpgradeProgressBar.Click += new System.EventHandler(this.UpgradeProgressBar_Click);
            // 
            // UpgradeButton
            // 
            this.UpgradeButton.Location = new System.Drawing.Point(707, 118);
            this.UpgradeButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.UpgradeButton.Name = "UpgradeButton";
            this.UpgradeButton.Size = new System.Drawing.Size(124, 32);
            this.UpgradeButton.TabIndex = 25;
            this.UpgradeButton.Text = "upgrade";
            this.UpgradeButton.UseVisualStyleBackColor = true;
            this.UpgradeButton.Click += new System.EventHandler(this.UpgradeButton_Click);
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(707, 76);
            this.BrowseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(124, 32);
            this.BrowseButton.TabIndex = 24;
            this.BrowseButton.Text = "browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // UgFileTextBox
            // 
            this.UgFileTextBox.Location = new System.Drawing.Point(65, 76);
            this.UgFileTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.UgFileTextBox.Multiline = true;
            this.UgFileTextBox.Name = "UgFileTextBox";
            this.UgFileTextBox.Size = new System.Drawing.Size(634, 32);
            this.UgFileTextBox.TabIndex = 23;
            this.UgFileTextBox.TextChanged += new System.EventHandler(this.UgFileTextBox_TextChanged);
            // 
            // IDConfigPanel
            // 
            this.IDConfigPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.IDConfigPanel.Controls.Add(this.label1);
            this.IDConfigPanel.Controls.Add(this.CANIDConfigButton);
            this.IDConfigPanel.Controls.Add(this.ConfigProgressBar);
            this.IDConfigPanel.Controls.Add(this.label8);
            this.IDConfigPanel.Controls.Add(this.label7);
            this.IDConfigPanel.Controls.Add(this.label6);
            this.IDConfigPanel.Controls.Add(this.FromFanText);
            this.IDConfigPanel.Controls.Add(this.Global2FanText);
            this.IDConfigPanel.Controls.Add(this.Specific2FanText);
            this.IDConfigPanel.Controls.Add(this.label5);
            this.IDConfigPanel.Controls.Add(this.label4);
            this.IDConfigPanel.Controls.Add(this.label3);
            this.IDConfigPanel.Controls.Add(this.label2);
            this.IDConfigPanel.Location = new System.Drawing.Point(394, 4);
            this.IDConfigPanel.Name = "IDConfigPanel";
            this.IDConfigPanel.Size = new System.Drawing.Size(494, 342);
            this.IDConfigPanel.TabIndex = 14;
            this.IDConfigPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 27);
            this.label1.TabIndex = 24;
            this.label1.Text = "CAN ID config";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // CANIDConfigButton
            // 
            this.CANIDConfigButton.Location = new System.Drawing.Point(43, 263);
            this.CANIDConfigButton.Name = "CANIDConfigButton";
            this.CANIDConfigButton.Size = new System.Drawing.Size(124, 32);
            this.CANIDConfigButton.TabIndex = 23;
            this.CANIDConfigButton.Text = "Config";
            this.CANIDConfigButton.UseVisualStyleBackColor = true;
            this.CANIDConfigButton.Click += new System.EventHandler(this.CANIDConfigButton_Click);
            // 
            // ConfigProgressBar
            // 
            this.ConfigProgressBar.Location = new System.Drawing.Point(173, 263);
            this.ConfigProgressBar.Name = "ConfigProgressBar";
            this.ConfigProgressBar.Size = new System.Drawing.Size(233, 32);
            this.ConfigProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.ConfigProgressBar.TabIndex = 22;
            this.ConfigProgressBar.Click += new System.EventHandler(this.ConfigProgressBar_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(199, 202);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 21);
            this.label8.TabIndex = 21;
            this.label8.Text = "0x";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(199, 155);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 21);
            this.label7.TabIndex = 20;
            this.label7.Text = "0x";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(199, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 21);
            this.label6.TabIndex = 19;
            this.label6.Text = "0x";
            // 
            // FromFanText
            // 
            this.FromFanText.Location = new System.Drawing.Point(223, 199);
            this.FromFanText.Name = "FromFanText";
            this.FromFanText.Size = new System.Drawing.Size(132, 28);
            this.FromFanText.TabIndex = 18;
            // 
            // Global2FanText
            // 
            this.Global2FanText.Location = new System.Drawing.Point(223, 152);
            this.Global2FanText.Name = "Global2FanText";
            this.Global2FanText.Size = new System.Drawing.Size(132, 28);
            this.Global2FanText.TabIndex = 17;
            // 
            // Specific2FanText
            // 
            this.Specific2FanText.Location = new System.Drawing.Point(223, 105);
            this.Specific2FanText.Name = "Specific2FanText";
            this.Specific2FanText.Size = new System.Drawing.Size(132, 28);
            this.Specific2FanText.TabIndex = 16;
            this.Specific2FanText.TextChanged += new System.EventHandler(this.Specific2FanText_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(61, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 21);
            this.label5.TabIndex = 15;
            this.label5.Text = "From fan";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 21);
            this.label4.TabIndex = 14;
            this.label4.Text = "Global to fan";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 21);
            this.label3.TabIndex = 13;
            this.label3.Text = "Specific to fan";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 21);
            this.label2.TabIndex = 12;
            this.label2.Text = "Message ID";
            // 
            // SoftwareConfigPanel
            // 
            this.SoftwareConfigPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SoftwareConfigPanel.Controls.Add(this.OldVersionButton);
            this.SoftwareConfigPanel.Controls.Add(this.LastestVersionButton);
            this.SoftwareConfigPanel.Controls.Add(this.BaudrateComboBox);
            this.SoftwareConfigPanel.Controls.Add(this.label11);
            this.SoftwareConfigPanel.Controls.Add(this.label9);
            this.SoftwareConfigPanel.Location = new System.Drawing.Point(9, 4);
            this.SoftwareConfigPanel.Name = "SoftwareConfigPanel";
            this.SoftwareConfigPanel.Size = new System.Drawing.Size(379, 342);
            this.SoftwareConfigPanel.TabIndex = 15;
            // 
            // OldVersionButton
            // 
            this.OldVersionButton.AutoSize = true;
            this.OldVersionButton.Location = new System.Drawing.Point(65, 94);
            this.OldVersionButton.Name = "OldVersionButton";
            this.OldVersionButton.Size = new System.Drawing.Size(126, 25);
            this.OldVersionButton.TabIndex = 29;
            this.OldVersionButton.TabStop = true;
            this.OldVersionButton.Text = "Old version";
            this.OldVersionButton.UseVisualStyleBackColor = true;
            this.OldVersionButton.CheckedChanged += new System.EventHandler(this.OldVersionButton_CheckedChanged);
            // 
            // LastestVersionButton
            // 
            this.LastestVersionButton.AutoSize = true;
            this.LastestVersionButton.Checked = true;
            this.LastestVersionButton.Location = new System.Drawing.Point(65, 63);
            this.LastestVersionButton.Name = "LastestVersionButton";
            this.LastestVersionButton.Size = new System.Drawing.Size(156, 25);
            this.LastestVersionButton.TabIndex = 28;
            this.LastestVersionButton.TabStop = true;
            this.LastestVersionButton.Text = "Lastest version";
            this.LastestVersionButton.UseVisualStyleBackColor = true;
            this.LastestVersionButton.CheckedChanged += new System.EventHandler(this.LastestVersionButton_CheckedChanged);
            // 
            // BaudrateComboBox
            // 
            this.BaudrateComboBox.FormattingEnabled = true;
            this.BaudrateComboBox.Items.AddRange(new object[] {
            "1000kbps",
            "800kbps",
            "500kbps",
            "250kbps",
            "125kbps",
            "100kbps",
            "50kbps",
            "20kbps",
            "10kbps",
            "5kbps"});
            this.BaudrateComboBox.Location = new System.Drawing.Point(203, 165);
            this.BaudrateComboBox.Name = "BaudrateComboBox";
            this.BaudrateComboBox.Size = new System.Drawing.Size(134, 29);
            this.BaudrateComboBox.TabIndex = 27;
            this.BaudrateComboBox.SelectedIndexChanged += new System.EventHandler(this.BaudrateComboBox_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(61, 168);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(123, 21);
            this.label11.TabIndex = 26;
            this.label11.Text = "CAN baudrate";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(178, 27);
            this.label9.TabIndex = 25;
            this.label9.Text = "Software config";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 558);
            this.Controls.Add(this.SoftwareConfigPanel);
            this.Controls.Add(this.IDConfigPanel);
            this.Controls.Add(this.UpgradeServicePanel);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CAN";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.UpgradeServicePanel.ResumeLayout(false);
            this.UpgradeServicePanel.PerformLayout();
            this.IDConfigPanel.ResumeLayout(false);
            this.IDConfigPanel.PerformLayout();
            this.SoftwareConfigPanel.ResumeLayout(false);
            this.SoftwareConfigPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel UpgradeServicePanel;
        private System.Windows.Forms.ProgressBar UpgradeProgressBar;
        private System.Windows.Forms.Button UpgradeButton;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.TextBox UgFileTextBox;
        private System.Windows.Forms.Panel IDConfigPanel;
        private System.Windows.Forms.Button CANIDConfigButton;
        private System.Windows.Forms.ProgressBar ConfigProgressBar;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox FromFanText;
        private System.Windows.Forms.TextBox Global2FanText;
        private System.Windows.Forms.TextBox Specific2FanText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel SoftwareConfigPanel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox BaudrateComboBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RadioButton OldVersionButton;
        private System.Windows.Forms.RadioButton LastestVersionButton;
    }
}

