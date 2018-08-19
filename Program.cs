using System;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Owin.Hosting;

namespace OwinSelfHosted
{
    static class Program
    {
        private static readonly log4net.ILog log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:5000/";

            using (WebApp.Start<Startup>(url: baseAddress))
            {
                log.Info("Web Server is running.");
                log.Info("Press any key to quit");
                Console.ReadLine();
            }
        }
    }
}
