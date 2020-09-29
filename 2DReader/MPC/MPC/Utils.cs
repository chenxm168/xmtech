using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FileDataLoader.DBSrv;
using FileDataLoader.FileUpload;
using System.Threading;
using System.Data;


namespace MPC
{
   public class Utils
    {
       static object locker = new object();

       public static bool ProductSpecValidation(string spec)
       {
           bool rtn= false;
           string pattern = @"^A\d{3}\w{1}-\d{3}\w{2}";

           Match match = Regex.Match(spec, pattern);
           rtn = match.Success;

           return rtn;

       }

       public static string getImages(Dictionary<int,string> map)
       {
           if(map.Count>0)
           {
               List<string> ls = new List<string>();
               foreach (KeyValuePair<int, string> en in map)
               {
                   string s = en.Value;
                   string s1 = s.Substring(s.LastIndexOf("\\")+1);
                   ls.Add(s1);
               }
               string s2 = "";
               for(int i=0;i<ls.Count;i++)
               {
                   if(i!=ls.Count-1)
                   {
                       s2 = s2 + ls[i] + ";";
                   }else
                   {
                       s2 = s2 + ls[i];
                   }
               }
               return s2;

           }
           return null;
       }

       public static string getLocalImagePath(Dictionary<int,string> map)
       {
           if (map.Count > 0)
           {
               List<string> ls = new List<string>();
               foreach (KeyValuePair<int, string> en in map)
               {
                   string s = en.Value;

                  string path= s.Substring(0, s.LastIndexOf("\\"));
                  return path;
               }




           }
           return null;
       }


       public static bool UploadDefectData(object db ,  string itemline)
       {
           bool bRtn = true;
           IDBService idb = db as IDBService;
           
           string[] panelItem = itemline.Split(',');
           string _DATE = panelItem[0];
           string _PRODUCTSPEC = panelItem[1];
           string _PANELID = panelItem[2];
           string _VCRID = panelItem[3];
           string _DEFECTCODE = panelItem[4];
           string _DEFECTDESC = panelItem[5];
           string _ISCELL = panelItem[6];
           string _ISLB = panelItem[7];
           string _ISZH = panelItem[8];
           string _LOCALIMAGEPATH = panelItem[9];
           string _IMAGENAMES = panelItem[10];
           string _FTPPATH = panelItem[11];


           int defectNo = 1;
           string[] totalImage = _IMAGENAMES.Split(';');
           foreach (string image in totalImage)
           {
               string sql = string.Format("insert into EDA_PANELJUDGE (TIMEKEY,DEFECTCODE,PRODUCTSPECNAME,GLASSID,PANELID,VCRPANELID,ISCELL,ISLB,ISZH,UPDATEFLAG,FTPPATH,LOCALIMAGEPATH,IMAGENAMES,DEFECTNO) values" +
                   "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                   _DATE, _DEFECTCODE, _PRODUCTSPEC, _PANELID.Substring(0, _PANELID.Length - 2), _PANELID, _VCRID, _ISCELL, _ISLB, _ISZH, "", _FTPPATH, _LOCALIMAGEPATH, image, defectNo);

              // DbService.InertSql(sql);


               if (!idb.ExcNonQuerySql(sql))
             {
                 bRtn = false;
             }
             defectNo = defectNo + 1;

               //if(_LOCALIMAGEPATH.Trim().Length>1&&image.Length>1)
               //{
               //    if (!Fileuploader.UpLoadFile(_LOCALIMAGEPATH + "\\" + image, _FTPPATH, image))
               //    {
               //        bRtn = false;
               //    }
               //}

             UploadFile(_LOCALIMAGEPATH + "\\" + image, _FTPPATH, image);


           }

           return bRtn;

       }


       //TODO
       public static string[] GetProductSpecList(IDBService db)
       {
           string sql ="";
          //  DataTable dt = db.QueryData
           return null;
       }


       public static void UploadFile(string localimage,string ftppath,string image)
       {
           string[] args = { localimage,ftppath,image };
           Thread t = new Thread(FtpUploadFile);
           t.Start(args);
       }


      private static void FtpUploadFile(object arg)
       {
          lock(locker)
          {
              string[] args = (string[])arg;
              string LocalImage = args[0];
              string FtpPath = args[1];
              string image = args[2];
              FtpUploader ftp = new FtpUploader("config/VCRConfig.ini");
              ftp.UpLoadFile(LocalImage, FtpPath, image);
          }

       }


      public static bool InserToDB(Dictionary<string, string> data, string table ,Object db)
      {
          bool bRtn = true;
          IDBService DbService = db as IDBService;
          int idx = 0;
          StringBuilder sbItem = new StringBuilder();
          StringBuilder sbValue = new StringBuilder();
          foreach(KeyValuePair<string,string> p in data)
          {
              if(idx !=data.Keys.Count-1)
              {
                  sbItem.Append(p.Key.ToString().Trim());
                  sbItem.Append(",");
                  sbValue.Append(p.Value.ToString().Trim());
                  sbItem.Append(",");
              }else
              {
                  sbItem.Append(p.Key.ToString().Trim());
                  sbValue.Append(p.Value.ToString().Trim());
              }
              idx++;
          }

          string sql = string.Format("INSERT TO {0} ( {1} ) VALUES ( {2} )  ", table, sbItem.ToString(), sbValue.ToString());
          if (!DbService.ExcNonQuerySql(sql))
          {
              bRtn = false;
          }

          return bRtn;
      }

      public static bool UpdateToDb( Dictionary<string,string> primaryKeys, Dictionary<string, string> data, string table,object db)
      {
          bool bRtn = true;
          IDBService DbService = db as IDBService;
          int idx = 0;
          StringBuilder sbItem = new StringBuilder();
          StringBuilder sbValue = new StringBuilder();
          foreach (KeyValuePair<string, string> p in data)
          {
              if (idx != data.Keys.Count - 1)
              {
                  sbItem.Append(p.Key.ToString().Trim());
                  sbItem.Append(",");
                  sbValue.Append(p.Value.ToString().Trim());
                  sbItem.Append(",");
              }
              else
              {
                  sbItem.Append(p.Key.ToString().Trim());
                  sbValue.Append(p.Value.ToString().Trim());
              }
              idx++;
          }

          //TODO

          return bRtn;
      }



    }
}
