using System;
using Newtonsoft.Json;

namespace Client.Tushare
{
    public class TushareData
    {
        [JsonProperty("fields")]
        public string[] Fields { get; set; }

        [JsonProperty("items")]
        public string[][] Items { get; set; }
    }
    public class TushareResult
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("data")]
        public TushareData Data { get; set; }
    }
}

