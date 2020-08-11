using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MPC
{
   public class ProductSpecColection
    {
       public string[] SpecList
       {
           get;
           set;
       }

       private string file;
       public ProductSpecColection(string file)
       {
           this.file = file;
           FileStream fs = null;
           StreamReader sr = null;
           List<string> list = new List<string>();
           try
           {
               if (File.Exists(file))
               {

                   fs = new FileStream(file, FileMode.Open);
                   sr = new StreamReader(fs, Encoding.Default);
                   for (; ; )
                   {


                       if (sr.Peek() > 0)
                       {
                           list.Add(sr.ReadLine().Trim().ToUpper());

                       }
                       else
                       {
                           break;
                       }
                   }




               }
               SpecList = list.ToArray<string>();

           }
           catch (Exception e)
           {

           }

           finally
           {
               if (sr != null)
               {
                   sr.Close();
               }
               if (fs != null)
               {
                   fs.Close();
               }

           }
       }

       ~ProductSpecColection()
       {
           string s = "";
           for(int i=0;i<SpecList.Length;i++)
           {
               if(i!=SpecList.Length-1)
               {
                   s = s + SpecList[i] + "\n";
               }else
               {
                   s = s + SpecList[i];
               }
           }


           FileStream fs = null;
           StreamWriter sw= null;

           try
           {
               fs = new FileStream(file, FileMode.Create);
               sw = new StreamWriter(fs, Encoding.Default);
               sw.Write(s);
               sw.Flush();


           }catch(Exception e)
           {

           }
           finally
           {
               if (sw != null)
               {
                   sw.Close();
               }
               if (fs != null)
               {
                   fs.Close();
               }
           }




       }

       public void addSpec(string p)
       {
           try
           {
              
               if(SpecList.Contains(p))
               {
                   return;
               }else
               {
                   List<string> ls = new List<string>();
                   ls.AddRange(SpecList);
                   string[] s = ls.ToArray<string>();
                   SpecList = s;
               }

           }catch(Exception e)
           {

           }
       }

    }
}
