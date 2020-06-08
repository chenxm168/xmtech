using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spring.Context.Support;
using MPC.LAN;
using RS232Srv;
using TCPSrv.Reader;
using FileSrv.csv;
using System.Threading;
using log4net;

namespace MPC
{
    public partial class Form1 : Form
    {
        //private XmlApplicationContext xtc = new XmlApplicationContext("spring-objects.xml");
        private EventWaitHandle ewh = new EventWaitHandle(false, EventResetMode.AutoReset);
        ILog logger = LogManager.GetLogger(typeof(Form1));
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // var cd = ObjectManager.getObject("reader") as IReader;
            var c = ObjectManager.getObject("reader") as VCReaderTCP;
            c.ConnectAsyn();

            // c.Connect("127.0.0.0", 30000);
            //c.ConnectAsyn("192.168.0.60", 27110);
            //byte[] data = new byte[] { 103, 13 };
            //c.Open();
            //c.SendBytes(data);
            //byte[] rd;
            //byte terminator = 13;
            //c.ReceiveBytes(out rd, terminator, 30000);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var c = ObjectManager.getObject("reader") as ReaderTCP;
            //byte[] rtn;
            //c.Connect("192.168.0.60", 27110);
            //if (c.ReadIDOnice(out rtn))
            //{
            //    return;
            //}
            var c = ObjectManager.getObject("reader") as VCReaderTCP;

            if (c.IsConnected)
            {
                c.StartRead();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var c = ObjectManager.getObject("reader") as VCReaderTCP;
            c.StopRead();

            CSVWriter2 cw = new CSVWriter2();
            
            string[] items = new string[] { "ITEM1", "ITEM2", "ITEM3" };
            //cw.WriteCSV(items);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(doWork));
            t.Start();
        }


        private void doWork()
        {
            for(int i=0;i<1000;i++)
            {
                ewh.WaitOne();

                logger.Debug("Realease waitone");
                
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            ewh.Set();
        }
    }
}
