using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF.DB.ObjectService.Type1.Pojo
{
    public  class AlarmHistory
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

    }
}
