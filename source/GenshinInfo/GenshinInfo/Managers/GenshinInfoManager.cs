using GenshinInfo.Enums;
using GenshinInfo.Models;
using GenshinInfo.Services;

using System.Net.Http;
using System.Threading.Tasks;

namespace GenshinInfo.Managers
{
    public class GenshinInfoManager
    {
        private readonly string uid;
        private readonly string ltuid;
        private readonly string ltoken;

        public GenshinInfoManager(string uid, string ltuid, string ltoken)
        {
            this.uid = uid;
            this.ltuid = ltuid;
            this.ltoken = ltoken;
        }

        public async Task<bool> CheckLogin()
        {
            (bool result, string jsonStr) = await WebService.Instance.GetRequestUserAsync(uid, ltuid, ltoken);
            (ResponseData data, _) = ResponseData.CreateData(result, jsonStr, DataType.None);

            //if (result &&
            //    !string.IsNullOrWhiteSpace(jsonStr))
            //{
            //    data = new(JsonDocument.Parse(jsonStr).RootElement);
            //}

            return (data is not null) &&
                data.RetCode is 0;
        }

        public async Task<RTNoteData> GetRealTimeNotes()
        {
            (bool result, string jsonStr) = 
                await WebService.Instance.GetRequestRealTimeNoteAsync(uid, ltuid, ltoken);
            (_, var rtNoteData)
                = ((ResponseData, RTNoteData))ResponseData.CreateData(result, jsonStr, DataType.RTNote);

            return rtNoteData;

            //if (result &&
            //    !string.IsNullOrWhiteSpace(jsonStr))
            //{
            //    JsonElement rootElement = JsonDocument.Parse(jsonStr).RootElement;

            //    //responseData = new(rootElement);

            //    if (responseData?.RetCode is 0)
            //    {
            //        rtNoteData = new(rootElement.GetProperty(Response.Data));
            //    }
            //}

            //return rtNoteData;

            //JObject dataObj = await WebService.Instance.GetRequestRealTimeNoteAsync(uid, ltuid, ltoken);

            //Dictionary<string, string> dic = null;

            //if (dataObj is not null)
            //{
            //    dic = new();

            //    try
            //    {
            //        dic.Add(RTNote.CurrentResin, dataObj[RTNote.CurrentResin].Value<string>());
            //        dic.Add(RTNote.MaxResin, dataObj[RTNote.MaxResin].Value<string>());
            //        dic.Add(RTNote.ResinRecoveryTime, dataObj[RTNote.ResinRecoveryTime].Value<string>());

            //        dic.Add(RTNote.FinishedTaskNum, dataObj[RTNote.FinishedTaskNum].Value<string>());
            //        dic.Add(RTNote.TotalTaskNum, dataObj[RTNote.TotalTaskNum].Value<string>());
            //        dic.Add(RTNote.IsExtraTaskRewardReceived, 
            //                dataObj[RTNote.IsExtraTaskRewardReceived].Value<string>());

            //        dic.Add(RTNote.RemainResinDiscountNum, 
            //                dataObj[RTNote.RemainResinDiscountNum].Value<string>());
            //        dic.Add(RTNote.ResinDiscountNumLimit, 
            //                dataObj[RTNote.ResinDiscountNumLimit].Value<string>());

            //        dic.Add(RTNote.CurrentRealmHomeCoin, dataObj[RTNote.CurrentRealmHomeCoin].Value<string>());
            //        dic.Add(RTNote.MaxRealmHomeCoin, dataObj[RTNote.MaxRealmHomeCoin].Value<string>());
            //        dic.Add(RTNote.RealmHomeCoinRecoveryTime, 
            //                dataObj[RTNote.RealmHomeCoinRecoveryTime].Value<string>());
            //    }
            //    catch (Exception)
            //    {
            //        dic = null;
            //    }
            //}

            //return dic;
        }

        public async Task<bool> SetRealTimeNoteSetting(bool isEnable)
        {
            return await SetRealTimeNoteSetting(isEnable, ltuid, ltoken);
        }

        public static async Task<bool> SetRealTimeNoteSetting(bool isEnable, string ltuid, string ltoken)
        {
            string switchValue = isEnable.ToString().ToLower();
            string str = $"{{\"game_id\":2,\"is_public\":{switchValue},\"switch_id\":3}}";

            StringContent content = new(str);

            (bool result, _) = await WebService.Instance.PostRequestGameRecordSettingAsync(ltuid, ltoken, content);

            return result;
        }
    }
}
