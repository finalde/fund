using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Client.Tushare
{
    public class TushareParam
    {
        [JsonPropertyName("api_name")]
        public string api_name { get; set; }

        [JsonPropertyName("token")]
        public string token { get; set; }

        [JsonPropertyName("params")]
        public Dictionary<string, string> Params { get; set; }

        [JsonPropertyName("fields")]
        public string fields { get; set; }
    }
}
