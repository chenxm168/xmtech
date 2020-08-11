namespace MPC.Forms
{
    partial class frmPanelInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPanelInfo));
            this.tlLevel1 = new System.Windows.Forms.TableLayoutPanel();
            this.plBtnArea = new System.Windows.Forms.Panel();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.plInfoArea = new System.Windows.Forms.Panel();
            this.lbDefectCodeErrinfo = new System.Windows.Forms.Label();
            this.lbSpecErrInfo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbVCRID = new System.Windows.Forms.Label();
            this.txDefectCode = new System.Windows.Forms.TextBox();
            this.txPanelID = new System.Windows.Forms.TextBox();
            this.txProductSpec = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbIsZH = new System.Windows.Forms.CheckBox();
            this.cbIsLB = new System.Windows.Forms.CheckBox();
            this.cbIsCell = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.plImageArea = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pbImage6 = new System.Windows.Forms.PictureBox();
            this.pbImage5 = new System.Windows.Forms.PictureBox();
            this.pbImage4 = new System.Windows.Forms.PictureBox();
            this.pbImage3 = new System.Windows.Forms.PictureBox();
            this.pbImage2 = new System.Windows.Forms.PictureBox();
            this.pbImage1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.tlLevel1.SuspendLayout();
            this.plBtnArea.SuspendLayout();
            this.plInfoArea.SuspendLayout();
            this.plImageArea.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlLevel1
            // 
            this.tlLevel1.ColumnCount = 3;
            this.tlLevel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tlLevel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlLevel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tlLevel1.Controls.Add(this.plBtnArea, 1, 3);
            this.tlLevel1.Controls.Add(this.plInfoArea, 1, 0);
            this.tlLevel1.Controls.Add(this.plImageArea, 1, 2);
            this.tlLevel1.Controls.Add(this.panel1, 1, 1);
            this.tlLevel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlLevel1.Location = new System.Drawing.Point(0, 0);
            this.tlLevel1.Name = "tlLevel1";
            this.tlLevel1.RowCount = 4;
            this.tlLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlLevel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlLevel1.Size = new System.Drawing.Size(651, 562);
            this.tlLevel1.TabIndex = 32;
            // 
            // plBtnArea
            // 
            this.plBtnArea.Controls.Add(this.btCancel);
            this.plBtnArea.Controls.Add(this.btOK);
            this.plBtnArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plBtnArea.Location = new System.Drawing.Point(8, 515);
            this.plBtnArea.Name = "plBtnArea";
            this.plBtnArea.Size = new System.Drawing.Size(635, 44);
            this.plBtnArea.TabIndex = 30;
            // 
            // btCancel
            // 
            this.btCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(52, 6);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(74, 35);
            this.btCancel.TabIndex = 7;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = false;
            // 
            // btOK
            // 
            this.btOK.BackColor = System.Drawing.SystemColors.Control;
            this.btOK.Location = new System.Drawing.Point(523, 6);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(74, 35);
            this.btOK.TabIndex = 6;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = false;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // plInfoArea
            // 
            this.plInfoArea.Controls.Add(this.lbDefectCodeErrinfo);
            this.plInfoArea.Controls.Add(this.lbSpecErrInfo);
            this.plInfoArea.Controls.Add(this.label2);
            this.plInfoArea.Controls.Add(this.lbVCRID);
            this.plInfoArea.Controls.Add(this.txDefectCode);
            this.plInfoArea.Controls.Add(this.txPanelID);
            this.plInfoArea.Controls.Add(this.txProductSpec);
            this.plInfoArea.Controls.Add(this.label4);
            this.plInfoArea.Controls.Add(this.cbIsZH);
            this.plInfoArea.Controls.Add(this.cbIsLB);
            this.plInfoArea.Controls.Add(this.cbIsCell);
            this.plInfoArea.Controls.Add(this.label1);
            this.plInfoArea.Controls.Add(this.label3);
            this.plInfoArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plInfoArea.Location = new System.Drawing.Point(8, 3);
            this.plInfoArea.Name = "plInfoArea";
            this.plInfoArea.Size = new System.Drawing.Size(635, 225);
            this.plInfoArea.TabIndex = 31;
            // 
            // lbDefectCodeErrinfo
            // 
            this.lbDefectCodeErrinfo.AutoSize = true;
            this.lbDefectCodeErrinfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDefectCodeErrinfo.ForeColor = System.Drawing.Color.Red;
            this.lbDefectCodeErrinfo.Location = new System.Drawing.Point(400, 117);
            this.lbDefectCodeErrinfo.Name = "lbDefectCodeErrinfo";
            this.lbDefectCodeErrinfo.Size = new System.Drawing.Size(125, 12);
            this.lbDefectCodeErrinfo.TabIndex = 16;
            this.lbDefectCodeErrinfo.Text = "缺陷代码输入错误！！";
            this.lbDefectCodeErrinfo.Visible = false;
            // 
            // lbSpecErrInfo
            // 
            this.lbSpecErrInfo.AutoSize = true;
            this.lbSpecErrInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSpecErrInfo.ForeColor = System.Drawing.Color.Red;
            this.lbSpecErrInfo.Location = new System.Drawing.Point(400, 29);
            this.lbSpecErrInfo.Name = "lbSpecErrInfo";
            this.lbSpecErrInfo.Size = new System.Drawing.Size(101, 12);
            this.lbSpecErrInfo.TabIndex = 15;
            this.lbSpecErrInfo.Text = "型号输入错误！！";
            this.lbSpecErrInfo.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(153, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "读码：";
            // 
            // lbVCRID
            // 
            this.lbVCRID.AutoSize = true;
            this.lbVCRID.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbVCRID.ForeColor = System.Drawing.Color.Red;
            this.lbVCRID.Location = new System.Drawing.Point(192, 85);
            this.lbVCRID.Name = "lbVCRID";
            this.lbVCRID.Size = new System.Drawing.Size(65, 12);
            this.lbVCRID.TabIndex = 13;
            this.lbVCRID.Text = "48E1J08232";
            // 
            // txDefectCode
            // 
            this.txDefectCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txDefectCode.Location = new System.Drawing.Point(150, 114);
            this.txDefectCode.Name = "txDefectCode";
            this.txDefectCode.Size = new System.Drawing.Size(210, 21);
            this.txDefectCode.TabIndex = 1;
            this.txDefectCode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txDefectCode_KeyUp);
            // 
            // txPanelID
            // 
            this.txPanelID.Location = new System.Drawing.Point(150, 59);
            this.txPanelID.Name = "txPanelID";
            this.txPanelID.ReadOnly = true;
            this.txPanelID.Size = new System.Drawing.Size(210, 21);
            this.txPanelID.TabIndex = 11;
            // 
            // txProductSpec
            // 
            this.txProductSpec.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txProductSpec.Location = new System.Drawing.Point(150, 26);
            this.txProductSpec.Name = "txProductSpec";
            this.txProductSpec.Size = new System.Drawing.Size(210, 21);
            this.txProductSpec.TabIndex = 0;
            this.txProductSpec.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txProductSpec_KeyPress);
            this.txProductSpec.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txProductSpec_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "缺陷代码：";
            // 
            // cbIsZH
            // 
            this.cbIsZH.AutoSize = true;
            this.cbIsZH.Location = new System.Drawing.Point(308, 170);
            this.cbIsZH.Name = "cbIsZH";
            this.cbIsZH.Size = new System.Drawing.Size(72, 16);
            this.cbIsZH.TabIndex = 4;
            this.cbIsZH.Text = "是否自划";
            this.cbIsZH.UseVisualStyleBackColor = true;
            this.cbIsZH.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbIsZH_KeyUp);
            // 
            // cbIsLB
            // 
            this.cbIsLB.AutoSize = true;
            this.cbIsLB.Checked = true;
            this.cbIsLB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIsLB.Location = new System.Drawing.Point(150, 170);
            this.cbIsLB.Name = "cbIsLB";
            this.cbIsLB.Size = new System.Drawing.Size(72, 16);
            this.cbIsLB.TabIndex = 3;
            this.cbIsLB.Text = "是否漏笔";
            this.cbIsLB.UseVisualStyleBackColor = true;
            this.cbIsLB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbIsLB_KeyUp);
            // 
            // cbIsCell
            // 
            this.cbIsCell.AutoSize = true;
            this.cbIsCell.Checked = true;
            this.cbIsCell.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIsCell.Location = new System.Drawing.Point(13, 170);
            this.cbIsCell.Name = "cbIsCell";
            this.cbIsCell.Size = new System.Drawing.Size(72, 16);
            this.cbIsCell.TabIndex = 2;
            this.cbIsCell.Text = "是否盒内";
            this.cbIsCell.UseVisualStyleBackColor = true;
            this.cbIsCell.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbIsCell_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "PanelID：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "型 号：";
            // 
            // plImageArea
            // 
            this.plImageArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plImageArea.Controls.Add(this.tableLayoutPanel1);
            this.plImageArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plImageArea.Location = new System.Drawing.Point(8, 284);
            this.plImageArea.Name = "plImageArea";
            this.plImageArea.Size = new System.Drawing.Size(635, 225);
            this.plImageArea.TabIndex = 32;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.pbImage6, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.pbImage5, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.pbImage4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pbImage3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.pbImage2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pbImage1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(633, 223);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pbImage6
            // 
            this.pbImage6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbImage6.BackgroundImage")));
            this.pbImage6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbImage6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbImage6.Location = new System.Drawing.Point(425, 114);
            this.pbImage6.Name = "pbImage6";
            this.pbImage6.Size = new System.Drawing.Size(205, 106);
            this.pbImage6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage6.TabIndex = 5;
            this.pbImage6.TabStop = false;
            this.pbImage6.Click += new System.EventHandler(this.pbImage6_Click);
            // 
            // pbImage5
            // 
            this.pbImage5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbImage5.BackgroundImage")));
            this.pbImage5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbImage5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbImage5.Location = new System.Drawing.Point(214, 114);
            this.pbImage5.Name = "pbImage5";
            this.pbImage5.Size = new System.Drawing.Size(205, 106);
            this.pbImage5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage5.TabIndex = 4;
            this.pbImage5.TabStop = false;
            this.pbImage5.Click += new System.EventHandler(this.pbImage5_Click);
            // 
            // pbImage4
            // 
            this.pbImage4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbImage4.BackgroundImage")));
            this.pbImage4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbImage4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbImage4.Location = new System.Drawing.Point(3, 114);
            this.pbImage4.Name = "pbImage4";
            this.pbImage4.Size = new System.Drawing.Size(205, 106);
            this.pbImage4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage4.TabIndex = 3;
            this.pbImage4.TabStop = false;
            this.pbImage4.Click += new System.EventHandler(this.pbImage4_Click);
            // 
            // pbImage3
            // 
            this.pbImage3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbImage3.BackgroundImage")));
            this.pbImage3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbImage3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbImage3.Location = new System.Drawing.Point(425, 3);
            this.pbImage3.Name = "pbImage3";
            this.pbImage3.Size = new System.Drawing.Size(205, 105);
            this.pbImage3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage3.TabIndex = 2;
            this.pbImage3.TabStop = false;
            this.pbImage3.Click += new System.EventHandler(this.pbImage3_Click);
            // 
            // pbImage2
            // 
            this.pbImage2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbImage2.BackgroundImage")));
            this.pbImage2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbImage2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbImage2.Location = new System.Drawing.Point(214, 3);
            this.pbImage2.Name = "pbImage2";
            this.pbImage2.Size = new System.Drawing.Size(205, 105);
            this.pbImage2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage2.TabIndex = 1;
            this.pbImage2.TabStop = false;
            this.pbImage2.Click += new System.EventHandler(this.pbImage2_Click);
            // 
            // pbImage1
            // 
            this.pbImage1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbImage1.BackgroundImage")));
            this.pbImage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbImage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbImage1.Location = new System.Drawing.Point(3, 3);
            this.pbImage1.Name = "pbImage1";
            this.pbImage1.Size = new System.Drawing.Size(205, 105);
            this.pbImage1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage1.TabIndex = 0;
            this.pbImage1.TabStop = false;
            this.pbImage1.Click += new System.EventHandler(this.pbImage1_Click);
            this.pbImage1.DoubleClick += new System.EventHandler(this.pbImage1_DoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(8, 234);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(635, 44);
            this.panel1.TabIndex = 33;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(15, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 5;
            this.label5.Text = "镜检图片";
            // 
            // frmPanelInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 562);
            this.ControlBox = false;
            this.Controls.Add(this.tlLevel1);
            this.Name = "frmPanelInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Panel information";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPanelInfo_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmPanelInfo_KeyUp);
            this.tlLevel1.ResumeLayout(false);
            this.plBtnArea.ResumeLayout(false);
            this.plInfoArea.ResumeLayout(false);
            this.plInfoArea.PerformLayout();
            this.plImageArea.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlLevel1;
        private System.Windows.Forms.Panel plBtnArea;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Panel plInfoArea;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbVCRID;
        private System.Windows.Forms.TextBox txDefectCode;
        private System.Windows.Forms.TextBox txPanelID;
        private System.Windows.Forms.TextBox txProductSpec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbIsZH;
        private System.Windows.Forms.CheckBox cbIsLB;
        private System.Windows.Forms.CheckBox cbIsCell;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel plImageArea;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pbImage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pbImage6;
        private System.Windows.Forms.PictureBox pbImage5;
        private System.Windows.Forms.PictureBox pbImage4;
        private System.Windows.Forms.PictureBox pbImage3;
        private System.Windows.Forms.PictureBox pbImage2;
        private System.Windows.Forms.Label lbSpecErrInfo;
        private System.Windows.Forms.Label lbDefectCodeErrinfo;
    }
}