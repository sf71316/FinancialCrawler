using System;
using System.Collections.Generic;
using System.Text;

namespace Financial.Lib.Crawler.Currency
{
    public abstract class CurrencylayerBase : CurrencyCrawlerBase
    {
        protected string _Source_Currency_TagName = "TWD";
        protected string[] _Key = new string[] { "469e874a16fd239622313b0d61bcbfa6", "53857ad68189a68b2b5ea4fea2b981c9" };
        public CurrencylayerBase(string API_Url) : base(API_Url)
        {

        }
    }
}
