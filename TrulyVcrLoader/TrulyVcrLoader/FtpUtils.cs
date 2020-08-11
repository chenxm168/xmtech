using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TrulyVcrLoader
{
    
    class FtpUtils
    {
        private string ftpUserName = "hello";
        private string ftpUserPassword = "1";
        private string ftpRootPath = "ftp://172.28.48.113/DUMMY/";
        FtpWebRequest reqFTP;
        FtpWebResponse response;
        Stream strm;
         FileStream fs;
         private string _path = "";
         public void UpLoadFile(string localFile, string ftpPath,string fileName)
        {
             _path=ftpRootPath;
          
                if (!File.Exists(localFile))
                {
                    AnalysizeVcrFile.addLog("文件：“" + localFile + "” 不存在！");
                    return;
                }
              //  loginfo.Info("Upload Local File:" + localFile + "--->Ftp Path:" + csvftpPath);
                FileInfo fileInf = new FileInfo(localFile);


                this.response = null;
                this.reqFTP = null;
                
              try
            {
                string[] path =Regex.Split(ftpPath,@"\\",RegexOptions.IgnoreCase);
                foreach (string dir in path)
                {
                    _path+=dir+"/";
                    MakeDir(_path);
                }
                this.reqFTP = (FtpWebRequest)FtpWebRequest.Create(_path+fileName);// 根据uri创建FtpWebRequest对象 
                
                  
                this.reqFTP.Credentials = new NetworkCredential(ftpUserName, ftpUserPassword);// ftp用户名和密码
                this.reqFTP.KeepAlive = false;// 默认为true，连接不会被关闭 // 在一个命令之后被执行
                this.reqFTP.Method = WebRequestMethods.Ftp.UploadFile;// 指定执行什么命令
                this.reqFTP.UseBinary = true;// 指定数据传输类型
                this.reqFTP.UsePassive = true;
                this.reqFTP.ContentLength = fileInf.Length;// 上传文件时通知服务器文件的大小
                this.response = (FtpWebResponse)this.reqFTP.GetResponse();
                int buffLength = 20480000;// 缓冲大小设置为2kb
                byte[] buff = new byte[buffLength];
                int contentLen;
                
                // 打开一个文件流 (System.IO.FileStream) 去读上传的文件
                 this.fs = fileInf.OpenRead();

                 this.strm = this.reqFTP.GetRequestStream();// 把上传的文件写入流
                contentLen = fs.Read(buff, 0, buffLength);// 每次读文件流的2kb
                while (contentLen != 0)// 流内容没有结束
                {
                    // 把内容从file stream 写入 upload stream
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                AnalysizeVcrFile.addLog("Upload Local File:" + localFile + "--->Ftp Path:" + _path);
                // MessageBox.Show("文件【" + ftpPath + "/" + fileInf.Name + "】上传成功！<br/>");
            }
            catch (Exception ex)
            {

                 if (this.response != null)
                {
                    this.response.Close();
                    
                }
                 if (this.strm != null) 
                this.strm.Close();
                if(this.fs!=null)
                this.fs.Close();

                this.response = null;
                this.strm = null;
                this.fs = null;
                AnalysizeVcrFile.addLog("UpLoadFile Exception:" + ex.Message);
              //  loginfo.Info("UpLoadFile Exception:" + ex.Message);
                UpLoadFile(localFile,_path,fileName);
                
              

            }
            finally
            {
                if (this.response != null)
                {
                    this.response.Close();
                    
                }
                // 关闭两个流
                if (this.strm != null)
                    this.strm.Close();
                if (this.fs != null)
                    this.fs.Close();

                this.response = null;
                this.strm = null;
                this.fs = null;
                AnalysizeVcrFile.addLog("FTP Response Kill");
             //   loginfo.Info("FTP Response Kill" );
            }
        }

      
         /// <summary>
        /// 在FTP上创建文件夹
        /// </summary>
        /// <param name="makeftpPath"></param>
        public void MakeDir(string makeftpPath)
        {

           
            try
            {
                if (CheckDirectoryExist(makeftpPath))
                {
                   // loginfo.Info(makeftpPath + "Also Exist!");
                    return;
                }
                string ui = (makeftpPath).Trim();
                this.reqFTP = (FtpWebRequest)FtpWebRequest.Create(ui);                
                this.reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                this.reqFTP.UseBinary = true;
                this.reqFTP.KeepAlive = false;
                this.reqFTP.Credentials = new NetworkCredential(ftpUserName, ftpUserPassword);
                this.response = (FtpWebResponse)this.reqFTP.GetResponse();
                long i = this.response.ContentLength;
                Console.Write(i);
                Stream ftpStream = this.response.GetResponseStream();
                ftpStream.Close();
              
                // MessageBox.Show("文件夹【" + dirName + "】创建成功！<br/>");
            }

            catch (Exception ex)
            {
                if (ex.Message.Contains("501"))
                {
                 //   loginfo.Info("Exception:" + ex.Message + " Retry Make Ftp Path Once Again!");
                    if (this.response != null)
                    {
                        this.response.Close();
                    }
                    this.response = null;
                    this.reqFTP = null;
                    MakeDir(makeftpPath);

                }
             //   loginfo.Info("MakeDir Exception:" + ex.Message);
                //  MessageBox.Show("新建文件夹【" + dirName + "】时，发生错误：" + ex.Message);
            }
            finally
            {
                if (this.response!=null)
                {
                    this.response.Close();
                }
                this.response = null;
                this.reqFTP = null;
               // loginfo.Info("FTP Response Kill" );
            }

        }
       
        /// 判断ftp服务器上该目录是否存在
        /// </summary>
        /// <param name="ftpPath">FTP路径目录</param>
        /// <param name="dirName">目录上的文件夹名称</param>
        /// <returns></returns>
        private bool CheckDirectoryExist(string ftpPath)
        {
            //响应结果
            //StringBuilder result = new StringBuilder();
            List<string> listresult = new List<string>();
            //FTP请求
            FtpWebRequest ftpRequest = null;
            //FTP响应
            WebResponse ftpResponse = null;
            //FTP响应流
            StreamReader ftpResponsStream = null;
         
            try
            {
                //生成FTP请求
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));

                //设置文件传输类型
                ftpRequest.UseBinary = true;
                ftpRequest.KeepAlive = false;
                //FTP登录
                ftpRequest.Credentials = new NetworkCredential(ftpUserName, ftpUserPassword);
                //设置FTP方法
                //ftpRequest.Timeout = 2 * 1000;
                //ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                //ftpRequest.Proxy = null;
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                //生成FTP响应
                ftpResponse = ftpRequest.GetResponse();
                //FTP响应流
                ftpResponsStream = new StreamReader(ftpResponse.GetResponseStream());
                ftpResponsStream.ReadLine();
                /*  while (line != null)
                  {
                      //result.Append(line);
                      //result.Append("\n");

                      if (line[0] == 'd')
                      {
                          string[] str = line.Split(' ');
                          string Dname = str[0][0] + "@" + str[str.Length - 1];
                          listresult.Add(Dname);
                      }
                      else if (line[0] == '-')
                      {
                          string[] str = line.Split(' ');
                          string Dname = str[0][0] + "@" + str[str.Length - 1];
                          listresult.Add(Dname);
                      }
                      line = ftpResponsStream.ReadLine();
                  }*/

                //去掉结果列表中最后一个换行
                //result.Remove(result.ToString().LastIndexOf('\n'), 1);

                //返回结果
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (ftpResponsStream != null)
                {
                    ftpResponsStream.Close();
                }

                if (ftpResponse != null)
                {
                    ftpResponse.Close();
                }
            }
        }
    }

}
