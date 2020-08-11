﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrulyVcrLoader
{
    class AnalysizeVcrFile
    {
       
         /// <summary>
            /// 将CSV文件的数据读取到DataTable中
            /// </summary>
            /// <param name="fileName">CSV文件路径</param>
            /// <returns>返回读取了CSV数据的DataTable</returns>
            public static DataTable OpenCSV(string filePath)
            {
               // Encoding encoding = System.Data.Common.GetType(filePath); //Encoding.ASCII;//
                DataTable dt = new DataTable();
                FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);

                //StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                //string fileContent = sr.ReadToEnd();
                //encoding = sr.CurrentEncoding;
                //记录每次读取的一行记录
                string strLine = "";
                //记录每行记录中的各字段内容
                string[] aryLine = null;
                string[] tableHead = null;
                //标示列数
                int columnCount = 0;
                //标示是否是读取的第一行
                bool IsFirst = true;
                //逐行读取CSV中的数据
                while ((strLine = sr.ReadLine()) != null)
                {
                    //strLine = Common.ConvertStringUTF8(strLine, encoding);
                    //strLine = Common.ConvertStringUTF8(strLine);

                    if (IsFirst == true)
                    {
                        tableHead = strLine.Split(',');
                        IsFirst = false;
                        columnCount = tableHead.Length;
                        //创建列
                        for (int i = 0; i < columnCount; i++)
                        {
                            DataColumn dc = new DataColumn(tableHead[i]);
                            dt.Columns.Add(dc);
                        }
                    }
                    else
                    {
                        aryLine = strLine.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = aryLine[j];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (aryLine != null && aryLine.Length > 0)
                {
                    dt.DefaultView.Sort = tableHead[0] + " " + "asc";
                }

                sr.Close();
                fs.Close();
                return dt;
            }
            public static string[] openFile(string filePath)
            {
                string[] lines = File.ReadAllLines(filePath, Encoding.Default);
                
                return lines;
            }
            public static void addLog(string log)
            {
              /*if(!  File.Exists(Application.StartupPath+"//"+DateTime.Now.ToString("yyyyMMHH")+".log")){
                  File.Create(Application.StartupPath+"//"+DateTime.Now.ToString("yyyyMMHH")+".log");

                }*/
                if (!Directory.Exists(Application.StartupPath + "//log//"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "//log//");
                }
                File.AppendAllText(Application.StartupPath + "//log//" + DateTime.Now.ToString("yyyyMMdd") + ".log", "[" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "]" + log+"\n", Encoding.Default);
            }
        }
    
}
