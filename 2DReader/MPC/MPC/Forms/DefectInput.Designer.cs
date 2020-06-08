namespace MPC.Forms
{
    partial class DefectInput
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
            this.label1 = new System.Windows.Forms.Label();
            this.txDefectInput = new System.Windows.Forms.TextBox();
            this.btDefectSelect = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(54, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "请输入缺陷代码：";
            // 
            // txDefectInput
            // 
            this.txDefectInput.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txDefectInput.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txDefectInput.Location = new System.Drawing.Point(55, 102);
            this.txDefectInput.Name = "txDefectInput";
            this.txDefectInput.Size = new System.Drawing.Size(323, 26);
            this.txDefectInput.TabIndex = 0;
            // 
            // btDefectSelect
            // 
            this.btDefectSelect.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btDefectSelect.Location = new System.Drawing.Point(398, 102);
            this.btDefectSelect.Name = "btDefectSelect";
            this.btDefectSelect.Size = new System.Drawing.Size(79, 32);
            this.btDefectSelect.TabIndex = 2;
            this.btDefectSelect.Text = "选 择";
            this.btDefectSelect.UseVisualStyleBackColor = true;
            this.btDefectSelect.Visible = false;
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btOK.Location = new System.Drawing.Point(398, 201);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(79, 32);
            this.btOK.TabIndex = 3;
            this.btOK.Text = "确 定";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btCancel.Location = new System.Drawing.Point(268, 201);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(79, 32);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "取 消";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // DefectInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 262);
            this.ControlBox = false;
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btDefectSelect);
            this.Controls.Add(this.txDefectInput);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DefectInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "输入缺缺代码";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txDefectInput;
        private System.Windows.Forms.Button btDefectSelect;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
    }
}