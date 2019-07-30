using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HF.DB
{
    public class ColummAttribute : Attribute
    {
        public Boolean PrimaryKey
        {
            get;
            set;
        }
        public Boolean AutoGrow
        {
            get;
            set;
        }
        public Boolean IsNotNull
        {
            get;
            set;
        }

        public Boolean Timestamp
        {
            get;
            set;
        }


        public ColummAttribute()
        {
            
        }
        public ColummAttribute(bool PrimaryKey, bool AutoGrow, bool IsNotNull)
        {
            this.PrimaryKey = PrimaryKey;
            this.AutoGrow = AutoGrow;
            this.IsNotNull = IsNotNull;
        }

        public ColummAttribute(bool PrimaryKey)
        {
            this.PrimaryKey = PrimaryKey;
        }
    }
}
