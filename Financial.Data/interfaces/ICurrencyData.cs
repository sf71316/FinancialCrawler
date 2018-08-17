using Financial.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Financial.Data.interfaces
{
    public interface ICurrencyData
    {
        DbContext DbContext { get; }
        IQueryable<CurrencyRelationMap> GetCurrencyMap();
        bool IsNewCurrencyExchange(Guid SourceCEGuid, Guid TargetCEGuid, DateTime UpdateDate);

    }

}
