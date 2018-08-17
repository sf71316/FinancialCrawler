using Financial.Data.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Dapper;

namespace Financial.Data.Models
{
    public class CurrencyData : DbContextBase, ICurrencyData
    {

        public CurrencyData(string connectionString) : base(connectionString)
        {

        }
        public DbContext DbContext
        {

            get
            {
                return this._Context;
            }

        }

        public IQueryable<CurrencyRelationMap> GetCurrencyMap()
        {
            return (from c in this._Context.Currency
                    join cnr in this._Context.CurrencyNameRelation on c.Uid equals cnr.CurrencyUid
                    where c.IsTracking == true
                    select new CurrencyRelationMap
                    {
                        Country = c.Country,
                        CurrencyName = c.CurrencyName,
                        CurrencySymbol = c.CurrencySymbol,
                        TagName = cnr.Name,
                        UID = c.Uid
                    }).AsNoTracking();

        }

        public bool IsNewCurrencyExchange(Guid SourceCEGuid, Guid TargetCEGuid, DateTime UpdateDate)
        {

            return (from c in this._Context.CurrencyHistory
                    where c.SourceCurrency == SourceCEGuid && c.TargetCurrency == TargetCEGuid
                    && c.UpdateDate >= UpdateDate
                    select c).AsNoTracking().Count() == 0;
            //return result.Count() == 0;
        }
        public bool HasCurrencyExchangeHistorical(Guid SourceCEGuid, Guid TargetCEGuid, DateTime StartDate, DateTime EndDate)
        {

            return (from c in this._Context.CurrencyHistory
                    where c.SourceCurrency == SourceCEGuid && c.TargetCurrency == TargetCEGuid
                    && (c.UpdateDate >= StartDate && c.UpdateDate < EndDate)
                    select c).AsNoTracking().Count() == 0;
            //return result.Count() == 0;
        }
    }


}
