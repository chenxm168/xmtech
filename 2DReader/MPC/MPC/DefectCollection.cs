using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using FileSrv.csv;

namespace MPC
{
   public class DefectCollection:IDisposable
    {

       public List<Defect> Defects
       {
           get;
           set;
       }

       protected string csvfile;

       protected string header;

       public DefectCollection(string csvfile)
       {
           Defects = new List<Defect>();
           if(File.Exists(csvfile))
           {
               this.csvfile = csvfile;
               CSVReader csv = new CSVReader();
               string[] items;
               string head;
               if(csv.ReadCSV(csvfile,out items,out head))
               {
                   for(int i=0;i<items.Length;i++)
                   {
                       //byte[] bItem = Encoding.Default.GetBytes(items[i]);
                       //string sItem = Encoding.UTF8.GetString(bItem);
                       Defect df = new Defect();
                       df.SetValue(items[i]);
                       Defects.Add(df);
                      // Defects.Add(new Defect(items[i]));
                   }

                   header = head;
               }
           }
       }

       ~DefectCollection()
       {
           this.Dispose();
       }


       public string[] getCodeAndNameString()
       {
           var list = (from s in Defects orderby s.priority descending select s).ToList<Defect>();

           List<string> rl = new List<string>();
           for (int i = 0; i < list.Count;i++ )
           {
               rl.Add(list[i].DefectCode + "," + list[i].DefectName);
           }
               return rl.ToArray<string>();
       }

       public void SetPriority(string codeAndName)
       {
           try
           {
               string[] ars = codeAndName.Split(new char[] { ',' });
               string ar = ars[0];
               for (int i = 0; i < Defects.Count;i++ )
               {
                   if(Defects[i].DefectCode.Trim().Equals(ar.Trim().ToUpper()))
                   {
                       Defects[i].priority++;
                       break;
                   }
               }

                   //if (ars.Length > 0)
                   //{
                   //    var dfs = (from d in Defects where d.DefectCode.Trim().Equals(ar.Trim()) select d).ToArray<Defect>();
                   //    foreach (Defect d in dfs)
                   //    {
                   //        d.priority++;
                   //    }
                   //}
           }catch(Exception e)
           {

           }
       }


       public void Dispose()
       {
          // throw new NotImplementedException();
       }
    }

    public class Defect
    {
        public int DefectNo
        {
            get;
            set;
        }

        public string DefectCode
        {
            get;
            set;
        }

        public string DefectName
        {
            get;
            set;
        }

        public string Desc
        {
            get;
            set;
        }

        public int priority
        {
            get;
            set;
        }
        public string Category
        {
            get;
            set;
        }


        public Defect()
        {
            DefectNo = 0;
            DefectCode = "";
            DefectName = "";
            Desc = "";
            Category = "";
            priority = 0;
        }

        public void SetValue(string df)
        {
            var df1 = df.Replace('"', ' ');
            var df2 = df1.Split(new char[] { ',' });

            try
            {
                string dd = df2[0].Trim();
                 DefectNo = Convert.ToInt32(df2[0].ToString().Trim());
               // DefectNo = 0;
               // DefectNo = Convert.ToInt16("1");
                DefectCode = df2[1].Trim();
                DefectName = df2[2].Trim();
                Desc = df2[3].Trim();
                Category = df2[4].Trim();
                priority = Convert.ToInt32(df2[5].Trim());

            }
            catch (Exception e)
            {

            }
        }

        public Defect(string df)
        {
          //  Defect defect = new Defect();
           var df1 = df.Replace('"',' ');
           var df2= df1.Split(new char[] { '\t' });

        try
        {
            string dd = df2[0].Trim();
          //  this.Number = Convert.ToInt32(df2[0].ToString().Trim());
            DefectNo = 0;
            DefectNo = Convert.ToInt16("1");
            DefectCode = df2[1].Trim();
            DefectName = df2[2].Trim();
            Desc = df2[3].Trim();
            Category = df2[4].Trim();
            priority = Convert.ToInt32(df2[5].Trim());

        }catch(Exception e)
        {

        }
         //  Defect defect = new Defect();

        }

        public override string ToString()
        {
            return this.DefectNo.ToString() + "," + this.DefectCode + "," + this.DefectName + "," + this.Desc + "," + this.Category+","+  this.priority.ToString();
        }


    }
}
