using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPC.LCForms
{
    public partial class LCMachineMainForm : Form
    {
        public LCMachineMainForm()
        {
            InitializeComponent();

            setFromWidth();
            setFormposition();

        }

        private void  setFromWidth( )
        {
            System.Drawing.Rectangle rec = Screen.GetWorkingArea(this);

            int SH = rec.Height;

            int SW = rec.Width;
            this.Width = SW; // 设置窗体宽度
            this.Height = SH; // 设置窗体高度 */
            

        }

        private void setFormposition()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "LC Mchine";
        }
    }
}
