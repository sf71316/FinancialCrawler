using Financial.Data.Models;
using Financial.Lib.Crawler;
using Financial.Lib.Crawler.Currency;
using Financial.Lib.Interfaces;
using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity;
using Unity.Injection;

namespace Financial.Lib.Scheduler
{
    public abstract class SchedulerBase
    {
        protected IUnityContainer _container;
        public SchedulerBase()
        {
            #region Dependency Injection
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();
            CurrencyData data = new CurrencyData(configuration.GetConnectionString("DefaultConnection"));

            _container = new UnityContainer();
            _container.RegisterType<ICrawler, CurrencyInfoMaker>("maker",
                new InjectionProperty("DbContainer", data)
                );
            _container.RegisterType<ICrawler, CurrencyCrawlerAPI1>("API1",
                new InjectionProperty("DbContainer", data)
                );
            _container.RegisterType<ICrawler, CurrencyCrawlerAPI2>("API2",
                new InjectionProperty("DbContainer", data)
                );
            _container.RegisterType<ICrawler, CurrencyCrawlerAPI2>("API3",
                new InjectionProperty("DbContainer", data)
                );
            #endregion
        }
        public static ITrigger CreateAPI2Trigger()
        {
            return TriggerBuilder.Create()
               .WithIdentity("ExchangeRate", "Currency")
               .WithCronSchedule("0 0/30 * * * ?")
               .StartNow()
               .WithPriority(1)
               .Build();
        }
    }
}
