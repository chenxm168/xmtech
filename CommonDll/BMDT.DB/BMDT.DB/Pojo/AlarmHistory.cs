using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB;

namespace BMDT.DB.Pojo
{
    public class AlarmHistory
    {
        [ColummAttribute(PrimaryKey = true)]
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

        public int Alst
        {
            get;
            set;
        }

        public int Alcd
        {
            get;
            set;
        }

        public string Alid
        {
            get;
            set;
        }

        public string Altx
        {
            get;
            set;
        }

        public string UnitId
        {
            get;
            set;
        }


        public AlarmHistory()
        {

        }

        public AlarmHistory(AlarmSpec al,int alst)
        {
            this.ObjectNo = "S_" + DateTime.Now.ToString("yyyyMMddHHmmssfff"); 
            this.HistoryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            this.EventName = alst==1? "Set":"Clear";

            this.UnitId = al.UnitId;
            this.Alid = al.Alid;
            this.Altx = al.Altx;
            this.Alcd = al.Alcd;
            this.Alst = alst;

        }
    }

}
