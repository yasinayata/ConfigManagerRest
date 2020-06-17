using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConfigManagerRest.General;
using ConfigManagerRest.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConfigManagerRest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Start ConfigManager constructors
            #region ConfigManager construction
            Common.isServiceActive = true;

            ConfigProcess.Start();

            Common.CreateProvider();
            #endregion

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
