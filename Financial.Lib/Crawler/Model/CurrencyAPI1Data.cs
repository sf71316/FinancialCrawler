using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Financial.Lib.Crawler.Model
{

    public partial class CurrencyAPI1Data
    {
        [JsonProperty("UTC")]
        public DateTimeOffset Utc { get; set; }

        [JsonProperty("Exrate")]
        public double Exrate { get; set; }
    }

    public partial class CurrencyAPI1Data
    {
        public static Dictionary<string, CurrencyAPI1Data> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, CurrencyAPI1Data>>(json, Converter.Settings);
        internal static class Converter
        {
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

    public static class Serialize
    {
        public static string ToJson(this Dictionary<string, CurrencyAPI1Data> self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }




}
