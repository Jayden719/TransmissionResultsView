﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransmissionResultsView
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //if (IsAdministrator() == false)
            //{
            //    try
            //    {
            //        ProcessStartInfo info = new ProcessStartInfo()
            //        {
            //            UseShellExecute = true,
            //            FileName = Application.ExecutablePath,
            //            WorkingDirectory = Environment.CurrentDirectory,
            //            Verb = "runas"
            //        };

            //        Process.Start(info);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //        return;
            //    }
            //}
            //else
            //{
                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new Form1());
            //}
            
        }
        
        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            if (identity != null)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            return false;
        }
    }
}
