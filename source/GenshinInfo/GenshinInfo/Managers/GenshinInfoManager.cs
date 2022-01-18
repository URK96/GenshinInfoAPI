using GenshinInfo.Services;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using RTNIndexes = GenshinInfo.Constants.Indexes.RealTimeNote;

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
            var data = await WebService.Instance.GetRequestUserAsync(uid, ltuid, ltoken);

            return data is not null;
        }

        public async Task<Dictionary<string, string>> GetRealTimeNotes()
        {
            JObject dataObj = await WebService.Instance.GetRequestRealTimeNoteAsync(uid, ltuid, ltoken);

            Dictionary<string, string> dic = null;

            if (dataObj is not null)
            {
                dic = new();

                try
                {
                    dic.Add(RTNIndexes.CurrentResin, dataObj[RTNIndexes.CurrentResin].Value<string>());
                    dic.Add(RTNIndexes.MaxResin, dataObj[RTNIndexes.MaxResin].Value<string>());
                    dic.Add(RTNIndexes.ResinRecoveryTime, dataObj[RTNIndexes.ResinRecoveryTime].Value<string>());

                    dic.Add(RTNIndexes.FinishedTaskNum, dataObj[RTNIndexes.FinishedTaskNum].Value<string>());
                    dic.Add(RTNIndexes.TotalTaskNum, dataObj[RTNIndexes.TotalTaskNum].Value<string>());
                    dic.Add(RTNIndexes.IsExtraTaskRewardReceived, 
                            dataObj[RTNIndexes.IsExtraTaskRewardReceived].Value<string>());

                    dic.Add(RTNIndexes.RemainResinDiscountNum, 
                            dataObj[RTNIndexes.RemainResinDiscountNum].Value<string>());
                    dic.Add(RTNIndexes.ResinDiscountNumLimit, 
                            dataObj[RTNIndexes.ResinDiscountNumLimit].Value<string>());

                    dic.Add(RTNIndexes.CurrentRealmHomeCoin, dataObj[RTNIndexes.CurrentRealmHomeCoin].Value<string>());
                    dic.Add(RTNIndexes.MaxRealmHomeCoin, dataObj[RTNIndexes.MaxRealmHomeCoin].Value<string>());
                    dic.Add(RTNIndexes.RealmHomeCoinRecoveryTime, 
                            dataObj[RTNIndexes.RealmHomeCoinRecoveryTime].Value<string>());
                }
                catch (Exception)
                {
                    dic = null;
                }
            }

            return dic;
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

            return await WebService.Instance.RequestGameRecordSettingAsync(ltuid, ltoken, content);
        }
    }
}
