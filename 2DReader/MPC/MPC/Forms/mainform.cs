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
using MPC;
using TCPSrv.Reader;
using FileSrv.csv;
using FileDataLoader;
using log4net;
using FileDataLoader.FileUpload;
using FileDataLoader.DBSrv;

namespace MPC.Forms
{
    public partial class mainform : Form
    {

        ILog logger = LogManager.GetLogger(typeof(mainform));

        bool defectBySheet = true;
        bool IsConnected = false;
        bool IsReading = false;
        string preProdSpec = "";
        string preDefectCode = "";

        ModReadPanelInfo pInfo = new ModReadPanelInfo();

        IVCReader reader;

        List<string> txList = new List<string>();

        Queue<String> queue = new Queue<string>();

        public mainform()
        {
            InitializeComponent();
            Init();

            
        }


        private void Init()
        {
          VCReaderTCP rd =  ObjectManager.getObject("reader") as VCReaderTCP ;

          rd.ConnectedSuccessEvent += this.ConnectSuccessEventInvoke;
          rd.DisconnectedEvent += this.DisconnectSuccessEventInvoke;
          rd.ConnectedFailEvent += this.ConnectFailEventInvoke;
          rd.ReadStartEvent += this.ReadStartEventInvoke;
          rd.ReadStopEvent += this.ReadStopEventInvoke;
          rd.ReadSuccess += this.ReadSuccessHandlerInvoke;
          rd.ReadFail += this.ReadFailHandlerInvoke;

          this.ConnectStateChange();
          txIpaddress.Text = rd.Config.RemoteIp;
          txIpPort.Text = rd.Config.TcpPort.ToString();
          txIformation.Clear();
          if(rbSelectDefectBySheet.Checked==true)
          {
              btDefectChange.Enabled = false;
          }


        }

        private void rbSelectDefectForSheets_CheckedChanged(object sender, EventArgs e)
        {
            this.btDefectChange.Enabled = true;
            defectBySheet = true;
            if(lbDefectCode.Text.Trim().Length<1&&!rbSelectDefectBySheet.Checked)
            {
                DefectInput fm = new DefectInput();
                if(fm.ShowDialog()==DialogResult.OK)
                {
                    string sCodeName = fm.DefectCodeAndName;
                    if (sCodeName != null && sCodeName.Length > 0)
                    {
                        string[] ss = sCodeName.Split(new char[] { ',' });
                        if (ss.Length > 0)
                        {
                            this.lbDefectCode.Text = ss[0];
                        }

                        if (ss.Length > 1)
                        {
                            this.lbDefectName.Text = ss[1];
                        }
                    }
                }
            }
        }



        private void rbSelectDefectBySheet_CheckedChanged(object sender, EventArgs e)
        {
            this.btDefectChange.Enabled = false;
            defectBySheet = false;
        }

        private void rbReadBySheet_CheckedChanged(object sender, EventArgs e)
        {
            this.ConnectStateChange();
            

        }

        private void rbCycleRead_CheckedChanged(object sender, EventArgs e)
        {
            this.ConnectStateChange();
        }


        private void ConnectSuccessEventInvoke(object sender,object args)
        {
            try
            {
                if(this.InvokeRequired)
                {
                    this.Invoke(new EventHandler(ConnectSuccessEvent), new object[] { sender ,args});
                }else
                {
                    ConnectSuccessEvent(sender, args);
                }

            }catch(Exception e)
            {

            }

        }

        private void ConnectSuccessEvent(object sender,object e)
        {
            //this.btConnect.Text = "已连接";
            //this.btConnect.BackColor = Color.Gold;
            //this.btConnect.Enabled = true;
            this.IsConnected = true;
            this.ConnectStateChange();
            UpdateText("VCR 连接成功！");
        }

