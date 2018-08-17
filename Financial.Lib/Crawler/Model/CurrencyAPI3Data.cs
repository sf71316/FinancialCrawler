using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Financial.Lib.Crawler.Model
{
    public partial class CurrencyApi3Data
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("terms")]
        public string Terms { get; set; }

        [JsonProperty("privacy")]
        public string Privacy { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("quotes")]
        public Dictionary<string, double> Quotes { get; set; }
    }

    public partial class CurrencyApi3Data
    {
        public static CurrencyApi3Data FromJson(string json) => JsonConvert.DeserializeObject<CurrencyApi3Data>(json, Settings);
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
        new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
      },
        };
    }
}
