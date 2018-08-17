using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.Extensions.Configuration.Json;
using Financial.Lib.Crawler.Currency;
using Financial.Lib.Crawler;
using Microsoft.Extensions.DependencyInjection;
using Financial.Data.Models;
using Microsoft.EntityFrameworkCore;
using Unity;
using Unity.Injection;
using Financial.Lib.Interfaces;
using Quartz.Impl;
using Financial.Lib.Scheduler;
using Quartz;
using System.Threading.Tasks;

namespace Financial
{
    class Program
    {
        static void Main(string[] args)
        {
            IScheduler scheduler = null;

            #region Register Job
            Task.Factory.StartNew(async () =>
            {
                scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            }).Wait();
            IJobDetail job = JobBuilder.Create<ImmediateExchangeRateJob>()
                .WithIdentity("job1", "group1")
                .Build();
            scheduler.ScheduleJob(job, SchedulerBase.CreateAPI2Trigger());
            scheduler.Start();
            #endregion

            while (true)
            {
                var cmd = Console.ReadLine();
                if (cmd == "exit")
                {
                    scheduler.Shutdown().Wait();
                    break;
                }
            }
        }
    }
}
