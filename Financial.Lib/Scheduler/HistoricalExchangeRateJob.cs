using Financial.Lib.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Financial.Lib.Scheduler
{
    [DisallowConcurrentExecutionAttribute]
    public class HistoricalExchangeRateJob : SchedulerBase, IJob
    {
        public HistoricalExchangeRateJob()
        {

        }
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Historical ER Job start....");
            var _api = _container.Resolve<ICrawler>("API3");
            _api.Notify += _api_Notify;
            return Task.Factory.StartNew(() =>
            {
                _api.Execute();
                Console.WriteLine("Historical ER Job end....");
            });
        }
        private void _api_Notify(object sender, NotifyArg e)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.hhh")}] {e.Message}");
        }
    }
}
