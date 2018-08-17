using Financial.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;

namespace Financial.Lib.Crawler
{
    public class CurrencyInfoMaker : CrawlerBase
    {
        string _API_Url = "https://free.currencyconverterapi.com/api/v6/countries";
        public CurrencyInfoMaker()
        {

        }
        public override async void Execute()
        {
            using (HttpClient client = new HttpClient())
            {
                var _json = await client.GetStringAsync(this._API_Url).ConfigureAwait(false);
                var _currencyData = JsonConvert.DeserializeObject<CurrencyInfos>(_json);
                Import(_currencyData);
            }

        }
        private void Import(CurrencyInfos Source)
        {
            using (var context = this.GetDbContext<FinancialContext>())
            {
                ClearAllData(context);
                context.SaveChanges();
                foreach (var item in Source.Results)
                {
                    Financial.Data.Models.Currency c = new Financial.Data.Models.Currency();
                    CurrencyNameRelation r = new CurrencyNameRelation();
                    c.Country = item.Value.Name;
                    c.CurrencyName = item.Value.CurrencyName;
                    c.CurrencySymbol = item.Value.CurrencySymbol;
                    c.Uid = Guid.NewGuid();
                    r.CurrencyUid = c.Uid;
                    r.Name = item.Value.CurrencyId;
                    r.Uid = Guid.NewGuid();
                    context.Currency.Add(c);
                    context.CurrencyNameRelation.Add(r);
                }
                context.SaveChanges();
            }

        }

        private void ClearAllData(FinancialContext context)
        {
            if (context.Currency.Count() > 1)
                context.Currency.RemoveRange(context.Currency);
            if (context.CurrencyHistory.Count() > 1)
                context.CurrencyHistory.RemoveRange(context.CurrencyHistory);
            if (context.CurrencyNameRelation.Count() > 1)
                context.CurrencyNameRelation.RemoveRange(context.CurrencyNameRelation);
        }
    }
}
