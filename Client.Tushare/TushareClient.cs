using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client.Tushare
{
    public interface ITushareClient
    {
        Task<TushareResult> GetCsi300Async(DateTime startDate, DateTime endDate);
    }
    public class TushareClient : ITushareClient
    {
        public async Task<TushareResult> GetCsi300Async(DateTime startDate, DateTime endDate)
        {
            var param = new TushareParam {
                api_name = "index_daily",
                token = "3ef55872ae70a785207c7bfa011e495b32027baf7bddf14cd1895ac6",
                Params = new Dictionary<string, string> {
                    { "start_date", startDate.ToString("yyyyMMdd")},
                    { "end_date", endDate.ToString("yyyyMMdd")},
                    { "ts_code","000300.SH"}
                },
                fields = "ts_code,trade_date,close,open,high,low,pre_close,change,pct_chg,vol,amount"
            };
          
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.Timeout = new TimeSpan(0, 0, 5);
            var content = JsonContent.Create(param);
            var response = await httpClient.PostAsync("http://api.waditu.com/", content);
            var str = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TushareResult>(str);
            return result;
        }
    }
}
