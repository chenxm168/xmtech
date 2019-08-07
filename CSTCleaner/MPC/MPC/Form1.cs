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
using EQPIO;
using TIBMessageIo;
using TIBMessageIo.MessageSet;
using HF.DB;
using HF.DB.ObjectService;
using log4net;
using MPC.Server.EQP;

namespace MPC
{
    public partial class Form1 : Form 
    {
        //private XmlApplicationContext xtc = new XmlApplicationContext("spring-objects.xml");
        private ILog logger = LogManager.GetLogger(typeof(Form1));
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //var c = ObjectManager.getObject("portStatusChange") as TIBMessageIo.MessageInfo;
            ////c.test();
            ////Class1.EQPIOTest();
            //var Sender = ObjectManager.getObject("sender") as ISendable;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ////Class1.CloseEQPIO();
            //var m = AreYouThereRequest.getInstance();
           // var Tsensder = ObjectManager.getObject("TibSender") as ISendable;
            ////Tsensder.Send(m);

            //var port = new HF.DB.ObjectService.Type1.Pojo.Port();

            //port.EquipmentName = "C1CCL01";
            //port.PortName = "PU01";
            //port.PortNo = 1;
            //port.GradePriority=0;
            //port.HighLimit=0;
            //port.LowLimit=0;
            //port.LocalNo=2;
            //port.PortEnableMode="Enable";
            //port.PortQtime = 10;
            //port.PortMode = "TFT";
            //port.PortStatus = "UnloadRequest";
            //port.PortTransferMode = "StockerInline";
            //port.PortType = "Unloader";
            //port.PortTypeAutoMode = "Disable";
            //port.Description = "PORT#2";
            //port.CurrentStatusTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //port.CassetteType = "NORMAL";

            //var s = ServiceManager.GetPortService() as HF.DB.ObjectService.Type1.Service.IPortService;
            //var keys = new Dictionary<string,object>();
            //keys.Add("EQUIPMENTNAME","C1CCL01");
            //keys.Add("PORTNAME","PL01");
            //var port = s.FindByKey(keys, null, false);
            //if(port==null)
            //{
            //    return;
            //}
           
            //port.PortStatus = "LoadRequest";
            //port.CurrentStatusTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            //s.UpdatePort(port, "LoadRequest");
            //((IDisposable)s).Dispose();

            var rs = ObjectManager.getObject("plcRequest") as PLCRequest;

            rs.SendRequest("L2_BIT_AllPortState", "R");
        }

        public void Init()
        {
            //lbConnection.Text = "Disconnection";
            //lbConnection.BackColor = Color.Red;
            var etHander = ObjectManager.getObject("m_EQPEventProcess") as Server.EQP.EQPEventHandler;
            etHander.OnConnected += OnEQPConnection;
            etHander.OnDisconnected += OnEQPDisconnection;
            var svr = ObjectManager.getObject("serverManager") as Server.ServerManager;
            var eqSvr = ObjectManager.getObject("controlManager") as IDisposable;
            var TibSvr = ObjectManager.getObject("TibSender") as IDisposable;
            var DbSvr = DBHelperManager.GetDBHelper() as IDisposable;
            svr.DisposbleObjectList.AddRange(new IDisposable[] { eqSvr, TibSvr, DbSvr });
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
            setControlStatus();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
           // Server.ControlStatusHandler.ControlStatusChangeToOffline();
            var svr = ObjectManager.getObject("serverManager") as Server.ServerManager;
            svr.Dispose();
        }

        private void setControlStatus()
        {
            HF.DB.ObjectService.Type1.Service.IEquipmentService svr = HF.DB.ObjectService.ServiceManager.GetEquipmentService();
            var eq = svr.FindAll().FirstOrDefault<HF.DB.ObjectService.Type1.Pojo.Equipment>();
            var status = eq.OnlineControlStatus.ToUpper().Trim();

            for(int i=0;i<cbControlStatus.Items.Count;i++)
            {
                var s = cbControlStatus.Items[i] as string;
                if(s.ToUpper().Trim().Equals(status))
                {
                    cbControlStatus.SelectedIndex = i;
                    return;
                }
            }
        }

        private void btControlStatus_Click(object sender, EventArgs e)
        {
            switch(cbControlStatus.Text.Trim().ToUpper())
            {
                case "OFFLINE":
                    Server.ControlStatusHandler.ControlStatusChangeToOffline();
                    break;
                case "REMOTE":
                    Server.ControlStatusHandler.ControlStatusChangeToOnline();
                    break;

                case "LOCAL":
                    Server.ControlStatusHandler.ControlStatusChangeToLocal();
                    break;
            }

            setControlStatus();
        }


        private void OnEQPConnection(object sender, object[] args)
        {
            logger.Debug("EQP Connection!");
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Server.MessageEventHandler(EQPConnection), new object[] {this,args });
                }
                else
                {
                    EQPConnection(sender, args);
                }

            }catch(Exception e)
            {

            }
        }

        private void OnEQPDisconnection(object sender, object[] args)
        {
            logger.Debug("EQP Disconnection!");
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Server.MessageEventHandler(EQPDisconnection), new object[] { this, args });
                }
                else
                {
                    EQPDisconnection(sender, args);
                }

            }
            catch (Exception e)
            {

            }
        }

        private void EQPConnection(object sender, object[] args)
        {
            lbConnection.BackColor = Color.Green;
            lbConnection.Text = "Connection";
        }

        private void EQPDisconnection(object sender, object[] args)
        {
            lbConnection.BackColor = Color.Red;
            lbConnection.Text = "Disconnection";
        }
        
    }
}
