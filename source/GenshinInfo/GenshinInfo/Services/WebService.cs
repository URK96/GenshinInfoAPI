using GenshinInfo.Constants;

using Newtonsoft.Json.Linq;

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GenshinInfo.Services
{
    public class WebService
    {
        public static WebService Instance => instance.Value;

        private static readonly Lazy<WebService> instance = new(() => { return new WebService(); });

        public async Task<(bool, string)> RawGetRequestAsync(HttpClient client, string url)
        {
            bool result;
            string str;
            
            try
            {
                str = await client.GetStringAsync(url);

                JObject rootObj = JObject.Parse(str);

                int retCode = rootObj["retcode"].Value<int>();

                if (retCode is 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    str = rootObj["message"].Value<string>();
                }
            }
            catch (Exception ex)
            {
                result = false;
                str = ex.ToString();
            }

            return (result, str);
        }

        public async Task<(bool, string)> RawPostRequestAsync(HttpClient client, string url, StringContent content)
        {
            bool result;
            string str;

            try
            {
                str = await (await client.PostAsync(url, content)).Content.ReadAsStringAsync();

                JObject rootObj = JObject.Parse(str);

                int retCode = rootObj["retcode"].Value<int>();

                if (retCode is 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    str = rootObj["message"].Value<string>();
                }
            }
            catch (Exception ex)
            {
                result = false;
                str = ex.ToString();
            }

            return (result, str);
        }

        public async Task<JObject> GetRequestAsync(HttpClient client, string url)
        {
            string str;

            try
            {
                str = await client.GetStringAsync(url);

                JObject rootObj = JObject.Parse(str);

                int retCode = rootObj["retcode"].Value<int>();

                return (retCode is 0) ? rootObj["data"].Value<JObject>() : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> PostRequestAsync(HttpClient client, string url, StringContent content)
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
            return await GetRequestAsync(client, $"{Urls.RecordBbsUrl}{endPoint}{queryStr}");
        }

        internal async Task<bool> PostRequestGameRecordAsync(HttpClient client, string endPoint, StringContent content)
        {
            return await PostRequestAsync(client, $"{Urls.RecordBbsUrl}{endPoint}", content);
        }

        internal async Task<JObject> GetRequestRealTimeNoteAsync(string uid, string ltuid, string ltoken)
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

        internal async Task<JObject> GetRequestUserAsync(string uid, string ltuid, string ltoken)
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

        internal async Task<JObject> GetRequestGachaInfoAsync(HttpClient client, string endPoint, string queryStr)
        {
            return await GetRequestAsync(client, $"{Urls.GachaInfoUrl}{endPoint}{queryStr}");
        }

        internal async Task<JObject> GetRequestGachaLogAsync(string gachaType, string endId, string langShortCode, string authKey)
        {
            using HttpClient client = new();

            StringBuilder querySb = new();

            querySb.Append($"?gacha_type={gachaType}");
            querySb.Append("&size=20");
            querySb.Append("&auth_appid=webview_gacha");
            querySb.Append($"&end_id={endId}");
            querySb.Append("&authkey_ver=1");
            querySb.Append($"&lang={langShortCode}");
            querySb.Append($"&authkey={authKey}");

            return await GetRequestGachaInfoAsync(client, "getGachaLog", querySb.ToString());
        }

        internal void AddDefaultHeaders(HttpClient client, string ltuid, string ltoken)
        {
            client.DefaultRequestHeaders.Add("Cookie", $"ltoken={ltoken}; ltuid={ltuid}");
            client.DefaultRequestHeaders.Add("x-rpc-app_version", "1.5.0");
            client.DefaultRequestHeaders.Add("x-rpc-client_type", "4");
            client.DefaultRequestHeaders.Add("DS", Utils.GenerateDS());
        }
    }
}
