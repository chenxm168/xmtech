using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF.DB.ObjectService.Type1.Pojo
{
    public class Equipment
    {
        [ColummAttribute(PrimaryKey = true)]
        public string EquipmentName
        {
            get;
            set;
        }

        public string RmsUseFlag
        {
            get;
            set;
        }

        public string CurrentStatusTime
        {
            get;
            set;
        }

        public string EquipmentStatus
        {
            get;
            set;
        }

        public string EquipmentType
        {
            get;
            set;
        }

        public string OldEquipmentStatus
        {
            get;
            set;
        }

        public string OnlineControlStatus
        {

            get;
            set;
        }

        public string Shop
        {
            get;
            set;
        }

        public string ProjectType
        {
            get;
            set;
        }

        public string StatusCode
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
