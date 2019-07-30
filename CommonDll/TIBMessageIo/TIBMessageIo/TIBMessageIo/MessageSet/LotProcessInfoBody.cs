using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
    public class LotProcessInfoBody
    {
       [XmlElement]
       public string MACHINENAME;

       [XmlElement]
       public string ABORTFLAG;

       [XmlElement]
       public string LOTNAME;

       [XmlElement]
       public string CARRIERNAME;

        [XmlElement]
       public string CARRIERTYPE;
      

       [XmlElement]
       public string PORTNAME;


       [XmlElement]
       public string PRODUCTSPECNAME;

       [XmlElement]
       public string PRODUCTSPECVERSION;

       [XmlElement]
       public string PROCESSFLOWNAME;

       [XmlElement]
       public string PROCESSOPERATIONNAME;

       [XmlElement]
       public string WORKORDERNAME;

       [XmlElement]
       public string BAREBOXNAME;

       [XmlElement]
       public string PRODUCTIONTYPE;

       [XmlElement]
       public string PRODUCTQUANTITY;

       [XmlElement]
       public string BAREBOXQUANTITY;

       [XmlElement]
       public string PRODUCTPLANQUANTITY;

       [XmlElement]
       public string SLOTMAP;

       [XmlElement]
       public string SELECTEDSLOTMAP;

       [XmlElement]
       public string EXECUTEDSLOTMAP;

       [XmlElement]
       public string MACHINERECIPENAME;

       [XmlElement]
       public string HOSTMACHINERECIPENAME;

       [XmlElement]
       public string LOTJUDGE;

       [XmlElement]
       public string PRODUCTMATERIALSPEC;

       [XmlElement]
       public string RECIPEPARAVALIDATEFLAG;

       [XmlElement]
       public string MASKDATAVALIDATIONFLAG;

       [XmlElement]
       public string PAIRWORKORDERNAME;

       [XmlElement]
       public string PRODUCTTHICKNESS;

       [XmlElement]
       public string PRODUCTSIZE;

       [XmlElement]
       public string PRODUCTMAKER;

       [XmlElement]
       public string NOMOREINPUT;

       [XmlElement]
       public string LOTKIND;

       [XmlElement]
       public string REASONCODE;

       [XmlElement]
       public string REASONTEXT;


       [XmlElement]
       public GlassInfoList PRODUCTLIST;


    }

    public class GlassInfoList
    {
        [XmlElement]
        public GlassInfo[] PRODUCT;
    }


    public class GlassInfo
    {

        [XmlElement]
        public string LOTNAME;

        [XmlElement]
        public string PRODUCTNAME;

        [XmlElement]
        public string HOSTPRODUCTNAME;

        [XmlElement]
        public string PRODUCTTYPE;

        [XmlElement]
        public string PROCESSOPERATIONNAME;

        [XmlElement]
        public string POSITION;

        [XmlElement]
        public string PRODUCTSPECNAME;

        [XmlElement]
        public string PRODUCTSPECVERSION;

        [XmlElement]
        public string PRODUCTIONTYPE;

        [XmlElement]
        public string BAREBOXNAME;

        [XmlElement]
        public string PRODUCTJUDGE;

        [XmlElement]
        public string SUBPRODUCTGRADES;

        [XmlElement]
        public string WORKORDER;

        [XmlElement]
        public string MACHINERECIPENAME;

        [XmlElement]
        public string HOSTMACHINERECIPENAME;

        [XmlElement]
        public string REWORKCOUNT;

        [XmlElement]
        public string DUMMYUSEDCOUNT;

        [XmlElement]
        public string PROBERNAME;

        [XmlElement]
        public string MASKNAME;

        [XmlElement]
        public string REJUDGEFLAGRESULT;

        [XmlElement]
        public string TURNFLAGRESULT;

        [XmlElement]
        public string PROCESSINGRESULT;

        [XmlElement]
        public string PROCESSINGINFO;

        [XmlElement]
        public string PRODUCTGROUPNAME;

        [XmlElement]
        public string MACHINENAME;

        [XmlElement]
        public string UNITNAME;

        [XmlElement]
        public string SUBUNITNAME;

        [XmlElement]
        public string PORTNAME;

        [XmlElement]
        public string CARRIERNAME;

        [XmlElement]
        public string TRACELEVEL;

        [XmlElement]
        public string MACHINECONTROLSTATENAME;

        [XmlElement]
        public string MACHINESTATENAME;

        [XmlElement]
        public string PRODUCTGRADE;

        [XmlElement]
        public string SUBPRODUCTGRADE;

        //[XmlElement]
        //public string PROCESSINGRESULT;

        [XmlElement]
        public string REASONCODE;

        [XmlElement]
        public ProcessedUnitList PROCESSEDUNITLIST;



    }


    public class ProcessedUnitList
    {
        [XmlElement]
        public ProcessedUnit[] PROCESSEDUNIT;
    }

    public class ProcessedUnit
    {
        [XmlElement]
        public string PROCESSEDUNITNAME;
        [XmlElement]
        public ProcessedSubUnitList PROCESSEDSUBUNITLIST;
    }

    public class ProcessedSubUnit
    {
        [XmlElement]
        public string PROCESSEDSUBUNITNAME;
    }

    public class ProcessedSubUnitList
    {
        [XmlElement]
        public ProcessedSubUnit[] PROCESSEDSUBUNIT;
    }
}
