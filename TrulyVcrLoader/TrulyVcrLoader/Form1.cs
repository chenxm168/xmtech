using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrulyVcrLoader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TrulyVcr vcr = new TrulyVcr(@"D:\桌面\VCR");
           
        }
    }
}
