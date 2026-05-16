using KenHRApp.TaskServices.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.TaskServices
{
    internal class Program
    {
        #region Fields

        #region Constants
        public const string RUN_TIMESHEET_PROCESS = "runtimesheet";
        #endregion

        #endregion     

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string command = args[0].ToLower();
                switch (command)
                {
                    case RUN_TIMESHEET_PROCESS:
                        #region Start running the Timesheet Process Service
                        DateTime? attendaceDate = null;

                        // Check if Start Date is specified
                        if (args.Length > 1)
                            attendaceDate = ServiceHelper.ConvertObjectToDateNew(args[1]);

                        if (!attendaceDate.HasValue)
                        {
                            attendaceDate = DateTime.Today;
                        }

                        Console.WriteLine($"Processing the attendance records for the period of {attendaceDate.Value.ToString("dd-MMM-yyyy")} started at {DateTime.Now.ToString("dd MMM yyyy, HH:mm:ss")}.");

                        // Send email message to the administrator
                        //NotifyAdminGeneric(sb.ToString().Trim(), "Procurement Audit Report Generation");

                        Console.WriteLine($"Processing the attendance records for the period of {attendaceDate.Value.ToString("dd-MMM-yyyy")} ended at {DateTime.Now.ToString("dd MMM yyyy, HH:mm:ss")}.");
                        break;
                        #endregion

                    default:
                        Console.WriteLine("Unknown command: " + command);
                        break;
                }   
            }
        }
    }
}
