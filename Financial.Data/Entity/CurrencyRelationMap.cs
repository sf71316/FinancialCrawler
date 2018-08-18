using System;
using System.Collections.Generic;
using System.Text;

namespace Financial.Data
{
    public class CurrencyRelationMap
    {
        public Guid UID { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string Country { get; set; }
        public string TagName { get; set; }
    }
}
