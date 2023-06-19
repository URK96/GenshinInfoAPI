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
                   (data.RetCode is 0);
        }

        /// <summary>
        /// Get Real-Time Note content
        /// </summary>
        /// <returns>Real-Time Note instance</returns>
        public async Task<RTNoteData> GetRealTimeNotes()
        {
            (bool result, string jsonStr) = 
                await WebService.Instance.GetRequestRealTimeNoteAsync(uid, ltuid, ltoken);
            (_, var rtNoteData) =
                ((ResponseData, RTNoteData))ResponseData.CreateData(result, jsonStr, DataType.RTNote);

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

        /// <summary>
        /// Get Daily Reward item list & info with user cookie info
        /// </summary>
        /// <param name="langCode">Data language code (ex. en-us)</param>
        /// <returns>Daily Reward list data instance</returns>
        public async Task<DailyRewardListData> GetDailyRewardList(string langCode = "en-us")
        {
            (bool result, string jsonStr) =
                await WebService.Instance.GetRequestDailyRewardListDataAsync(ltuid, ltoken, langCode);
            (_, var listData) = 
                ((ResponseData, DailyRewardListData))ResponseData.CreateData(result, jsonStr, DataType.DailyRewardList);

            return listData;
        }

        /// <summary>
        /// Get Daily Reward status info with user cookie info
        /// </summary>
        /// <param name="langCode">Data language code (ex. en-us)</param>
        /// <returns>Daily Reward status data instance</returns>
        public async Task<DailyRewardStatusData> GetDailyRewardStatus(string langCode = "en-us")
        {
            (bool result, string jsonStr) = 
                await WebService.Instance.GetRequestDailyRewardStatusDataAsync(ltuid, ltoken, langCode);
            (_, var statusData) = 
                ((ResponseData, DailyRewardStatusData))ResponseData.CreateData(result, jsonStr, DataType.DailyRewardStatus);

            return statusData;
        }

        /// <summary>
        /// Sign in Daily Reward with user cookie info
        /// </summary>
        /// <returns>Request & Sign-In result</returns>
        public async Task<bool> SignInDailyReward()
        {
            (bool result, string jsonStr) = 
                await WebService.Instance.PostRequestDailyRewardSignInAsync(ltuid, ltoken, "en-us");
            (var responseData, var resultData) =
                ((ResponseData, DailyRewardSignInResultData))ResponseData.CreateData(result, jsonStr, DataType.DailyRewardSingInResult);

            if (resultData?.GtResult?.ChallengeData is not null)
            {
                (result, jsonStr) =
                    await WebService.Instance.PostRequestDailyRewardSignInAgainAsync(ltuid, ltoken, "en-us", resultData.GtResult.ChallengeData);

                (responseData, resultData) =
                    ((ResponseData, DailyRewardSignInResultData))ResponseData.CreateData(result, jsonStr, DataType.DailyRewardSingInResult);
            }

            return result && 
                   (responseData.RetCode is -5003 or 0);
        }

        #region HonkaiAPI

        /// <summary>
        /// Get Honkai Impact Daily Reward item list & info with user cookie info
        /// </summary>
        /// <param name="langCode">Data language code (ex. en-us)</param>
        /// <returns>Daily Reward list data instance</returns>
        public async Task<DailyRewardListData> GetHonkaiDailyRewardList(string langCode = "en-us")
        {
            (bool result, string jsonStr) =
                await WebService.Instance.GetRequestHonkaiDailyRewardListDataAsync(ltuid, ltoken, langCode);
            (_, var listData) =
                ((ResponseData, DailyRewardListData))ResponseData.CreateData(result, jsonStr, DataType.DailyRewardList);

            return listData;
        }

        /// <summary>
        /// Get Honkai Impact Daily Reward status info with user cookie info
        /// </summary>
        /// <param name="langCode">Data language code (ex. en-us)</param>
        /// <returns>Daily Reward status data instance</returns>
        public async Task<DailyRewardStatusData> GetHonkaiDailyRewardStatus(string langCode = "en-us")
        {
            (bool result, string jsonStr) =
                await WebService.Instance.GetRequestHonkaiDailyRewardStatusDataAsync(ltuid, ltoken, langCode);
            (_, var statusData) =
                ((ResponseData, DailyRewardStatusData))ResponseData.CreateData(result, jsonStr, DataType.DailyRewardStatus);

            return statusData;
        }

        /// <summary>
        /// Sign in Honkai Impact Daily Reward with user cookie info
        /// </summary>
        /// <returns>Request & Sign-In result</returns>
        public async Task<bool> SignInHonkaiDailyReward()
        {
            (bool result, string jsonStr) =
                await WebService.Instance.PostRequestHonkaiDailyRewardSignInAsync(ltuid, ltoken, "en-us");
            (var responseData, _) = ResponseData.CreateData(result, jsonStr, DataType.None);

            return result &&
                   (responseData.RetCode is -5003 or 0);
        }

        #endregion HonkaiAPI

        #region HonkaiStarRailAPI

        /// <summary>
        /// Get Honkai Star Rail Daily Reward item list & info with user cookie info
        /// </summary>
        /// <param name="langCode">Data language code (ex. en-us)</param>
        /// <returns>Daily Reward list data instance</returns>
        public async Task<DailyRewardListData> GetHonkaiStarRailDailyRewardList(string langCode = "en-us")
        {
            (bool result, string jsonStr) =
                await WebService.Instance.GetRequestHonkaiStarRailDailyRewardListDataAsync(ltuid, ltoken, langCode);
            (_, var listData) =
                ((ResponseData, DailyRewardListData))ResponseData.CreateData(result, jsonStr, DataType.DailyRewardList);

            return listData;
        }

        /// <summary>
        /// Get Honkai Star Rail Daily Reward status info with user cookie info
        /// </summary>
        /// <param name="langCode">Data language code (ex. en-us)</param>
        /// <returns>Daily Reward status data instance</returns>
        public async Task<DailyRewardStatusData> GetHonkaiStarRailDailyRewardStatus(string langCode = "en-us")
        {
            (bool result, string jsonStr) =
                await WebService.Instance.GetRequestHonkaiStarRailDailyRewardStatusDataAsync(ltuid, ltoken, langCode);
            (_, var statusData) =
                ((ResponseData, DailyRewardStatusData))ResponseData.CreateData(result, jsonStr, DataType.DailyRewardStatus);

            return statusData;
        }

        /// <summary>
        /// Sign in Honkai Star Rail Daily Reward with user cookie info
        /// </summary>
        /// <returns>Request & Sign-In result</returns>
        public async Task<bool> SignInHonkaiStarRailDailyReward()
        {
            (bool result, string jsonStr) =
                await WebService.Instance.PostRequestHonkaiStarRailDailyRewardSignInAsync(ltuid, ltoken, "en-us");
            (var responseData, _) = ResponseData.CreateData(result, jsonStr, DataType.None);

            return result &&
                   (responseData.RetCode is -5003 or 0);
        }

        #endregion HonkaiStarRailAPI
    }
}
