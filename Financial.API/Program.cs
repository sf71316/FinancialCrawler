﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace Financial.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var _container = new UnityContainer();
            return WebHost.CreateDefaultBuilder(args)
                  .UseUnityServiceProvider(_container)
                  .UseStartup<Startup>();
        }
    }
}
