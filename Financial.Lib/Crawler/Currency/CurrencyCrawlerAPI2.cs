using Financial.Data.Models;
using Financial.Lib.Crawler.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;

namespace Financial.Lib.Crawler.Currency
{
    public class CurrencyCrawlerAPI2 : CurrencylayerBase
    {
        List<CurrencyHistory> histories = new List<CurrencyHistory>();
        CurrencyAPI2Data _CurrencyData;
        public CurrencyCrawlerAPI2() : base("http://www.apilayer.net/api/live?access_key=")
        {
            this._API_Url = string.Concat(this._API_Url, _Key[0]);
        }
        public override void Execute()
        {
            try
            {
                Console.WriteLine("Start crawle ER data....");
                this.OnNotify("Crawling....");
                Crawl();
                this.OnNotify("Crawled....");
                this.OnNotify("Analysis data....");
                Analysis();
                this.OnNotify("Analysis complete....");
                this.OnNotify("Import to db....");
                Import();
                this.OnNotify("Import complete");
            }
            catch (Exception ex)
            {
                this.OnNotify("Error! " + ex.Message);
            }
        }
        protected override void Crawl()
        {
            using (HttpClient client = new HttpClient())
            {
                var _json = client.GetStringAsync(this._API_Url).Result;
                _CurrencyData = CurrencyAPI2Data.FromJson(_json);
            }
        }
        protected override void Analysis()
        {
            if (_CurrencyData != null)
            {
                var map = this.DbContainer.GetCurrencyMap();
                var _sc = (from p in map
                           where p.TagName == _Source_Currency_TagName
                           select p)
                    .FirstOrDefault(c => c.TagName == _Source_Currency_TagName);
                if (_sc != null)
                {
                    System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    DateTime _UpdateDate = dtDateTime.AddSeconds(_CurrencyData.Timestamp).ToLocalTime();
                    var _scER = _CurrencyData.Quotes.FirstOrDefault(p => p.Key == $"USD{_Source_Currency_TagName}");
                    foreach (var item in _CurrencyData.Quotes)
                    {
                        if (item.Key.Length > 3 && item.Key != "USDUSD")
                        {
                            CurrencyHistory ch = new CurrencyHistory();
                            var _tctag = string.Join("", item.Key.Skip(3));
                            var _tc = map.FirstOrDefault(p => p.TagName == _tctag);
                            if (_tc != null)
                            {
                                if (_tc != null && _tc.UID != _sc.UID)
                                {
                                    if (this.DbContainer.IsNewCurrencyExchange(_sc.UID, _tc.UID, _UpdateDate))
                                    {
                                        ch.SourceCurrency = _sc.UID;
                                        ch.TargetCurrency = _tc.UID;
                                        ch.UpdateDate = _UpdateDate;
                                        ch.Uid = Guid.NewGuid();
                                        ch.Value = Convert.ToDecimal(_scER.Value / item.Value);
                                        histories.Add(ch);
                                    }
                                }
                                else
                                {

                                    if (this.DbContainer.IsNewCurrencyExchange(_sc.UID, _tc.UID, _UpdateDate))
                                    {
                                        _tc = map.FirstOrDefault(p => p.TagName == "USD");
                                        ch.SourceCurrency = _sc.UID;
                                        ch.TargetCurrency = _tc.UID;
                                        ch.UpdateDate = _UpdateDate;
                                        ch.Uid = Guid.NewGuid();
                                        ch.Value = Convert.ToDecimal(item.Value);
                                        histories.Add(ch);
                                    }
                                }
                            }
                        }
                    }
                }
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
