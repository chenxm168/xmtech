using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MPC
{
   public class GlassDefectColection
    {
       public string[] DefectGlasses
       {
           get;
           set;
       }
       public GlassDefectColection(string file)
       {
           FileStream fs =null;
           StreamReader sr = null;
           List<string> list = new List<string>();
           try
           {
               if (File.Exists(file))
               {

                   fs = new FileStream(file, FileMode.Open);
                   sr = new StreamReader(fs, Encoding.Default);
                   for(;;)
                   {


                      if(sr.Peek()>0)
                      {
                          list.Add(sr.ReadLine().Trim().ToUpper());

                      }else
                      {
                          break;
                      }
                   }




               }
               DefectGlasses= list.ToArray<string>();

           }catch (Exception e)
           {

           }

           finally
           {
               if(sr!=null)
               {
                   sr.Close();
               }
               if(fs!=null)
               {
                  fs.Close();
               }
               
           }


       }

       public string[] GetDefectGlasses()
       {
           return DefectGlasses;
       }


    }
}
