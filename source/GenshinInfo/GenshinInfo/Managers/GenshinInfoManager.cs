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

        /// <summary>
        /// Check login with user's UID & cookie infos (ltuid, ltoken)
        /// </summary>
        /// <returns>Login result</returns>
        public async Task<bool> CheckLogin()
        {
            (bool result, string jsonStr) = await WebService.Instance.GetRequestUserAsync(uid, ltuid, ltoken);
            (ResponseData data, _) = ResponseData.CreateData(result, jsonStr, DataType.None);

            return (data is not null) &&
                data.RetCode is 0;
        }

        /// <summary>
        /// Get Real-Time Note content
        /// </summary>
        /// <returns>Real-Time Note instance</returns>
        public async Task<RTNoteData> GetRealTimeNotes()
        {
            (bool result, string jsonStr) = 
                await WebService.Instance.GetRequestRealTimeNoteAsync(uid, ltuid, ltoken);
            (_, var rtNoteData)
                = ((ResponseData, RTNoteData))ResponseData.CreateData(result, jsonStr, DataType.RTNote);

            return rtNoteData;
        }

        /// <summary>
        /// Set Real-Time Note enable setting
        /// </summary>
        /// <param name="isEnable">Setting value</param>
        /// <returns>Set request result</returns>
        public async Task<bool> SetRealTimeNoteSetting(bool isEnable)
        {
            return await SetRealTimeNoteSetting(isEnable, ltuid, ltoken);
        }

        /// <summary>
        /// Set Real-Time Note enable setting with user cookie info
        /// </summary>
        /// <param name="isEnable">Setting value</param>
        /// <param name="ltuid">User cookie info (ltuid)</param>
        /// <param name="ltoken">User cookie info (ltoken)</param>
        /// <returns>Set request result</returns>
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
