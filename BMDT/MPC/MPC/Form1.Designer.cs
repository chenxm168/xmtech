namespace MPC
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
            this.btExit = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbConnection = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btControlStatus = new System.Windows.Forms.Button();
            this.cbControlStatus = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnS1F1 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btExit
            // 
            this.btExit.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btExit.Location = new System.Drawing.Point(390, 323);
            this.btExit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(79, 33);
            this.btExit.TabIndex = 0;
            this.btExit.Text = "退  出";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(45, 323);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbConnection);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(27, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(465, 105);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "EQP Info";
            // 
            // lbConnection
            // 
            this.lbConnection.AutoSize = true;
            this.lbConnection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lbConnection.Location = new System.Drawing.Point(254, 51);
            this.lbConnection.Name = "lbConnection";
            this.lbConnection.Size = new System.Drawing.Size(98, 16);
            this.lbConnection.TabIndex = 1;
            this.lbConnection.Text = "Connection";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(15, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Communication Status:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btControlStatus);
            this.groupBox2.Controls.Add(this.cbControlStatus);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(27, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(465, 160);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Host Info";
            // 
            // btControlStatus
            // 
            this.btControlStatus.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btControlStatus.Location = new System.Drawing.Point(363, 118);
            this.btControlStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btControlStatus.Name = "btControlStatus";
            this.btControlStatus.Size = new System.Drawing.Size(79, 33);
            this.btControlStatus.TabIndex = 5;
            this.btControlStatus.Text = "更  改";
            this.btControlStatus.UseVisualStyleBackColor = true;
            this.btControlStatus.Click += new System.EventHandler(this.btControlStatus_Click);
            // 
            // cbControlStatus
            // 
            this.cbControlStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbControlStatus.FormattingEnabled = true;
            this.cbControlStatus.Items.AddRange(new object[] {
            "OFFLINE",
            "REMOTE",
            "LOCAL"});
            this.cbControlStatus.Location = new System.Drawing.Point(257, 79);
            this.cbControlStatus.Name = "cbControlStatus";
            this.cbControlStatus.Size = new System.Drawing.Size(121, 24);
            this.cbControlStatus.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(15, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "Control  Status:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(254, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Connection";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(15, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Communication  Status:";
            // 
            // btnS1F1
            // 
            this.btnS1F1.Location = new System.Drawing.Point(27, 380);
            this.btnS1F1.Name = "btnS1F1";
            this.btnS1F1.Size = new System.Drawing.Size(75, 23);
            this.btnS1F1.TabIndex = 4;
            this.btnS1F1.Text = "S1F1";
            this.btnS1F1.UseVisualStyleBackColor = true;
            this.btnS1F1.Click += new System.EventHandler(this.btnS1F1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(111, 384);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 18);
            this.button1.TabIndex = 5;
            this.button1.Text = "S6F11";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(348, 424);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "dbTEST";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 501);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnS1F1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btExit);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cassette Cleaner";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbConnection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbControlStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btControlStatus;
        private System.Windows.Forms.Button btnS1F1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
    }
}

