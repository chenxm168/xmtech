using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF.DB.ObjectService.Type1.Pojo
{
    public class Port
    {
        [ColummAttribute(PrimaryKey = true)]
        public string EquipmentName
        {
            get;
            set;
        }

        [ColummAttribute(PrimaryKey = true)]
        public string PortName
        {
            get;
            set;
        }

        public int BCRExist
        {
            get;
            set;
        }

        public int Capacity
        {
            get;
            set;
        }

        public int CassetteSeqNo
        {
            get;
            set;
        }

        public string CassetteType
        {
            get;
            set;
        }

        public string CurrentStatusTime
        {
            get;
            set;
        }

        public int GradePriority
        {
            get;
            set;
        }

        public int HighLimit
        {
            get;
            set;
        }

        public int LocalNo
        {
            get;
            set;
        }

        public int LowLimit
        {
            get;
            set;
        }

        public int MappingUnitExist
        {
            get;
            set;
        }

        public string PortEnableMode
        {
            get;
            set;
        }

        public string PortGrade
        {
            get;
            set;
        }

        public string PortMode
        {
            get;
            set;
        }

        public int PortNo
        {
            get;
            set;
        }

        public int PortQtime
        {
            get;
            set;
        }

        public string PortStatus
        {
            get;
            set;
        }

        public string PortTransferMode
        {
            get;
            set;
        }

        public string PortType
        {
            get;
            set;
        }

        public string PortTypeAutoMode
        {
            get;
            set;
        }

        public string PortUseType
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

    }
}
