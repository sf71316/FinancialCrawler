using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace Financial.Lib.Crawler
{


    public partial class CurrencyInfos
    {
        [JsonProperty("results")]
        public Dictionary<string, Result> Results { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("alpha3")]
        public string Alpha3 { get; set; }

        [JsonProperty("currencyId")]
        public string CurrencyId { get; set; }

        [JsonProperty("currencyName")]
        public string CurrencyName { get; set; }

        [JsonProperty("currencySymbol")]
        public string CurrencySymbol { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class CurrencyInfos
    {
        public static CurrencyInfos FromJson(string json) => JsonConvert.DeserializeObject<CurrencyInfos>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this CurrencyInfos self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

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
