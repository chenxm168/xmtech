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
    public partial class DefectInput : Form
    {

        public string DefectCodeAndName
        { get; set; }

        public DefectInput()
        {
            InitializeComponent();
            Init();
        }


        private void Init()
        {
            var dfs = ObjectManager.getObject("defects") as DefectCollection;
            var dfs2 = dfs.getCodeAndNameString();

              var source = new AutoCompleteStringCollection();
            //source.AddRange(new string[]
            //        {
            //            "January:星期一",
            //            "February",
            //            "March",
            //            "April",
            //            "May",
            //            "June",
            //            "July",
            //            "August",
            //            "September",
            //            "October",
            //            "November",
            //            "December"
            //        });

              source.AddRange(dfs2);

            txDefectInput.AutoCompleteCustomSource = source;


            txDefectInput.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txDefectInput.AutoCompleteSource = AutoCompleteSource.CustomSource;
            

        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if(txDefectInput.Text.Trim().Length>0)
            {
                //var defects = ObjectManager.getObject("defects") as DefectCollection;
                //defects.SetPriority(txDefectInput.Text);
                this.DefectCodeAndName = txDefectInput.Text;
            }else
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }






    }



    
}
