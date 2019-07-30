using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace MPC.Server
{
    public class ServerManager : IDisposable
    {
        private ILog logger = LogManager.GetLogger(typeof(ServerManager));
        private List<IDisposable> disposbleObjectList;

        public List<IDisposable> DisposbleObjectList
        {
            get { return disposbleObjectList; }
            set { disposbleObjectList = value; }
        }

        public void Init()
        {
            logger.Debug("Initial Server");
            // var info = ObjectManager.getObject("tibMessageInfo") as TIBMessageIo.MessageInfo;
            // var map = ObjectManager.getObject("trxMapping") as BCServer.TrxMapping;

            //StaticVarible.DefauleSourceSubject= info.SourceSubject;
            //StaticVarible.DefauleTargetSubject = info.TargetSubject;
            //StaticVarible.MachineID = map.Equipment;

            //var s = ObjectManager.getObject("HostIO") as IDisposable;
            //disposbleObjectList.Add(s);
            //var cm = ObjectManager.getObject("controlManager") as IDisposable;

            //disposbleObjectList.Add(cm);

        }

        public ServerManager()
        {
            disposbleObjectList = new List<IDisposable>();
        }

        public void Dispose()
        {
            foreach (IDisposable d in disposbleObjectList)
            {
                if (d != null)
                {
                    d.Dispose();
                }
            }
        }
    }
}
