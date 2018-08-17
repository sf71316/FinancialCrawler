using System;
using System.Collections.Generic;

namespace Financial.Data.Models
{
    public partial class CurrencyHistory
    {
        public Guid Uid { get; set; }
        public Guid SourceCurrency { get; set; }
        public Guid TargetCurrency { get; set; }
        public decimal Value { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