        private void DisconnectSuccessEventInvoke(object sender, object args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new EventHandler(DisconnectSuccessEvent), new object[] { sender, args });
                }
                else
                {
                    DisconnectSuccessEvent(sender, args);
                }

            }
            catch (Exception e)
            {

            }

        }

        private void DisconnectSuccessEvent(object sender, object e)
        {
            if(this.IsConnected)
            {
                this.IsConnected = false;
                this.ConnectStateChange();
                UpdateText("VCR 已断开连接！");
            }

        }

        private void ConnectFailEventInvoke(object sender, object args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new EventHandler(ConnectFailEvent), new object[] { sender, args });
                }
                else
                {
                    ConnectFailEvent(sender, args);
                }

            }
            catch (Exception e)
            {

            }
        }

        private void ConnectFailEvent(object sender, object args)
        {
            this.btConnect.BackColor = System.Drawing.SystemColors.Control;
            this.btConnect.Enabled = true;
            this.IsConnected = false;
            this.btConnect.Text = "请连接";  
            VCREeventArgs arg= (VCREeventArgs) args;
            MessageBox.Show("连接失败！请检查VCR电源或VCR配置后重新连接！", "VCR Connect fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            if(IsConnected)
            {
                return;
            }

            this.btConnect.Enabled = false;
            this.btConnect.Text = "连接中...";

            VCReaderTCP rd = ObjectManager.getObject("reader") as VCReaderTCP;
            rd.ConnectAsyn();
        }


        private void ConnectStateChange()
        {
            if(IsConnected)
            {
               //if( rbReadBySheet.Checked)
               //{
               //    btReadStop.Enabled = false;
               //    btReadStart.Enabled = false;
               //    btRead.Enabled = true;
               //}else
               //{
               //    btReadStop.Enabled = true;
               //    btReadStart.Enabled = true;
               //    btRead.Enabled = false ;

               //}
                this.btConnect.Text = "已连接";
                this.btConnect.BackColor = Color.Gold;
                this.btConnect.Enabled = true;
                btRead.Enabled = true;
            }else
            {
                btReadStop.Enabled = false;
                btReadStart.Enabled = false;
                btRead.Enabled = false;

                this.btConnect.BackColor = System.Drawing.SystemColors.Control;
                this.btConnect.Enabled = true;
                this.btConnect.Text = "请连接";  

            }
        }


        private void ReadStartEventInvoke(object sender ,EventArgs args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new EventHandler(ReadStartEvent), new object[] { sender, args });
                }
                else
                {
                    ReadStartEvent(sender, args);
                }

            }
            catch (Exception e)
            {

            }
        }

        private void ReadStopEventInvoke(object sender, EventArgs args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new EventHandler(ReadStopEvent), new object[] { sender, args });
                }
                else
                {
                    ReadStopEvent(sender, args);
                }

            }
            catch (Exception e)
            {

            }
        }

        private void ReadStartEvent(object sender ,EventArgs args)
        {
            IsReading = true;
            ReadStateChange(IsReading);

        }

        private void ReadStopEvent(object sender, EventArgs args)
        {
            IsReading = false;
            btRead.Enabled = true;
            ReadStateChange(IsReading);
        }

        private void btRead_Click(object sender, EventArgs e)
        {
            IVCReader rd = ObjectManager.getObject("reader") as IVCReader;
            if(IsReading)
            {
                rd.StopRead();
            }else
            {
                rd.StartRead();
                //btRead.Enabled = false;
            }
        }

        private void ReadStateChange(bool reading)
        {
            if(rbReadBySheet.Checked)
            {
                if(reading)
                {
                    btRead.BackColor = Color.Gold;
                    btRead.Enabled = true;
                }else
                {
                    btRead.BackColor = System.Drawing.SystemColors.Control;
                    btRead.Enabled = true;
                }
            }
            if(rbCycleRead.Checked)
            {

            }
        }

        private void btDefectChange_Click(object sender, EventArgs e)
        {
            fixedDefectCodeChange();
        }


        private void fixedDefectCodeChange()
        {
            DefectInput fm = new DefectInput();
            if (fm.ShowDialog() == DialogResult.OK)
            {

                string sCodeName = fm.DefectCodeAndName;
                if (sCodeName != null && sCodeName.Length > 0)
                {
                    string[] ss = sCodeName.Split(new char[] { ',' });
                    if (ss.Length > 0)
                    {
                        this.lbDefectCode.Text = ss[0];
                    }

                    if (ss.Length > 1)
                    {
                        this.lbDefectName.Text = ss[1];
                    }
                }



            }
        }

        private void btReadStart_Click(object sender, EventArgs e)
        {


        }

        private void btUpate_Click(object sender, EventArgs e)
        {
            //var wt = ObjectManager.getObject("csvwriter") as CSVWriter;

            //wt.WriteNewFileAsyn("id,LB,漏笔,N");
        }


        private void writeDefectToCsv(string id,string defectCodeAndName)
        {
            string s = id.ToUpper().Trim() + "'" + defectCodeAndName + "," + "N";
            var wt = ObjectManager.getObject("csvwriter") as CSVWriter;
            wt.AppendFileAsyn(s);
            //wt.WriteNewFileAsyn("id,LB,漏笔,N");
        }

       private void ReadFailHandlerInvoke(object sender,VCREeventArgs args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    //this.Invoke(new EventHandler(ReadSuccessHandler), new object[] {sender, args});
                    this.Invoke(new EventHandler(ReadFailHandler), new object[] { sender, args });
                }
                else
                {
                    //ReadStopEvent(sender, args);

                    ReadFailHandler(sender, args);
                }

            }
            catch (Exception e)
            {

            }
        }

        private void ReadFailHandler(object sender, object args)
       {
           UpdateText("读取失败!"); 
       }


        private void ReadSuccessHandlerInvoke(object sender,VCREeventArgs args)
        {
            string id =args.Message;

            try
            {
                if (this.InvokeRequired)
                {
                    //this.Invoke(new EventHandler(ReadSuccessHandler), new object[] {sender, args});
                   //20200929 this.Invoke(new EventHandler(ReadSuccessHandler3), new object[] { sender, args });

                    //for module read 
                    this.Invoke(new EventHandler(ReadSuccessHandler4), new object[] { sender, args });
                }
                else
                {
                    //ReadStopEvent(sender, args);

                    //20200929 ReadSuccessHandler3(sender, args);
                    ReadSuccessHandler3(sender, args);
                }

            }
            catch (Exception e)
            {

            }
            
        }

        private void ReadSuccessHandler(object sender,object arg)
        {
            var wt = ObjectManager.getObject("csvwriter") as CSVWriter;
            string sItem = "";
            var args = arg as VCREeventArgs;
            string vcrid = args.Message.Trim().ToUpper();
            string panelid="";
            /*
             * moidfy by cxm 20200806
             * 
            if (rbSelectDefectBySheet.Checked)
            {
                DefectInput di = new DefectInput();
                string defectCodeAndName = "";
                if (di.ShowDialog() == DialogResult.OK)
                {
                    defectCodeAndName = di.DefectCodeAndName;
                }




                sItem = id + "," + defectCodeAndName + "," + "N";
            }
            else
            {
                sItem = id + "," + lbDefectCode.Text.Trim() + "," + lbDefectName.Text + "," + "N";
            }
             */

            if (rbSelectDefectBySheet.Checked)
            {
                frmPanelInfo frm = new frmPanelInfo();
                frm.PreProductSpec = this.preProdSpec;
                frm.PreDefectCode = this.preDefectCode;
                frm.VCRID = vcrid;

                if(frm.ShowDialog()==DialogResult.OK)
                {
                    sItem = sItem + DateTime.Now.ToString("yyyyMMddHHmmss") + ",";
                    sItem = sItem + frm.ProductSpec + ",";
                    this.preProdSpec = frm.ProductSpec;
                    string tem = frm.ProductSpec.Substring(0, 2);
                    panelid = tem + vcrid;
                    string lotid = panelid.Substring(0, panelid.Length - 3);
                    string glassid = panelid.Substring(0, panelid.Length - 2);
                    sItem = sItem + panelid + ",";
                    //sItem = sItem + glassid + ",";
                    
                    //sItem = sItem + tem + vcrid + ",";
                    sItem = sItem + vcrid + ",";

                    sItem = sItem + frm.DefectCode + ",";
                    sItem = sItem + Convert.ToInt16(frm.IsCell) + ",";
                    sItem = sItem + Convert.ToInt16(frm.IsLB) + ",";
                    sItem = sItem + Convert.ToInt16(frm.IsZH) + ",";
                    Dictionary<int, string> map = frm.dtImage;
                    if(map.Count>0)
                    {
                        sItem = sItem + Utils.getLocalImagePath(map) + ",";
                        sItem = sItem + Utils.getImages(map) + ",";

                        string ftppath = "";
                        string penelid =vcrid+ frm.ProductSpec.Substring(1, 2);
                        //string lotid = penelid.Substring(0, 9);
                        //string glassid = penelid.Substring(0, 10);
                        ftppath = "VCR\\" + frm.ProductSpec.ToUpper().Trim() + "\\" + lotid.ToUpper() + "\\" + glassid.ToUpper()  ;
                        sItem = sItem + ftppath + ",";


                    }else
                    {
                        sItem = sItem + ",,,";
                    }

                    var db = ObjectManager.getObject("db");
                   // var uploader = ObjectManager.getObject("uploader");

                    if(Utils.UploadDefectData(db,sItem))
                    {
                        sItem = sItem + "Y";
                    }

                    else
                    {
                        sItem = sItem + "N";
                    }

                    

                }
            }
            
            wt.AppendFileAsyn(sItem);

            UpdateText(sItem);

        }


        private void ReadSuccessHandler2(object sender,object arg)
        {
            var glass = ObjectManager.getObject("glass") as GlassDefectColection;
            
            string[] gl=   glass.DefectGlasses;

            VCREeventArgs args = arg as VCREeventArgs;
            string id = args.Message.Substring(0, 8);
            UpdateText(id);

            var rst = (from r in gl where r.Contains(id.Trim()) select r).ToArray<string>();

            if(rst.Length>0)
            {
                MatchResult.Display(true, id);
            }else
            {
                MatchResult.Display(false, id);
            }



        }

        private void  ReadSuccessHandler3(object sender,object arg)
        {
            if(rbReadBySheet.Checked)
            {
                //缺陷录入模式
                ReadSuccessHandler(sender, arg);

            }else
            {
                //筛选出需要的PANEL ID
                ReadSuccessHandler2(sender,arg);
            }
        }


        //for PGM read panel id

        private void ReadSuccessHandler4(object sender, object arg)
        {
            string sItem = "";
            var args = arg as VCREeventArgs;
            string vcrid = args.Message.Trim().ToUpper();

            string dtcode = "";

            if(rbSelectDefectBySheet.Checked)
            {
                DefectInput frm = new DefectInput();
                if(frm.ShowDialog()==DialogResult.OK)
                {
                    string cn = frm.DefectCodeAndName;
                    dtcode = cn.Split(',')[0];
                }else
                {
                    return;
                }
            }
            else
            {
                if (!chkDefectCode(vcrid))
                {
                    return;
                }

                dtcode = lbDefectCode.Text;

            }


            if (!chkProdSpec(vcrid))
            {
                return;
            }

          


                UploadModDefectData(txProductSpec.Text, vcrid, dtcode);


                //if(lbDefectCode.Text.Length<1)
                //{
                //    btDefectChange_Click(null,null);
                //    if(lbDefectCode.Text.Length<1)
                //    {
                //        for(;;)
                //        {
                //          if(  MessageBox.Show("如果放弃本次读取数据，请点取消，如果不，请点OK", "必须输入缺陷代码", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)==DialogResult.Cancel)
                //          {
                //              return;
                //          }else
                //          {
                //              btDefectChange_Click(null, null);
                //          }
                //        }
                //    }
                //}
            
        }


        private void mainform_FormClosing(object sender, FormClosingEventArgs e)
        {
          var reader = ObjectManager.getObject("reader") as VCReaderTCP;
          reader.StopRead();

        }

        private void UpdateText(string message)
        {
            for(;;)
            {
                //if(txList.Count>10)
                //{
                //    txList.RemoveAt(0);
                //}else
                //{
                //    break;
                //}

                if(queue.Count>10)
                {
                    queue.Dequeue();
                }
                else
                {
                    break;
                }
            }

           // txList.Add(DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss:fff ")+message);
            queue.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff ") + message);
            txList.Clear();
            txList.AddRange(queue.ToArray<string>());

            List<string> list = new List<string>();
            string tx = "";

            //for (int i = txList.Count-1; i >= 0; i--)
            //{
            //   // tx +=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ")+ txList[i] + "\n";
            //    list.Add(DateTime.Now.ToString( + txList[i]);
            //   list=  txList.Reverse
            //}
           txList.Reverse(0, txList.Count);
           list = txList;
            txIformation.Clear();
            //txIformation.Text = tx;
            txIformation.Lines = list.ToArray<string>();
            
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            
                
            
            

            //var cfg = ConfigLoader.getConfigInstance("Config\\VcrConfig.ini");
            //string s1= cfg.getParam("ConnString1");

            //string s21 = cfg.getParam("ConnString2");

            //string s3 = cfg.getAsciiParam("ConnString3");

            //string cs1 = cfg.getAsciiParam("ConnString1");
            //string cs2 = cfg.getAsciiParam("ConnString2");


            //getAsciiString(s1);
            //getAsciiString(s21);







            VCREeventArgs args = new VCREeventArgs("12E4606422");
            ReadSuccessHandler4(this, args);

            //Utils.ProductSpecValidation("A01A-084WD");

            //frmPanelInfo frm = new frmPanelInfo();
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    Utils.getImages(frm.dtImage);
            //}
            //else
            //{

            //}
        }

        private string getAsciiString(string str)
        {
            string s2 = "";
            byte[] bs = System.Text.Encoding.ASCII.GetBytes(str);
            foreach (byte b in bs)
            {
                s2 += Convert.ToString(b, 16).PadLeft(2, '0');
            }

            logger.Debug(s2);

            return s2;

        }

        private void txProductSpec_Validating(object sender, CancelEventArgs e)
        {
            System.Console.WriteLine("test validating");
        }

        private void txProductSpec_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar))
            {
                
                e.Handled = false;
            }else
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


        private void UploadModDefectData(string productSpec,string VCRPanelID,string defectcode)
        {
            var wt = ObjectManager.getObject("csvwriter") as CSVWriter;
            string sql = "";
            switch (pInfo.ComparePrevious(VCRPanelID,defectcode,productSpec))
            
            {
                case 1 :
                    {
                        

                         sql = string.Format("UPDATE EDA_MODPANELJUDGE SET DEFECTCODE='{0}',TIMEKEY='{1}' WHERE GLASSID='{2}' ", pInfo.CrPanel[CNS.DEFECTCODE_IDX],
                            pInfo.CrPanel[CNS.TIMEKEY_IDX], pInfo.CrPanel[CNS.GLASSID_IDX]
                            );


                        break;
                    }
                case 2:
                    {

                       
                         sql = string.Format("INSERT INTO EDA_MODPANELJUDGE ( GLASSID,PANELID,VCRPANELID,DEFECTNO,DEFECTCODE,PRODUCTSPECNAME,TIMEKEY,DESCRIPTION ) VALUES ( "+
                       "'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}' )",pInfo.CrPanel[CNS.GLASSID_IDX],pInfo.CrPanel[CNS.PANELID_IDX],pInfo.CrPanel[CNS.VCRPANELID_IDX],pInfo.CrPanel[CNS.DEFECTNO_IDX],
                        pInfo.CrPanel[CNS.DEFECTCODE_IDX],pInfo.CrPanel[CNS.PRODUCTSPEC_IDX],pInfo.CrPanel[CNS.TIMEKEY_IDX],pInfo.CrPanel[CNS.DESCRIPTION_IDX]);



                        break;
                    }

                default:
                    {
                        return;
                    }


            }//END switch

            

            IDBService db = ObjectManager.getObject("db") as IDBService;

            if (db.ExcNonQuerySql(sql))
            {
                pInfo.CrPanel[CNS.UPLOADDBFLAG_IDX] = "Y";
            }
            wt.AppendFileAsyn(pInfo.CrPanelToString());
            UpdateText(pInfo.CrPanelToString());




        }//end UploadModDefectData


        private bool chkProdSpec(string vcrid)
        {

            if (txProductSpec.Text.Length < 3)
            {
                MessageBox.Show(string.Format("型号最小要三位数，001至Z99"                          
                           ), "型号有错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            bool isProductSpecOK = false;
            for (; ; )
            {
                if (txProductSpec.Text.Length == 3)
                {
                    //Compare productspec  char with vcrid
                    if (txProductSpec.Text.Substring(1, 2) == vcrid.Substring(0, 2))
                    {
                        isProductSpecOK = true;
                        break;
                    }
                    else
                    {
                     if(   MessageBox.Show(string.Format("型号的第2，3位数：[{0}]与VCR读取的PANELID的前两位数:[{1}]不符！请重新输入！",
                            txProductSpec.Text.Substring(1, 2), vcrid.Substring(0, 2)
                            ), "型号有错误", MessageBoxButtons.OK, MessageBoxIcon.Error)==DialogResult.Cancel)
                     {
                        
                         return false;
                     }
                    }
                }

                frmProductSpecInput frm = new frmProductSpecInput();
                frm.PanelID = vcrid;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txProductSpec.Text = frm.ProductSpec;
                    continue;
                }
                else
                {
                    if (MessageBox.Show("请完善产品型号信息，如果放弃本次读取信息，请点Cancel，继续请点OK", "必须完善产品型号", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        continue;
                    }
                    else
                    {
                        isProductSpecOK = false;
                        break;
                    }
                }
            }//end for

            return isProductSpecOK;
   

        } //end function


        private bool chkDefectCode(string vcrid)
        {
            bool isDefectCodeOK = false;
            
                for (; ; )
                {

                    if (lbDefectCode.Text.Length >0)
                    {
                        return true;
                    }


                    fixedDefectCodeChange();
                    if (lbDefectCode.Text.Length > 0)
                    {
                        isDefectCodeOK = true;
                        break;
                    }
                    else
                    {
                        if (MessageBox.Show("如果放弃本次读取数据，请点取消，如果不，请点OK", "必须输入缺陷代码", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }//end for




            

            return isDefectCodeOK;




        }//end function



    }
}
