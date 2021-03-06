﻿using Financial.Data.Models;
using Financial.Lib.Crawler.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;

namespace Financial.Lib.Crawler.Currency
{
    public class CurrencyCrawlerAPI3 : CurrencylayerBase
    {

        List<CurrencyHistory> _Histories = new List<CurrencyHistory>();
        CurrencyApi3Data _CurrencyData;
        DateTime _StartDate = new DateTime(2017, 1, 1);
        DateTime _EndDate = DateTime.Now.AddDays(-1).Date;
        public CurrencyCrawlerAPI3() : base("http://apilayer.net/api/historical?date={0}&access_key=")
        {
            this._API_Url = this._API_Url + _Key[1];
        }

        public override void Execute()
        {
            var _API = this._API_Url;
            while (_StartDate <= _EndDate)
            {
                this._API_Url = string.Format(_API, _EndDate.ToString("yyyy-MM-dd"));
                Crawl();
                Analysis();
                _EndDate = _EndDate.AddDays(-1);
                Import();
                _Histories.Clear();
            }

        }
        protected override void Analysis()
        {
            var map = this.DbContainer.GetCurrencyMap().ToList();
            var _sc = (from p in map
                       where p.TagName == _Source_Currency_TagName
                       select p)
                .FirstOrDefault(c => c.TagName == _Source_Currency_TagName);
            if (_sc != null)
            {
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                DateTime _UpdateDate = dtDateTime.AddSeconds(_CurrencyData.Timestamp).ToLocalTime();
                DateTime _StartDate = _UpdateDate.Date;
                DateTime _EndDate = _UpdateDate.Date.AddDays(1).AddSeconds(-1);
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
                                if (this.DbContainer.HasCurrencyExchangeHistorical(_sc.UID, _tc.UID, _StartDate, _EndDate))
                                {
                                    ch.SourceCurrency = _sc.UID;
                                    ch.TargetCurrency = _tc.UID;
                                    ch.UpdateDate = _UpdateDate;
                                    ch.Uid = Guid.NewGuid();
                                    ch.Value = Convert.ToDecimal(_scER.Value / item.Value);
                                    _Histories.Add(ch);
                                }
                            }
                            else
                            {
                                _tc = map.FirstOrDefault(p => p.TagName == "USD");
                                if (this.DbContainer.HasCurrencyExchangeHistorical(_sc.UID, _tc.UID, _StartDate, _EndDate))
                                {
                                    ch.SourceCurrency = _sc.UID;
                                    ch.TargetCurrency = _tc.UID;
                                    ch.UpdateDate = _UpdateDate;
                                    ch.Uid = Guid.NewGuid();
                                    ch.Value = Convert.ToDecimal(item.Value);
                                    _Histories.Add(ch);
                                }

                            }
                        }

                    }


                }
            }
        }

        protected override  void Crawl()
        {
            using (HttpClient client = new HttpClient())
            {
                var _json = client.GetStringAsync(this._API_Url).Result;
                try
                {
                    _CurrencyData = CurrencyApi3Data.FromJson(_json);
                }
                catch
                {

                }
            }
        }

        protected override void Import()
        {
            var _db = this.GetDbContext<FinancialContext>();
            _db.CurrencyHistory.AddRange(_Histories);
            _db.SaveChanges();

        }
    }
}
