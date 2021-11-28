﻿using GenshinInfo.Constants;

using Newtonsoft.Json.Linq;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GenshinInfo.Services
{
    public static class TestAPIService
    {
        public static async Task<(bool, string)> TestRealTimeNoteAPI(string uid, string ltuid, string ltoken)
        {
            using HttpClient client = new();

            WebService.Instance.AddDefaultHeaders(client, ltuid, ltoken);

            string queryStr = $"?server={Utils.AnalyzeServer(uid)}&role_id={uid}";
            string requestUrl = $"{Urls.RecordUrl}genshin/api/dailyNote{queryStr}";

            return await WebService.Instance.RawGetRequestAsync(client, requestUrl);
        }

        public static async Task<(bool, string)> TestRealTimeNoteSettingAPI(string ltuid, string ltoken)
        {
            bool? nowStatus = await GetRealTimeNoteSettingValue(ltuid, ltoken);

            if (nowStatus is null)
            {
                return (false, "HoYoLAB Privacy Setting response not working");
            }

            using HttpClient client = new();

            WebService.Instance.AddDefaultHeaders(client, ltuid, ltoken);

            string str = $"{{\"game_id\":2,\"is_public\":{(!nowStatus.Value).ToString().ToLower()},\"switch_id\":3}}";
            string str2 = $"{{\"game_id\":2,\"is_public\":{nowStatus.Value.ToString().ToLower()},\"switch_id\":3}}";

            using StringContent content = new(str);
            using StringContent content2 = new(str2);

            (bool, string) first;
            (bool, string) second;

            try
            {
                first = await WebService.Instance.RawPostRequestAsync(client, "https://bbs-api-os.mihoyo.com/game_record/card/wapi/changeDataSwitch", content);

                await Task.Delay(1000);

                second = await WebService.Instance.RawPostRequestAsync(client, "https://bbs-api-os.mihoyo.com/game_record/card/wapi/changeDataSwitch", content2);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString());
            }

            return (first.Item1 && second.Item1, $"{first.Item2}\n{second.Item2}");
        }

        private static async Task<bool?> GetRealTimeNoteSettingValue(string ltuid, string ltoken)
        {
            using HttpClient client = new();

            WebService.Instance.AddDefaultHeaders(client, ltuid, ltoken);

            string queryStr = $"?uid={ltuid}";
            string requestUrl = $"{Urls.RecordBbsUrl}card/wapi/getGameRecordCard{queryStr}";

            JObject dataObj;

            try
            {
                dataObj = await WebService.Instance.GetRequestAsync(client, requestUrl);

                if (dataObj is null)
                {
                    throw new Exception("Response data is null");
                }
            }
            catch (Exception)
            {
                return null;
            }

            JToken token = dataObj["list"][0]["data_switches"][2];

            return token["is_public"].Value<bool>();
        }
    }
}