using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using EQPIO.Controller;
using EQPIO.Controller.Proxy;
using EQPIO.MessageData;
using EQPIO.Common;


namespace EQPIO.Controller
{
  public abstract class AbsCommandService
    {

        protected ILog logger = LogManager.GetLogger(typeof(AbsCommandService));

        public ControlManager PLCManager
        {
            get;
            set;
        }

        protected BlockMap getBlockMap(string local)
        {

            BlockMap blockMap = null;

            if (PLCManager.UseEthernet)
            {
                PLCMap map;
                if (PLCManager.getMProtocolProxy().MapList.TryGetValue(local, out map))
                {
                    blockMap = map.blockMap;

                }
                else
                {
                    return null;
                }

            }
            if (PLCManager.UseBoard)
            {
                blockMap = PLCManager.getMNetProxy().EQPPlcMap.blockMap;
            }

            return blockMap;
            

        }
       protected BlockMap getBlockMap()
        {
            return getBlockMap("L2");
        }
    }
}
