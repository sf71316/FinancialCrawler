using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Financial.Lib.Crawler.Model
{
    public partial class ApiErrorModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public Error Error { get; set; }
    }

    public partial class Error
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("info")]
        public string Info { get; set; }
    }
    public partial class ApiErrorModel
    {
        public static ApiErrorModel FromJson(string json) => JsonConvert.DeserializeObject<ApiErrorModel>(json, Settings);
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
