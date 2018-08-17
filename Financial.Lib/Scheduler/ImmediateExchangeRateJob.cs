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
    public class ImmediateExchangeRateJob : SchedulerBase, IJob
    {
        public ImmediateExchangeRateJob() : base()
        {

        }



        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("ER Job start....");
            var _api = _container.Resolve<ICrawler>("API2");
            _api.Notify += _api_Notify;
            return Task.Factory.StartNew(() =>
            {
                _api.Execute();
                Console.WriteLine("ER Job end....");
            });
        }

        private void _api_Notify(object sender, NotifyArg e)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.hhh")}] {e.Message}");
        }
    }
}
