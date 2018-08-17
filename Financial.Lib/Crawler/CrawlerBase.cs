using Financial.Data.interfaces;
using Financial.Lib.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Financial.Lib.Crawler
{
    public abstract class CrawlerBase : ICrawler
    {
        public CrawlerBase()
        {

        }
        public T GetDbContext<T>() where T : DbContext
        {
            return (T)this.DbContainer.DbContext;
        }
        protected ICurrencyData DbContainer { get; set; }

        public abstract void Execute();
        protected void OnNotify(string message)
        {
            if (this.Notify != null)
            {
                this.Notify(this, new NotifyArg { Message = message });
            }
        }
        public event EventHandler<NotifyArg> Notify;
    }
}
