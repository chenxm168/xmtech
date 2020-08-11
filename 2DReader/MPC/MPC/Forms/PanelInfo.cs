using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
namespace MPC.Forms
{
    public partial class frmPanelInfo : Form
    {
        public string VCRID = "";
        public string PreProductSpec = "";
        public bool IsCell = true;
        public bool IsLB = true;
        public bool IsZH = false;
        public string DefectCode ="";
        public string[] ImageFiles = null;
        public string PreDefectCode = "";
        public string ProductSpec = "";

        private PictureBox[] pbList = new PictureBox[6];
        int clickIdx = 0;

        public Dictionary<int, string> dtImage = new Dictionary<int, string>();
        
        public frmPanelInfo()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            txProductSpec.Text = PreProductSpec;
            lbVCRID.Text = VCRID;
            cbIsCell.Checked = IsCell;
            cbIsLB.Checked = IsLB;
            cbIsZH.Checked = IsZH;
            
            
            var pl = ObjectManager.getObject("prodlist") as ProductSpecColection;
            var list = pl.SpecList;
            var source = new AutoCompleteStringCollection();
            source.AddRange(list);
            txProductSpec.AutoCompleteCustomSource = source;
            txProductSpec.AutoCompleteMode = AutoCompleteMode.Append;
            txProductSpec.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.ActiveControl = txProductSpec;
            if (!txProductSpec.Focused)
            {
              var t=   txProductSpec.Focus();
            }

            var dfs = ObjectManager.getObject("defects") as DefectCollection;
            var dfs2 = dfs.getCodeAndNameString();

            var source2 = new AutoCompleteStringCollection();
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

            source2.AddRange(dfs2);


            txDefectCode.AutoCompleteCustomSource = source2;


            txDefectCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txDefectCode.AutoCompleteSource = AutoCompleteSource.CustomSource;

            pbList[0] = pbImage1;
            pbList[1] = pbImage2;
            pbList[2] = pbImage3;
            pbList[3] = pbImage4;
            pbList[4] = pbImage5;
            pbList[5] = pbImage6;


        }

        private void pbImage1_Click(object sender, EventArgs e)
        {

           

            pbImage_click(0,e);

        }

        private void pbImage_click(int idx,EventArgs e)
        {
            clickIdx = idx;
            MouseEventArgs ev = e as MouseEventArgs;
            if (ev.Button == MouseButtons.Left)
            {
                string[] files = selectImage();
                if (files != null)
                {
                    this.ImageFiles = files;



                    for (int i = 0; i < files.Length; i++)
                    {
                        if (i + clickIdx > pbList.Length)
                        {
                            break;
                        }
                        string tempt;
                       if(   dtImage.TryGetValue(i + clickIdx,out tempt))
                        {
                            dtImage.Remove(i + clickIdx);
                        }
                        dtImage.Add(i + clickIdx, files[i]);

                       // pbList[i + clickIdx].Image = Image.FromFile(images[i]);
                    }



                    setImage(dtImage);
                }
            }
            if (ev.Button == MouseButtons.Right)
            {
                

                removeImage(clickIdx);
            }
        }


        private string[] selectImage()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Multiselect = true;
            op.Filter = "所有文件|*.JPG|*.pgn|*.jpeb";
            op.FilterIndex = 1;
           // op.InitialDirectory =
            if(op.ShowDialog()==DialogResult.OK)
            {
                return op.FileNames;
            }else
            {
                return null;
            }
        }

        private void setImage(Dictionary<int,string> dt)
        {
             foreach(KeyValuePair<int,string> en in dt)
             {
                 if(en.Value!=null||en.Value.Trim().Length>0)
                 {
                     Image image = Image.FromFile(en.Value);
                     pbList[en.Key].Image = image;
                 }
                 
             }

        }

        private void pbImage1_DoubleClick(object sender, EventArgs e)
        {
            clickIdx = 0;

            removeImage(0);
        }

        private void removeImage(int idx)
        {
            for (int i = 0; i < pbList.Length;i++ )
            {
                pbList[i].Image = null;
            }
                dtImage.Remove(idx);

            setImage(dtImage);
        }

        private void pbImage2_Click(object sender, EventArgs e)
        {
            pbImage_click(1, e);
        }

        private void pbImage3_Click(object sender, EventArgs e)
        {
            pbImage_click(2, e);
        }

        private void pbImage4_Click(object sender, EventArgs e)
        {
            pbImage_click(3, e);
        }

        private void pbImage5_Click(object sender, EventArgs e)
        {
            pbImage_click(4, e);
        }

        private void pbImage6_Click(object sender, EventArgs e)
        {
            pbImage_click(5, e);
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            lbDefectCodeErrinfo.Visible = false;
            lbSpecErrInfo.Visible = false;

            if(txProductSpec.Text.Trim().Length<1)
            {
                lbSpecErrInfo.Visible = true;
                
                txProductSpec.Focus();
            }else
            {
                if(Utils.ProductSpecValidation(txProductSpec.Text.Trim()))
                {

                    if(txDefectCode.Text.Trim().Length<1)
                    {
                        lbDefectCodeErrinfo.Visible = true;
                        txDefectCode.Focus();
                        return;
                    }
                    this.DefectCode = txDefectCode.Text;
                    this.ProductSpec = txProductSpec.Text.Trim();
                    this.IsCell = cbIsCell.Checked;
                    this.IsLB = cbIsLB.Checked;
                    this.IsZH = cbIsZH.Checked;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                lbSpecErrInfo.Visible = true;
                txProductSpec.Focus();
            }

           
            
        }

        private void frmPanelInfo_Load(object sender, EventArgs e)
        {
            txDefectCode.Text = this.PreDefectCode;
            lbVCRID.Text = this.VCRID;
        }

        private void txProductSpec_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void txProductSpec_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // this.SelectNextControl(this.ActiveControl, false, false, false, true);

                SendKeys.Send("{TAB}");
            }
        }

        private void frmPanelInfo_KeyUp(object sender, KeyEventArgs e)
        {


            if (e.KeyCode == Keys.Enter && Utils.ProductSpecValidation(txProductSpec.Text.Trim()))
            {
               // this.SelectNextControl(this.ActiveControl, false, false, false, true);

                SendKeys.Send("{TAB}");
            }
        }

        private void txDefectCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (txDefectCode.Text.Trim().Length > 0 && e.KeyCode == Keys.Enter)
               {
                   SendKeys.Send("{TAB}");
               }

        }

        private void cbIsCell_KeyUp(object sender, KeyEventArgs e)
        {
            //if(e.KeyCode==Keys.Space)
            //{
            //    cbIsCell.Checked = !cbIsCell.Checked;
            //}

            if(e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void cbIsLB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
            //if(e.KeyCode==Keys.Space)
            //{
            //    cbIsLB.Checked = !cbIsLB.Checked;
            //}
        }

        private void cbIsZH_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }

            //if (e.KeyCode == Keys.Space)
            //{
            //    cbIsZH.Checked = !cbIsZH.Checked;
            //}
        }


    }
}
