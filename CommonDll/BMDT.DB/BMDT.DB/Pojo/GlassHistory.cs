using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB;

namespace BMDT.DB.Pojo
{
  public  class GlassHistory
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
        public string PnlId
        {
            get;
            set;
        }

        public string StartTime
        {
            get;
            set;
        }

        public string EndTime
        {
            get;
            set;
        }

        public string StageId
        {
            get;
            set;
        }

        public string UnitId
        {
            get;
            set;
        }

        public double CostTime
        {
            get;
            set;
        }

        public string BluId
        {
            get;
            set;
        }

        public string PnlJudge
        {
            get;
            set;
        }

        public int State
        {
            get;
            set;
        }


     public  GlassHistory (Glass glass)
        {
            this.PnlId = glass.PnlId;
            this.StartTime = glass.StartTime;
            this.EndTime = glass.EndTime;
            this.StageId = glass.StageId;
            this.BluId = glass.StageId;
            this.CostTime = glass.CostTime;
            this.UnitId = glass.UnitId;
            this.State = glass.State;
            this.EventName = State == 1 ? "Start" : "End";
            this.ObjectNo = "S_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            this.HistoryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        }

    }
}
