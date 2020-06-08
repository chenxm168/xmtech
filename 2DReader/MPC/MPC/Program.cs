﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPC
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            ////Application.Run(new Form1());
            //Application.Run(new Forms.mainform());

            bool isCanStart;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out   isCanStart);
            if (isCanStart)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);



                Application.Run(new Forms.mainform());

                
            }
            else
            {
                MessageBox.Show("程序已运行!");
            }


        }
    }
}