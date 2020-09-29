using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC
{
    
    

  public  class ModReadPanelInfo
    {

      

      //public Dictionary<string, string> CrPanel { get; set; }

      //public Dictionary<string, string> PrePanel { get; set; }

      public string[] CrPanel { get; set; }

      public string[] PrePanel { get; set; }


      public ModReadPanelInfo( )
      {



      }
      

      public int ComparePrevious(string vcrpanelid,string defectcode,string productspec)
      {


          int iRt = 0; //0: same; 1: defcode difference; 2: new panel
          string[] pInfo = new string[CNS.MODPANELINFOCOUNT];
           string panelid ="A"+ productspec.Substring(0, 1) + vcrpanelid;
           pInfo[CNS.GLASSID_IDX] = panelid.Substring(0, panelid.Length - 2);
           pInfo[CNS.PANELID_IDX] = panelid;
           pInfo[CNS.VCRPANELID_IDX] = vcrpanelid;
           pInfo[CNS.DEFECTNO_IDX] = "1";
           pInfo[CNS.DEFECTCODE_IDX] = defectcode;
           pInfo[CNS.TIMEKEY_IDX] = DateTime.Now.ToString("yyyyMMddHHmmss");
           pInfo[CNS.DESCRIPTION_IDX] = "";
           pInfo[CNS.PRODUCTSPEC_IDX] = productspec;
           pInfo[CNS.UPLOADDBFLAG_IDX] = "N";

          if(CrPanel ==null)
          {
              iRt = 2;


              CrPanel = pInfo;



              return iRt;
          }
          if(CrPanel[CNS.VCRPANELID_IDX]==vcrpanelid)
          {
              if(CrPanel[CNS.DEFECTCODE_IDX]==defectcode)
              {
                  iRt = 0;
                  return iRt;
              }else
              {
                  iRt = 1;
              }
             
            
          }else
          {
              iRt=2;
          }

          PrePanel = CrPanel;
          CrPanel = pInfo;

          return iRt;


          //int iRt = 0; //0: same; 1: defcode difference; 2: new panel


          //Dictionary<string, string> temInof = new Dictionary<string, string>();
          //string panelid = productspec.Substring(0,2)+vcrpanelid;
          //temInof.Add("GLASSID", panelid.Substring(0,panelid.Length-2));
          //temInof.Add("PANELID", panelid);
          //temInof.Add("VCRPANELID", vcrpanelid);
          //temInof.Add("PRODUCTSPEC", productspec);
          //temInof.Add("DEFECTCODE",defectcode);

          //if(CrPanel==null)
          //{
          //    CrPanel = temInof;
          //    iRt = 2;
          //    return iRt;

          //}else
          //{
          //    string _prevcrpanelid = "";
          //    if(CrPanel.TryGetValue("GLASSID",out _prevcrpanelid))
          //    {
          //        if(_prevcrpanelid==vcrpanelid)
          //        {
          //            string _defectcode = "";
          //            if(CrPanel.TryGetValue("DEFECTCODE",out _defectcode))
          //            {

          //                if(_defectcode==defectcode)
          //                {
          //                    iRt = 0;
          //                }

          //            }else
          //            {
          //                iRt = 1;
          //                PrePanel = CrPanel;
          //                CrPanel = temInof;
          //                return iRt;
          //            }

          //        }else
          //        {
          //            iRt = 2;
          //            PrePanel = CrPanel;
          //            CrPanel = temInof;
          //            return iRt;
          //        }
          //    }else
          //    {

          //        CrPanel = temInof;
          //        iRt = 2;
          //        return iRt;
          //    }




          //}

          

          //return iRt;
      }

      public string CrPanelToString()
      {
          return AryToString(CrPanel);
      }
      private string AryToString(string[] datas)
      {
          string sRt = "";
          for(int i =0;i<datas.Length;i++)
          {
              sRt+= datas[i];
              if(i!=datas.Length-1)
              {
                  sRt += ",";
              }
          }

          return sRt;
      }



    }
}
