using GenshinInfo.Constants;
using GenshinInfo.Constants.Indexes;
using GenshinInfo.Enums;
using GenshinInfo.Models;

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GenshinInfo.Services
{
    /// <summary>
    /// Test GenshinInfo API
    /// </summary>
    public static class TestAPIService
    {
        const string APINotResponseMessage = "API did not return response";
        const string ResponseIsNullMessage = "Response is not valid";

        /// <summary>
        /// Test Real-Time Note API response
        /// </summary>
        /// <param name="uid">User UID</param>
        /// <param name="ltuid">HoYoLAB user ltuid cookie info</param>
        /// <param name="ltoken">HoYoLAB user ltoken cookie info</param>
        /// <returns></returns>
        public static async Task<(bool, string)> TestRealTimeNoteAPI(string uid, string ltuid, string ltoken)
        {
            try
            {
                (bool result, string jsonStr) = 
                    await WebService.Instance.GetRequestRealTimeNoteAsync(uid, ltuid, ltoken);

                if (!result)
                {
                    return (false, APINotResponseMessage);
                }

                if (string.IsNullOrWhiteSpace(jsonStr))
                {
                    return (false, ResponseIsNullMessage);
                }

                return (true, jsonStr);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// Test Real-Time Note setting API response
        /// </summary>
        /// <param name="ltuid">HoYoLAB user ltuid cookie info</param>
        /// <param name="ltoken">HoYoLAB user ltoken cookie info</param>
        /// <returns></returns>
        public static async Task<(bool, string)> TestRealTimeNoteSettingAPI(string ltuid, string ltoken)
        {
            const string requestUrl = "https://bbs-api-os.mihoyo.com/game_record/card/wapi/changeDataSwitch";
            (bool? getResult, string getMessage) = await GetRealTimeNoteSettingValue(ltuid, ltoken);

            if (getResult is null)
            {
                return (false, getMessage);
            }

            using HttpClient client = new();

            WebService.Instance.AddDefaultHeaders(client, ltuid, ltoken);

            string str = $"{{\"game_id\":2,\"is_public\":{(getResult).ToString().ToLower()},\"switch_id\":3}}";
            string str2 = $"{{\"game_id\":2,\"is_public\":{getResult.ToString().ToLower()},\"switch_id\":3}}";

            using StringContent content = new(str);
            using StringContent content2 = new(str2);

            (bool result, string message) first;
            (bool result, string message) second;

            try
            {
                first = await WebService.Instance.PostRequestAsync(client, requestUrl, content);

                await Task.Delay(1000);

                second = await WebService.Instance.PostRequestAsync(client, requestUrl, content2);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString());
            }

            return (first.result && second.result, $"{first.message}\n{second.message}");
        }

        private static async Task<(bool?, string)> GetRealTimeNoteSettingValue(string ltuid, string ltoken)
        {
            using HttpClient client = new();

            WebService.Instance.AddDefaultHeaders(client, ltuid, ltoken);

            string queryStr = $"?uid={ltuid}";
            string requestUrl = $"{Urls.RecordBbsUrl}card/wapi/getGameRecordCard{queryStr}";

            (bool result, string jsonStr) = await WebService.Instance.GetRequestAsync(client, requestUrl);

            if (!result)
            {
                return (null, APINotResponseMessage);
            }

            if (string.IsNullOrWhiteSpace(jsonStr))
            {
                return (null, ResponseIsNullMessage);
            }

            (ResponseData responseData, GameRecordCardData recordData) =
                ((ResponseData, GameRecordCardData))ResponseData.CreateData(result, jsonStr, DataType.GameRecordCard);

            if (responseData is null)
            {
                return (null, ResponseIsNullMessage);
            }

            if (responseData.RetCode is not 0)
            {
                return (null, responseData.Message);
            }

            try
            {
                DataSwitchInfo info = recordData.DataSwitches.Find(x => x.SwitchId is 3);

                return (info.IsPublic, "Success to get Real-Time Note Setting");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public static async Task<(bool, string)> TestWishLogAPI(string authKey)
        {
            using HttpClient client = new();

            string queryStr = $"?authkey_ver=1&lang=ko&authkey={authKey}";
            string requestUrl = $"{Urls.GachaInfoUrl}getConfigList{queryStr}";

            return await WebService.Instance.GetRequestAsync(client, requestUrl);
        }
    }
}
