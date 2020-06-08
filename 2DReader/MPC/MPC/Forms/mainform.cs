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

namespace MPC.Forms
{
    public partial class mainform : Form
    {

        bool defectBySheet = true;
        bool IsConnected = false;
        bool IsReading = false;

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
            this.btConnect.Text = "已连接";
            this.btConnect.BackColor = Color.Gold;
            this.btConnect.Enabled = true;
            this.IsConnected = true;
            this.ConnectStateChange();
            UpdateText("VCR 连接成功！");
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
                btRead.Enabled = true;
            }else
            {
                btReadStop.Enabled = false;
                btReadStart.Enabled = false;
                btRead.Enabled = false;
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
            DefectInput fm = new DefectInput();
           if( fm.ShowDialog()==DialogResult.OK)
           {
               string sCodeName = fm.DefectCodeAndName;
               if(sCodeName!=null&&sCodeName.Length>0)
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
                    this.Invoke(new EventHandler(ReadSuccessHandler3), new object[] { sender, args });
                }
                else
                {
                    //ReadStopEvent(sender, args);

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
            string id = args.Message.Trim().ToUpper();
            if(rbSelectDefectBySheet.Checked)
            {
                DefectInput di = new DefectInput();
                string defectCodeAndName = "";
                if (di.ShowDialog() == DialogResult.OK)
                {
                    defectCodeAndName = di.DefectCodeAndName;
                }




                sItem = id + "," + defectCodeAndName + "," + "N";
            }else
            {
                sItem = id + "," + lbDefectCode.Text.Trim() + "," + lbDefectName.Text + "," + "N";
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
                ReadSuccessHandler(sender, arg);

            }else
            {
                ReadSuccessHandler2(sender,arg);
            }
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

    }
}
