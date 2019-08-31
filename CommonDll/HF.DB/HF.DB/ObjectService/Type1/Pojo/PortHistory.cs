using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB;

namespace HF.DB.ObjectService.Type1.Pojo
{
    public class PortHistory
    {
        [ColummAttribute(PrimaryKey=true)]
        public string ObjectNo
        {
            get;
            set;
        }
        public string EventName    
        {
            get;
            set;
        }

        public string HistoryTime
        {
            get;
            set;
        }

        public string MachineName //check
        {
            get;
            set;
        }

        //public string EquipmentName
        //{
        //    get;
        //    set;
        //}

        public string PortName  //check
        {
            get;
            set;
        }

        public int BCRExist  //Check
        {
            get;
            set;
        }

        public int Capacity  //Check
        {
            get;
            set;
        }

        public int CassetteSeqNo  //check
        {
            get;
            set;
        }

        public string CassetteType  //check
        {
            get;
            set;
        }

        public string CurrentStatusTime //check
        {
            get;
            set;
        }

        public int GradePriority  //check
        {
            get;
            set;
        }

        public int HighLimit //
        {
            get;
            set;
        }

        public int LocalNo //
        {
            get;
            set;
        }

        public int LowLimit //
        {
            get;
            set;
        }

        public int MappingUnitExist //
        {
            get;
            set;
        }

        public string PortEnableMode   //
        {
            get;
            set;
        }

        public string PortGrade //
        {
            get;
            set;
        }

        public string PortMode //
        {
            get;
            set;
        }

        public int PortNo  //
        {
            get;
            set;
        }

        public int PortQtime  //
        {
            get;
            set;
        }

        public string PortStatus  //
        {
            get;
            set;
        }

        public string PortTransferMode //
        {
            get;
            set;
        }

        public string PortType  //
        {
            get;
            set;
        }

        public string PortTypeAutoMode  //
        {
            get;
            set;
        }

        public string PortUseType //
        {
            get;
            set;
        }

        public string CarrierId
        {
            get;
            set;
        }

        public string Description  //
        {
            get;
            set;
        }
    }
}
