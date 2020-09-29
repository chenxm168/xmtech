using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPC.Forms
{
    public partial class frmProductSpecInput : Form
    {
        public string ProductSpec = "";
        public string PanelID = "";

        public frmProductSpecInput()
        {
            InitializeComponent();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if(txProductSpec.Text.Length!=3)
            {
                lbMessage.Text="产品型号长度必需等于3";
                txProductSpec.Focus();
                
            }
            if(txProductSpec.Text.Substring(1,2)!=PanelID.Substring(0,2))
            {
                lbMessage.Text = string.Format("产品型号第2位及第3位：[{0}] 与VCR读取PANELID:[{1}前2位:[{2}]不一致！\n请重新输入正确的ASD ARY产品型号",txProductSpec.Text.Substring(1,2),
                    this.PanelID,this.PanelID.Substring(0,2));
                txProductSpec.Focus();
                return;
            }
            this.ProductSpec = txProductSpec.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmProductSpecInput_Shown(object sender, EventArgs e)
        {
            if(ProductName.Length<1)
            {
                lbMessage.Text = "产品型号3位数为空，请输入ASD ARRAY产品型号!";
            }else
            {

                    lbMessage.Text = "请输入正确的ASD ARRAY产品型号3位数，001-Z99！";
                
                
            }

        }

        private void txProductSpec_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar))
            {

                e.Handled = false;
            }
            else
            {


                if (txProductSpec.Text.Length < 1)
                {
                    if ("Aa0123456789".Contains(e.KeyChar))
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    if ("0123456789".Contains(e.KeyChar))
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }

            }
        }
    }
}
