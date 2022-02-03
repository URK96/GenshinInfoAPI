﻿using GenshinInfo.Constants;

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

        public async Task<(bool, string)> GetRequestAsync(HttpClient client, string url)
        {
            bool result;
            string str;
            
            try
            {
                str = await client.GetStringAsync(url);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                str = ex.ToString();
            }

            return (result, str);
        }

        public async Task<(bool, string)> PostRequestAsync(HttpClient client, string url, StringContent content)
        {
            bool result;
            string str;

            try
            {
                str = await (await client.PostAsync(url, content)).Content.ReadAsStringAsync();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                str = ex.ToString();
            }

            return (result, str);
        }

        internal async Task<(bool, string)> GetRequestHoyolabAsync(HttpClient client, string endPoint)
        {
            return await GetRequestAsync(client, $"{Urls.TakumiUrl}{endPoint}");
        }

        internal async Task<(bool, string)> GetRequestGameRecordAsync(HttpClient client, string endPoint, string queryStr)
        {
            return await GetRequestAsync(client, $"{Urls.RecordBbsUrl}{endPoint}{queryStr}");
        }

        internal async Task<(bool, string)> PostRequestGameRecordAsync(HttpClient client, string endPoint, StringContent content)
        {
            return await PostRequestAsync(client, $"{Urls.RecordBbsUrl}{endPoint}", content);
        }

        internal async Task<(bool, string)> GetRequestRealTimeNoteAsync(string uid, string ltuid, string ltoken)
        {
            using HttpClient client = new();

            AddDefaultHeaders(client, ltuid, ltoken);

            string queryStr = $"?server={Utils.AnalyzeServer(uid)}&role_id={uid}";

            return await GetRequestGameRecordAsync(client, "genshin/api/dailyNote", queryStr);
        }

        internal async Task<(bool, string)> GetRequestUserAsync(string uid, string ltuid, string ltoken)
        {
            using HttpClient client = new();

            AddDefaultHeaders(client, ltuid, ltoken);

            string queryStr = $"?server={Utils.AnalyzeServer(uid)}&role_id={uid}";

            return await GetRequestGameRecordAsync(client, "genshin/api/index", queryStr);
        }

        internal async Task<(bool, string)> PostRequestGameRecordSettingAsync(string ltuid, string ltoken, StringContent content)
        {
            using HttpClient client = new();

            AddDefaultHeaders(client, ltuid, ltoken);

            return await PostRequestGameRecordAsync(client, "card/wapi/changeDataSwitch", content);
        }

        internal async Task<(bool, string)> GetRequestGachaInfoAsync(HttpClient client, string endPoint, string queryStr)
        {
            return await GetRequestAsync(client, $"{Urls.GachaInfoUrl}{endPoint}{queryStr}");
        }

        internal async Task<(bool, string)> GetRequestGachaLogAsync(int gachaType, string endId, 
                                                                    string langShortCode, string authKey)
        {
            using HttpClient client = new();

            StringBuilder querySb = new();

            querySb.Append($"?gacha_type={gachaType}");
            querySb.Append("&size=20");
            querySb.Append($"&end_id={endId}");
            querySb.Append("&authkey_ver=1");
            querySb.Append($"&lang={langShortCode}");
            querySb.Append($"&authkey={authKey}");

            return await GetRequestGachaInfoAsync(client, "getGachaLog", querySb.ToString());
        }

        public void AddDefaultHeaders(HttpClient client, string ltuid, string ltoken)
        {
            client.DefaultRequestHeaders.Add("Cookie", $"ltoken={ltoken}; ltuid={ltuid}");
            client.DefaultRequestHeaders.Add("x-rpc-app_version", "1.5.0");
            client.DefaultRequestHeaders.Add("x-rpc-client_type", "4");
            client.DefaultRequestHeaders.Add("DS", Utils.GenerateDS());
        }
    }
}
