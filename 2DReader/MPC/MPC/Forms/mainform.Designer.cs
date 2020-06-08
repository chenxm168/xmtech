namespace MPC.Forms
{
    partial class mainform
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainform));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btUpate = new System.Windows.Forms.Button();
            this.txIpPort = new System.Windows.Forms.TextBox();
            this.txIpaddress = new System.Windows.Forms.TextBox();
            this.btConnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnVCRMode = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btDefectChange = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rbSelectDefectForSheets = new System.Windows.Forms.RadioButton();
            this.rbSelectDefectBySheet = new System.Windows.Forms.RadioButton();
            this.pnOutput = new System.Windows.Forms.Panel();
            this.tcOutput = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txIformation = new System.Windows.Forms.TextBox();
            this.pnControl = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btReadStop = new System.Windows.Forms.Button();
            this.btReadStart = new System.Windows.Forms.Button();
            this.btRead = new System.Windows.Forms.Button();
            this.rbCycleRead = new System.Windows.Forms.RadioButton();
            this.rbReadBySheet = new System.Windows.Forms.RadioButton();
            this.lbDefectCode = new System.Windows.Forms.Label();
            this.lbDefectName = new System.Windows.Forms.Label();
            this.lbDefectCategory = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnVCRMode.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.pnOutput.SuspendLayout();
            this.tcOutput.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.pnControl.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnVCRMode, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnOutput, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.pnControl, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(995, 604);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btUpate);
            this.groupBox1.Controls.Add(this.txIpPort);
            this.groupBox1.Controls.Add(this.txIpaddress);
            this.groupBox1.Controls.Add(this.btConnect);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(659, 23);
            this.groupBox1.Name = "groupBox1";
            this.tableLayoutPanel1.SetRowSpan(this.groupBox1, 2);
            this.groupBox1.Size = new System.Drawing.Size(312, 370);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "VCR Information";
            // 
            // btUpate
            // 
            this.btUpate.BackColor = System.Drawing.SystemColors.Control;
            this.btUpate.Location = new System.Drawing.Point(191, 317);
            this.btUpate.Name = "btUpate";
            this.btUpate.Size = new System.Drawing.Size(100, 35);
            this.btUpate.TabIndex = 5;
            this.btUpate.Text = "数据上传";
            this.btUpate.UseVisualStyleBackColor = false;
            this.btUpate.Visible = false;
            this.btUpate.Click += new System.EventHandler(this.btUpate_Click);
            // 
            // txIpPort
            // 
            this.txIpPort.Enabled = false;
            this.txIpPort.Location = new System.Drawing.Point(33, 153);
            this.txIpPort.Name = "txIpPort";
            this.txIpPort.Size = new System.Drawing.Size(79, 26);
            this.txIpPort.TabIndex = 4;
            // 
            // txIpaddress
            // 
            this.txIpaddress.Enabled = false;
            this.txIpaddress.Location = new System.Drawing.Point(33, 70);
            this.txIpaddress.Name = "txIpaddress";
            this.txIpaddress.Size = new System.Drawing.Size(170, 26);
            this.txIpaddress.TabIndex = 3;
            // 
            // btConnect
            // 
            this.btConnect.BackColor = System.Drawing.SystemColors.Control;
            this.btConnect.Location = new System.Drawing.Point(191, 222);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(100, 35);
            this.btConnect.TabIndex = 2;
            this.btConnect.Text = "请连接";
            this.btConnect.UseVisualStyleBackColor = false;
            this.btConnect.Click += new System.EventHandler(this.btConnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "IP Port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP Address";
            // 
            // pnVCRMode
            // 
            this.pnVCRMode.Controls.Add(this.groupBox2);
            this.pnVCRMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnVCRMode.Location = new System.Drawing.Point(23, 23);
            this.pnVCRMode.Name = "pnVCRMode";
            this.tableLayoutPanel1.SetRowSpan(this.pnVCRMode, 2);
            this.pnVCRMode.Size = new System.Drawing.Size(312, 370);
            this.pnVCRMode.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbDefectCategory);
            this.groupBox2.Controls.Add(this.lbDefectName);
            this.groupBox2.Controls.Add(this.lbDefectCode);
            this.groupBox2.Controls.Add(this.btDefectChange);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.rbSelectDefectForSheets);
            this.groupBox2.Controls.Add(this.rbSelectDefectBySheet);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(312, 370);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "VCR 读码模式";
            // 
            // btDefectChange
            // 
            this.btDefectChange.BackColor = System.Drawing.SystemColors.Control;
            this.btDefectChange.Location = new System.Drawing.Point(219, 317);
            this.btDefectChange.Name = "btDefectChange";
            this.btDefectChange.Size = new System.Drawing.Size(74, 35);
            this.btDefectChange.TabIndex = 5;
            this.btDefectChange.Text = "修 改";
            this.btDefectChange.UseVisualStyleBackColor = false;
            this.btDefectChange.Click += new System.EventHandler(this.btDefectChange_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(53, 270);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "缺陷名称：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(53, 229);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "缺陷代码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 184);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "缺陷类别：";
            // 
            // rbSelectDefectForSheets
            // 
            this.rbSelectDefectForSheets.AutoSize = true;
            this.rbSelectDefectForSheets.Location = new System.Drawing.Point(27, 133);
            this.rbSelectDefectForSheets.Name = "rbSelectDefectForSheets";
            this.rbSelectDefectForSheets.Size = new System.Drawing.Size(153, 24);
            this.rbSelectDefectForSheets.TabIndex = 1;
            this.rbSelectDefectForSheets.Text = "同缺陷批量读码模式";
            this.rbSelectDefectForSheets.UseVisualStyleBackColor = true;
            this.rbSelectDefectForSheets.CheckedChanged += new System.EventHandler(this.rbSelectDefectForSheets_CheckedChanged);
            // 
            // rbSelectDefectBySheet
            // 
            this.rbSelectDefectBySheet.AutoSize = true;
            this.rbSelectDefectBySheet.Checked = true;
            this.rbSelectDefectBySheet.Location = new System.Drawing.Point(27, 60);
            this.rbSelectDefectBySheet.Name = "rbSelectDefectBySheet";
            this.rbSelectDefectBySheet.Size = new System.Drawing.Size(153, 24);
            this.rbSelectDefectBySheet.TabIndex = 0;
            this.rbSelectDefectBySheet.TabStop = true;
            this.rbSelectDefectBySheet.Text = "每片选缺陷读码模式";
            this.rbSelectDefectBySheet.UseVisualStyleBackColor = true;
            this.rbSelectDefectBySheet.CheckedChanged += new System.EventHandler(this.rbSelectDefectBySheet_CheckedChanged);
            // 
            // pnOutput
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.pnOutput, 3);
            this.pnOutput.Controls.Add(this.tcOutput);
            this.pnOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnOutput.Location = new System.Drawing.Point(23, 399);
            this.pnOutput.Name = "pnOutput";
            this.pnOutput.Size = new System.Drawing.Size(948, 182);
            this.pnOutput.TabIndex = 3;
            // 
            // tcOutput
            // 
            this.tcOutput.Controls.Add(this.tabPage1);
            this.tcOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcOutput.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tcOutput.Location = new System.Drawing.Point(0, 0);
            this.tcOutput.Name = "tcOutput";
            this.tcOutput.SelectedIndex = 0;
            this.tcOutput.Size = new System.Drawing.Size(948, 182);
            this.tcOutput.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txIformation);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(940, 154);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "读码信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txIformation
            // 
            this.txIformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txIformation.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txIformation.Location = new System.Drawing.Point(3, 3);
            this.txIformation.Multiline = true;
            this.txIformation.Name = "txIformation";
            this.txIformation.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txIformation.Size = new System.Drawing.Size(934, 148);
            this.txIformation.TabIndex = 0;
            this.txIformation.Text = "Read Information";
            // 
            // pnControl
            // 
            this.pnControl.Controls.Add(this.groupBox4);
            this.pnControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnControl.Location = new System.Drawing.Point(341, 23);
            this.pnControl.Name = "pnControl";
            this.tableLayoutPanel1.SetRowSpan(this.pnControl, 2);
            this.pnControl.Size = new System.Drawing.Size(312, 370);
            this.pnControl.TabIndex = 4;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btReadStop);
            this.groupBox4.Controls.Add(this.btReadStart);
            this.groupBox4.Controls.Add(this.btRead);
            this.groupBox4.Controls.Add(this.rbCycleRead);
            this.groupBox4.Controls.Add(this.rbReadBySheet);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(312, 370);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Control";
            // 
            // btReadStop
            // 
            this.btReadStop.BackColor = System.Drawing.SystemColors.Control;
            this.btReadStop.Location = new System.Drawing.Point(104, 246);
            this.btReadStop.Name = "btReadStop";
            this.btReadStop.Size = new System.Drawing.Size(74, 35);
            this.btReadStop.TabIndex = 8;
            this.btReadStop.Text = "停 止";
            this.btReadStop.UseVisualStyleBackColor = false;
            this.btReadStop.Visible = false;
            // 
            // btReadStart
            // 
            this.btReadStart.BackColor = System.Drawing.SystemColors.Control;
            this.btReadStart.Location = new System.Drawing.Point(24, 255);
            this.btReadStart.Name = "btReadStart";
            this.btReadStart.Size = new System.Drawing.Size(74, 35);
            this.btReadStart.TabIndex = 7;
            this.btReadStart.Text = "开 始";
            this.btReadStart.UseVisualStyleBackColor = false;
            this.btReadStart.Visible = false;
            this.btReadStart.Click += new System.EventHandler(this.btReadStart_Click);
            // 
            // btRead
            // 
            this.btRead.BackColor = System.Drawing.SystemColors.Control;
            this.btRead.Location = new System.Drawing.Point(198, 317);
            this.btRead.Name = "btRead";
            this.btRead.Size = new System.Drawing.Size(74, 35);
            this.btRead.TabIndex = 6;
            this.btRead.Text = "读 取";
            this.btRead.UseVisualStyleBackColor = false;
            this.btRead.Click += new System.EventHandler(this.btRead_Click);
            // 
            // rbCycleRead
            // 
            this.rbCycleRead.AutoSize = true;
            this.rbCycleRead.Location = new System.Drawing.Point(24, 203);
            this.rbCycleRead.Name = "rbCycleRead";
            this.rbCycleRead.Size = new System.Drawing.Size(121, 24);
            this.rbCycleRead.TabIndex = 5;
            this.rbCycleRead.Text = "Panel筛选模式";
            this.rbCycleRead.UseVisualStyleBackColor = true;
            this.rbCycleRead.CheckedChanged += new System.EventHandler(this.rbCycleRead_CheckedChanged);
            // 
            // rbReadBySheet
            // 
            this.rbReadBySheet.AutoSize = true;
            this.rbReadBySheet.Checked = true;
            this.rbReadBySheet.Location = new System.Drawing.Point(24, 60);
            this.rbReadBySheet.Name = "rbReadBySheet";
            this.rbReadBySheet.Size = new System.Drawing.Size(121, 24);
            this.rbReadBySheet.TabIndex = 4;
            this.rbReadBySheet.TabStop = true;
            this.rbReadBySheet.Text = "Panel缺陷模式";
            this.rbReadBySheet.UseVisualStyleBackColor = true;
            this.rbReadBySheet.CheckedChanged += new System.EventHandler(this.rbReadBySheet_CheckedChanged);
            // 
            // lbDefectCode
            // 
            this.lbDefectCode.AutoSize = true;
            this.lbDefectCode.Location = new System.Drawing.Point(158, 229);
            this.lbDefectCode.Name = "lbDefectCode";
            this.lbDefectCode.Size = new System.Drawing.Size(0, 20);
            this.lbDefectCode.TabIndex = 6;
            // 
            // lbDefectName
            // 
            this.lbDefectName.AutoSize = true;
            this.lbDefectName.Location = new System.Drawing.Point(158, 270);
            this.lbDefectName.Name = "lbDefectName";
            this.lbDefectName.Size = new System.Drawing.Size(0, 20);
            this.lbDefectName.TabIndex = 7;
            // 
            // lbDefectCategory
            // 
            this.lbDefectCategory.AutoSize = true;
            this.lbDefectCategory.Location = new System.Drawing.Point(158, 184);
            this.lbDefectCategory.Name = "lbDefectCategory";
            this.lbDefectCategory.Size = new System.Drawing.Size(0, 20);
            this.lbDefectCategory.TabIndex = 8;
            // 
            // mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 604);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mainform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "2D CODE Reader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainform_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnVCRMode.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.pnOutput.ResumeLayout(false);
            this.tcOutput.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.pnControl.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txIpPort;
        private System.Windows.Forms.TextBox txIpaddress;
        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnVCRMode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbSelectDefectForSheets;
        private System.Windows.Forms.RadioButton rbSelectDefectBySheet;
        private System.Windows.Forms.Button btDefectChange;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnOutput;
        private System.Windows.Forms.TabControl tcOutput;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txIformation;
        private System.Windows.Forms.Panel pnControl;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btReadStop;
        private System.Windows.Forms.Button btReadStart;
        private System.Windows.Forms.Button btRead;
        private System.Windows.Forms.RadioButton rbCycleRead;
        private System.Windows.Forms.RadioButton rbReadBySheet;
        private System.Windows.Forms.Button btUpate;
        private System.Windows.Forms.Label lbDefectCategory;
        private System.Windows.Forms.Label lbDefectName;
        private System.Windows.Forms.Label lbDefectCode;
    }
}