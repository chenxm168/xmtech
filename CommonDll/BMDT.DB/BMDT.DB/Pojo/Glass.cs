using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB;
using System.Globalization;

namespace BMDT.DB.Pojo
{
    public class Glass
    {
        [ColummAttribute(PrimaryKey = true)]
        public string PnlId
        {
            get;
            set;
        }

        [ColummAttribute(PrimaryKey = true)]
        public string UnitId
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
        public Glass()
        {

        }

        
        

        public Glass(string unitid,string pnlid,int state,string stage)
        {
            this.UnitId = unitid;
            this.PnlId = pnlid;
            this.StageId = stage;
            this.State = state;
            this.BluId = " ";
            this.PnlJudge = " ";

        }

        public Glass(string unitid, string pnlid, int state, string stage,string bluid,string pnljudge)
        {
            this.UnitId = unitid;
            this.PnlId = pnlid;
            this.StageId = stage;
            this.State = state;
            this.BluId = bluid;
            this.PnlJudge = pnljudge;
            this.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

     
        /*
         * 1:Start;2:End;
         */
        public int State
        {
          get;
          set;

        }

        public void  SetState(int state)
        {
                 if(state==2)
                {
                    this.State = state;
                    
                    if(StartTime!=null&&StartTime.Length>0)
                    {
                        try
                        {

                       
                        DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                        dtFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss.fff";
                        DateTime dt = DateTime.ParseExact(StartTime, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                       var dt2 = DateTime.Now;
                       this.EndTime = dt2.ToString("yyyy-MM-dd HH:mm:ss.fff");
                       TimeSpan ts = dt2 - dt;
                       CostTime =Math.Round( ts.TotalSeconds);
                             }catch(Exception e)
                        {

                        }
                    }

                }else
                {
                    this.State = state;
                    this.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
            }
        

    }
}
