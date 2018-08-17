using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financial.Data.Models
{
    public partial class FinancialContext
    {
        public FinancialContext()
        {
            this.CurrencyRelationMaps = this.Query<CurrencyRelationMap>();
        }
        public DbQuery<CurrencyRelationMap> CurrencyRelationMaps { get; protected set; }
    }
}
