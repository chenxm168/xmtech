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
    public partial class MatchResult : Form
    {
        public MatchResult()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }


        public static void Display(bool result,string id)
        {
            MatchResult fr = new MatchResult();
            if(!result)
            {
                fr.lbResult.Text=id+"\n匹配结果：OK";
                fr.lbResult.ForeColor = Color.Green;
            }else
            {
                fr.lbResult.Text =id+ "\n匹配结果：NG";
                fr.lbResult.ForeColor = Color.Red;
            }

            fr.Show();
        }
    }
}
