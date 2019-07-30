using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB;

namespace BMDT.DB.Pojo
{
    public class AlarmSpec
    {
        [ColummAttribute(PrimaryKey = true)]
        public string UnitId
        {
            get;
            set;
        }

        [ColummAttribute(PrimaryKey = true)]
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


        public int Alcd
        {
            get;
            set;
        }

        public AlarmSpec()
        {

        }

        public AlarmSpec(string unitid,string alid,int alcd,string altx)
        {
            this.Alcd = alcd;
            this.Alid = alid;
            this.Altx = altx;
            this.UnitId = unitid;
        }
    }
}
