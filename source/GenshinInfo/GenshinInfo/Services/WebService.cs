using GenshinInfo.Constants;

using Newtonsoft.Json.Linq;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GenshinInfo.Services
{
    public class WebService
    {
        public static WebService Instance => instance.Value;

        private static readonly Lazy<WebService> instance = new(() => { return new WebService(); });

        private async Task<JObject> GetRequestAsync(HttpClient client, string url)
        {
            try
            {
                string str = await client.GetStringAsync(url);

                JObject rootObj = JObject.Parse(str);

                int retcode = rootObj["retcode"].Value<int>();

                return (retcode is 0) ? rootObj["data"].Value<JObject>() : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<bool> PostRequestAsync(HttpClient client, string url, StringContent content)
        {
            try
            {
                string str = await (await client.PostAsync(url, content)).Content.ReadAsStringAsync();

                JObject rootObj = JObject.Parse(str);

                int retcode = rootObj["retcode"].Value<int>();

                return (retcode is 0) && rootObj["message"].Value<string>().Equals("OK");
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal async Task<JObject> GetRequestHoyolabAsync(HttpClient client, string endPoint)
        {
            return await GetRequestAsync(client, $"{Urls.TakumiUrl}{endPoint}");
        }

        internal async Task<JObject> GetRequestGameRecordAsync(HttpClient client, string endPoint, string queryStr)
        {
            return await GetRequestAsync(client, $"{Urls.RecordUrl}{endPoint}{queryStr}");
        }

        internal async Task<bool> PostRequestGameRecordAsync(HttpClient client, string endPoint, StringContent content)
        {
            return await PostRequestAsync(client, $"{Urls.RecordUrl}{endPoint}", content);
        }

        internal async Task<JObject> RequestRealTimeNoteAsync(string uid, string ltuid, string ltoken)
        {
            try
            {
                using HttpClient client = new();

                AddDefaultHeaders(client, ltuid, ltoken);

                string queryStr = $"?server={Utils.AnalyzeServer(uid)}&role_id={uid}";

                return await GetRequestGameRecordAsync(client, "genshin/api/dailyNote", queryStr);
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal async Task<JObject> RequestUserAsync(string uid, string ltuid, string ltoken)
        {
            try
            {
                using HttpClient client = new();

                AddDefaultHeaders(client, ltuid, ltoken);

                string queryStr = $"?server={Utils.AnalyzeServer(uid)}&role_id={uid}";

                return await GetRequestGameRecordAsync(client, "genshin/api/index", queryStr);
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal async Task<bool> RequestGameRecordSettingAsync(string ltuid, string ltoken, StringContent content)
        {
            try
            {
                using HttpClient client = new();

                AddDefaultHeaders(client, ltuid, ltoken);

                return await PostRequestGameRecordAsync(client, "card/wapi/changeDataSwitch", content);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void AddDefaultHeaders(HttpClient client, string ltuid, string ltoken)
        {
            client.DefaultRequestHeaders.Add("Cookie", $"ltoken={ltoken}; ltuid={ltuid}");
            client.DefaultRequestHeaders.Add("x-rpc-app_version", "1.5.0");
            client.DefaultRequestHeaders.Add("x-rpc-client_type", "4");
            client.DefaultRequestHeaders.Add("DS", Utils.GenerateDS());
        }
    }
}
