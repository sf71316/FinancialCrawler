using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Financial.Data.Models;
using Financial.Lib.Crawler.Model;
using Newtonsoft.Json;
using System.Linq;

namespace Financial.Lib.Crawler.Currency
{
    public class CurrencyCrawlerAPI1 : Crawler.CurrencyCrawlerBase
    {
        string _Source_Currency_TagName = "TWD";
        Dictionary<string, CurrencyAPI1Data> _CurrencyData;
        List<CurrencyHistory> histories = new List<CurrencyHistory>();
        public CurrencyCrawlerAPI1() : base("https://tw.rter.info/capi.php")
        {

        }

        public override void Execute()
        {
            Crawl();
            Analysis();
            Import();
        }

        protected override void Analysis()
        {
            var map = this.DbContainer.GetCurrencyMap();
            var _sc = (from p in map
                       where p.TagName == _Source_Currency_TagName
                       select p)
                .FirstOrDefault(c => c.TagName == _Source_Currency_TagName);
            if (_sc != null)
            {
                var _scER = _CurrencyData.FirstOrDefault(p => p.Key == $"USD{_Source_Currency_TagName}");
                foreach (var item in _CurrencyData)
                {
                    if (item.Key.Length > 3 && item.Key != "USDUSD")
                    {
                        CurrencyHistory ch = new CurrencyHistory();
                        var _tctag = string.Join("", item.Key.Skip(3));
                        var _tc = map.FirstOrDefault(p => p.TagName == _tctag);
                        if (_tc != null && _tc.UID != _sc.UID)
                        {

                            ch.SourceCurrency = _sc.UID;
                            ch.TargetCurrency = _tc.UID;
                            ch.UpdateDate = item.Value.Utc.DateTime;
                            ch.Uid = Guid.NewGuid();
                            ch.Value = Convert.ToDecimal(_scER.Value.Exrate / item.Value.Exrate);
                            histories.Add(ch);
                        }
                        else
                        {
                            if (_tc != null)
                            {
                                _tc = map.FirstOrDefault(p => p.TagName == "USD");
                                ch.SourceCurrency = _sc.UID;
                                ch.TargetCurrency = _tc.UID;
                                ch.UpdateDate = item.Value.Utc.DateTime;
                                ch.Uid = Guid.NewGuid();
                                ch.Value = Convert.ToDecimal(item.Value.Exrate);
                                histories.Add(ch);
                            }
                        }

                    }
                }
            }
        }

        protected override async void Crawl()
        {
            using (HttpClient client = new HttpClient())
            {
                var _json = await client.GetStringAsync(this._API_Url).ConfigureAwait(false);
                _CurrencyData = CurrencyAPI1Data.FromJson(_json);
            }
        }

        protected override void Import()
        {
            using (var _db = this.GetDbContext<FinancialContext>())
            {
                _db.CurrencyHistory.AddRange(histories);
                _db.SaveChanges();
            }
        }
    }
}
