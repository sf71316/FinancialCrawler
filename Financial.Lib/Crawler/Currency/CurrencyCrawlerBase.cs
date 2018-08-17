using Financial.Lib.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financial.Lib.Crawler
{
    public abstract class CurrencyCrawlerBase : CrawlerBase
    {
        protected string _API_Url;
        public string CrawlerName { get; set; }
        public CurrencyCrawlerBase(string apiUrl)
        {
            this.CrawlerName = "Default Crawler";
            this._API_Url = apiUrl;
        }
       

        protected abstract void Crawl();
        protected abstract void Analysis();
        protected abstract void Import();


    }
}
