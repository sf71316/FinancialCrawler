using System;
using System.Collections.Generic;

namespace Financial.Data.Models
{
    public partial class Currency
    {
        public Guid Uid { get; set; }
        public string Country { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public bool IsTracking { get; set; }
    }
}
