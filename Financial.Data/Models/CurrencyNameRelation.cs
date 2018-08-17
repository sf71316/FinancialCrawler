using System;
using System.Collections.Generic;

namespace Financial.Data.Models
{
    public partial class CurrencyNameRelation
    {
        public Guid Uid { get; set; }
        public Guid CurrencyUid { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
