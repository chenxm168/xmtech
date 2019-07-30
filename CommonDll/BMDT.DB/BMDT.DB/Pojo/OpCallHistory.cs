using HF.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMDT.DB.Pojo
{
   public  class OpCallHistory
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

        public string OpCallContent
        {
            get;
            set;
        }

        public string Owner
        {
            get;
            set;
        }


    }
}
