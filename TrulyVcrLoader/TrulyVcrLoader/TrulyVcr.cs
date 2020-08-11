using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrulyVcrLoader
{
   public class TrulyVcr
    {
        FtpUtils ftpUtils = new FtpUtils();
        OracleUtils oracleUtils = new OracleUtils();
        public TrulyVcr(string LocalRawDataPath)
        {
            this.initial(LocalRawDataPath);
        }
        public bool initial(string LocalRawDataPath)
        {
            try
            {

                this.WatcherStrat(LocalRawDataPath, "*.csv");
                oracleUtils.openEnvironment();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
           
        }
        #region  监控路径下的文件和特定的文件格式是否有变化
        /// <summary>
        /// 监控路径下的文件和特定的文件格式是否有变化
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filter"></param>
        private void WatcherStrat(string path, string filter)
        {

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.Filter = filter;
            watcher.Changed += new FileSystemEventHandler(OnProcess);
            watcher.Created += new FileSystemEventHandler(OnProcess);
            watcher.Deleted += new FileSystemEventHandler(OnProcess);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
            watcher.IncludeSubdirectories = true;
        }
        #endregion


        #region  判断文件变化的类型：Created;Changed;Deleted
        /// <summary>
        /// 判断文件变化的类型：Created;Changed;Deleted
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnProcess(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                OnCreated(source, e);
                

            }
            else if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                 OnChanged(source, e);
               
            }
            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                OnDeleted(source, e);
            }

        }
        #endregion
        #region  监听新建文件
        /// <summary>
        /// 监听新建文件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnCreated(object source, FileSystemEventArgs e)//新建
        {
            try
            {
                 // DataTable dt = AnalysizeVcrFile.OpenCSV(e.FullPath);
            AnalysizeVcrFile.addLog("Created File:" + e.FullPath);
            string[] allLines = AnalysizeVcrFile.openFile(e.FullPath);
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
                string[] totalImage=_IMAGENAMES.Split(';');
                foreach (string image in totalImage)
                {
                    string sql = string.Format("insert into EDA_PANELJUDGE (TIMEKEY,DEFECTCODE,PRODUCTSPECNAME,GLASSID,PANELID,VCRPANELID,ISCELL,ISLB,ISZH,UPDATEFLAG,FTPPATH,LOCALIMAGEPATH,IMAGENAMES,DEFECTNO) values"+
                        "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                        _DATE, _DEFECTCODE, _PRODUCTSPEC, _PANELID.Substring(0, _PANELID.Length - 2), _PANELID, _VCRID, _ISCELL, _ISLB, _ISZH, "",_FTPPATH,_LOCALIMAGEPATH,image, defectNo);
                    oracleUtils.inertSql(sql);
                    defectNo = defectNo + 1;
                    ftpUtils.UpLoadFile(_LOCALIMAGEPATH + "//" + image, _FTPPATH,image);
                }

            }
            if (!Directory.Exists(@"C:/backup/"))
            {
                Directory.CreateDirectory(@"C:/backup/");
            }
            if (File.Exists(@"C:/backup/" + e.Name))
            {
                Random rd=new Random();
                File.Move(e.FullPath, @"C:/backup/" + e.Name + rd.Next());
                AnalysizeVcrFile.addLog("File Move:" + e.FullPath + "-->" + (@"C:/backup/" + e.Name + rd.Next()));
            }
            else
            {
                File.Move(e.FullPath, @"C:/backup/" + e.Name);
                AnalysizeVcrFile.addLog("File Move:" + e.FullPath + "-->" + (@"C:/backup/" + e.Name));
            }

            }
            catch (Exception ex)
            {
                AnalysizeVcrFile.addLog("OnCreated Exception:" + ex.Message);
            }
          
            
          
        }
        #endregion
        private string glassId;

        #region  监听文件是否发生变化
        /// <summary>
        /// 监听文件是否发生变化
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnChanged(object source, FileSystemEventArgs e)//变化
        {
           

        }
        #endregion
        #region  监听文件是否被删除
        /// <summary>
        /// 监听文件是否被删除
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnDeleted(object source, FileSystemEventArgs e)//删除
        {
   

            //Console.WriteLine("文件删除事件处理逻辑{0}  {1}   {2}", e.ChangeType, e.FullPath, e.Name);
        }

        #endregion

        #region  监听文件是否重命名
        /// <summary>
        /// 监听文件是否重命名
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnRenamed(object source, RenamedEventArgs e)//重命名
        {
         
            // Console.WriteLine("文件重命名事件处理逻辑{0}  {1}  {2}", e.ChangeType, e.FullPath, e.Name);
        }
        #endregion
    }
}
