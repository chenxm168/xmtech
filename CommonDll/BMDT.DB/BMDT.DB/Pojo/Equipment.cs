using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB;

namespace BMDT.DB.Pojo
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

        public bool IsConnected
        {
            get;
            set;
        }
        public int GetStateCode()
        {
            switch (this.EquipmentStatus.ToUpper())
            {
                case "RUN":
                    {
                        return 2;
                        
                    }

                case "IDLE":
                    {
                        return 1;
                        
                    }
                case "TROUBLE" :
                    {
                        return 3;
                    }
                case "MAINTENANCE":
                    {
                        return 4;
                    }
            }
            return 5;
        }



        public void SetState(int iState)
        {
            switch(iState)
            {
                case 1:
                    {
                        this.EquipmentStatus = "Idle";
                        break;
                    }
                case 2:
                    {
                        this.EquipmentStatus = "Run";
                        break;
                    }
                case 3:
                    {
                        this.EquipmentStatus = "Trouble";
                        break;
                    }
                case 4:
                    {
                        this.EquipmentStatus = "Maintenance";
                        break;
                    }

            }
        }

        public int GetControlStateCode()
        {
            switch (this.OnlineControlStatus.ToUpper())
            {
                case "OFFLINE":
                    {
                        return 0;
                    }

                case  "ONLINE":
                    {
                        return 1;
                    }

                case "LOCAL":
                    {
                        return 2;
                    }

              
            }
            return 0;
        }

        public void SetControlState(int st)
        {
            switch (st)
            {
                case 0:
                    {
                        this.OnlineControlStatus = "Offline";
                        return;
                    }
                case 1:
                    {
                        this.OnlineControlStatus = "Online";
                        return;
                    }

                case 2:
                    {
                        this.OnlineControlStatus = "Local";
                        return;
                    }
            }
        }

  
    }
}
