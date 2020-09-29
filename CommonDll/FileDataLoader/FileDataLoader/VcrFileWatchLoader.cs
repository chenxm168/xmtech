using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileDataLoader.DBSrv;
using FileDataLoader.FileUpload;
using System.IO;

namespace FileDataLoader
{
   public class VcrFileWatchLoader:AbsFileWatchLoader
    {

       public IDBService DbService
       { get; set; }

       public IFileUploader Fileuploader
       { get; set; }


       public VcrFileWatchLoader()
       {
           Init();
       }

       public VcrFileWatchLoader(string configfile)
       {
           Init(configfile);
       }

       public void Init(string file)
       {

           var cfg = ConfigLoader.getConfigInstance(file);
           Clog = LogFactory.getLogger(cfg.getParam("Logger"));
           
           string dbs1= cfg.getAsciiParam("ConnString1");
           string dbs2 = cfg.getAsciiParam("ConnString2");

           OracleDBService db = new OracleDBService(dbs1, dbs2);
           DbService = db;

           string ftprootpath = cfg.getParam("FTPRootPath");
           string ftpusr = cfg.getParam("FTPUser");
           string ftppwd = cfg.getParam("FTPPwd");

           FtpUploader uploader = new FtpUploader(ftpusr, ftppwd, ftprootpath);

           Fileuploader = uploader;

           WatcherStrat();
       }

       public void Init()
       {
           Init("config\\VcrConfig.ini");
       }

      


        protected override void OnRenamed(object source, System.IO.RenamedEventArgs e)
        {
            if(Clog==null)
            {
                Clog = LogFactory.getSimpleLogger();
            }
        }

        protected override void OnCreated(object source, System.IO.FileSystemEventArgs e)
        {
            if (Clog == null)
            {
                Clog = LogFactory.getSimpleLogger();
            }

            try
            {
                // DataTable dt = AnalysizeVcrFile.OpenCSV(e.FullPath);
                Clog.info("Created File:" + e.FullPath);
                //string[] allLines = AnalysizeVcrFile.openFile(e.FullPath);
                string[] allLines =File.ReadAllLines(e.FullPath, Encoding.Default);
                string tiitle = allLines[0];
                for (int i = 1; i < allLines.Length; i++)
                {
                    string[] panelItem = allLines[i].Split(',');
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
                        
                        DbService.ExcNonQuerySql(sql);
                        defectNo = defectNo + 1;
                        Fileuploader.UpLoadFile(_LOCALIMAGEPATH + "\\" + image, _FTPPATH, image);
                        
                    }

                }
                if (!Directory.Exists(@"C:/backup/"))
                {
                    Directory.CreateDirectory(@"C:/backup/");
                }
                if (File.Exists(@"C:/backup/" + e.Name))
                {
                    Random rd = new Random();
                    File.Move(e.FullPath, @"C:/backup/" + e.Name + rd.Next());
                   Clog.info("File Move:" + e.FullPath + "-->" + (@"C:/backup/" + e.Name + rd.Next()));
                }
                else
                {
                    File.Move(e.FullPath, @"C:/backup/" + e.Name);
                    Clog.info("File Move:" + e.FullPath + "-->" + (@"C:/backup/" + e.Name));
                }

            }
            catch (Exception ex)
            {
                Clog.error("OnCreated Exception:" + ex.Message);
            }
        }

        protected override void OnChanged(object source, System.IO.FileSystemEventArgs e)
        {
            if (Clog == null)
            {
                Clog = LogFactory.getSimpleLogger();
            }
        }

        protected override void OnDeleted(object source, System.IO.FileSystemEventArgs e)
        {
            if (Clog == null)
            {
                Clog = LogFactory.getSimpleLogger();
            }
        }
    }
}
