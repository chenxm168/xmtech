using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB.Service;
using System.Collections;
using BMDT.DB.Pojo;

namespace BMDT.DB.Service
{
    public class GlassServiceImpl : AbsService, IGlassService
    {

        public int GlassStart(string unitId, string stage, string pnlId, string bluId, string pnlJudge)
        {
            Dictionary<string,object> keys = new Dictionary<string,object>();
            keys.Add("PnlId",pnlId);
            keys.Add("UNITID", unitId);
            Glass tgs = FindbyKey<Glass>(keys, null, false);
            int irt =-1;
            if(tgs!=null)
            {
                if(tgs.State==2)
                {
                    tgs.SetState(1);
                   // tgs.StarTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                    tgs.UnitId = unitId;
                    tgs.StageId = stage;
                  irt=  UpdateTable(tgs);
                }
            }else
            {
                tgs = new Glass(unitId, pnlId, 1, stage, string.Empty, string.Empty);
                //tgs.StarTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                tgs.SetState(1);
               irt  = InsertToTable(tgs);
            }
             

            GlassHistory his = new GlassHistory(tgs);
            InsertToTable(his);
            return irt;

        }

        public int GlassEnd(string unitId, string stage, string pnlId, string bluId, string pnlJudge)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add("PnlId", pnlId);
            keys.Add("UNITID", unitId);

            Glass tgs = FindbyKey<Glass>(keys, null, false);
            int irt = -1;
            if (tgs != null)
            {
                
                    
                    //tgs.StarTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                    tgs.UnitId = unitId;
                    tgs.SetState(2);
                    tgs.StageId = stage;
                    tgs.PnlJudge = pnlJudge;
                    tgs.BluId = bluId;
                  irt=  UpdateTable(tgs);
                
            }else
            {
                tgs = new Glass(unitId, pnlId, 2, stage, bluId, pnlJudge);
                //tgs.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                tgs.SetState(2);
                 irt = InsertToTable(tgs);
            }
            

            GlassHistory his = new GlassHistory(tgs);
           // GlassHistory his = new GlassHistory(new Glass(unitId, pnlId, 2, stage, bluId, pnlJudge));
            InsertToTable(his);
            return irt;
        }

        public System.Data.DataTable FindHistoryByTimeRtnDt(string unitId, string frTime, string toTime)
        {
            string sql = "SELECT * FROM GLASSHISTORY WHERE UNITID='{0}' AND ( HISTORYTIME BETWEEN '{1}' AND '{2}' )";

            return ExtQueryBySqlRtnDt<GlassHistory>(sql, new object[] { unitId, frTime, toTime });
        }

        public GlassHistory[] FindHistoryByTime(string unitId, string frTime, string toTime)
        {
            string sql = "SELECT * FROM GLASSHISTORY WHERE UNITID='{0}' AND ( HISTORYTIME BETWEEN '{1}' AND '{2}' )";

            return ExtQueryBySql<GlassHistory>(sql, new object[] { unitId, frTime, toTime });
        }

        public int HistoryDailyClean(int remainDay)
        {
            int rtn= HistoryDailyClean<GlassHistory>(remainDay);
            DateTime dt = DateTime.Now.AddDays((-1)*remainDay);
            string sDt = dt.ToString("yyyy-MM-dd HH:mm:ss:fff");
            string sql = string.Format ("DELETE FROM GLASS WHERE STATE=2 AND ENDTIME<'{0}'",sDt);
            return   ExtNoQueryBysql(sql);
        }
    }
}
