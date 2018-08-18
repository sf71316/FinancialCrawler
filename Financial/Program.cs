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
        static bool _ShowMenu = true;
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
            IJobDetail job2 = JobBuilder.Create<HistoricalExchangeRateJob>()
              .WithIdentity("job2", "group2")
              .Build();
            //scheduler.ScheduleJob(job, SchedulerBase.CreateAPI2Trigger());
            //scheduler.ScheduleJob(job2, SchedulerBase.CreateAPI3Trigger());
            //scheduler.Start();
            #endregion
            var _Isrun = true;

            while (_Isrun)
            {
                var cmd = SelectMenu();
                switch (cmd)
                {
                    case 1:
                        scheduler.ScheduleJob(job, SchedulerBase.CreateAPI2Trigger());
                        scheduler.Start();
                        Console.Clear();
                        Console.WriteLine("Immediate exchange rate will start....");
                        _ShowMenu = false;
                        break;
                    case 2:
                        scheduler.ScheduleJob(job, SchedulerBase.CreateAPI3Trigger());
                        scheduler.Start();
                        Console.Clear();
                        Console.WriteLine("Historical exchange rate will start....");
                        _ShowMenu = false;
                        break;
                    case 3:
                        scheduler.Shutdown().Wait();
                        Console.WriteLine("Task is shutdown ");
                        Console.WriteLine("please press any key to continue....");
                        Console.ReadLine();
                        _ShowMenu = true;
                        break;
                    case 4:
                        _ShowMenu = true;
                        break;
                    case 5:
                        _Isrun = false;
                        break;
                    default:
                        break;
                }
            }
        }
        static int SelectMenu()
        {
            if (_ShowMenu)
            {
                Console.Clear();
                Console.WriteLine($"1. Start immediate exchange rate process");
                Console.WriteLine($"2. Start historical exchange rate process");
                Console.WriteLine($"3. Stop current task");
                Console.WriteLine($"4. Show menu");
                Console.WriteLine($"5. Quit");
            }
            int _cmd = 0;
            if (int.TryParse(Console.ReadLine(), out _cmd))
            {
                return _cmd;
            }
            else
            {
                Console.WriteLine("Error command please try again.... ");
                return SelectMenu();
            }
        }
    }
}
