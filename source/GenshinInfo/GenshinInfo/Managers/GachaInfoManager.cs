using GenshinInfo.Models;
using GenshinInfo.Services;

using Newtonsoft.Json.Linq;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenshinInfo.Managers
{
    public class GachaInfoManager
    {
        public enum GachaTypeNum
        {
            Novice = 100,
            Permanent = 200,
            CharacterEvent = 301,
            WeaponEvent = 302
        }

        private string authKey;

        public GachaInfoManager() { }

        public void SetAuthKeyByUrl(string authKeyUrl)
        {
            authKey = Utils.ExtractAuthkey(authKeyUrl);
        }

        public void SetAuthKey(string authKey)
        {
            this.authKey = authKey;
        }

        public async Task<List<GachaInfo>> GetGachaInfos(GachaTypeNum gachaType, string endId = "0", string langShortCode = "en")
        {
            List<GachaInfo> resultList = null;
            JObject dataObj = await WebService.Instance.GetRequestGachaLogAsync(((int)gachaType).ToString(), endId, langShortCode, authKey);

            if (dataObj is not null)
            {
                JArray infoList = dataObj["list"].Value<JArray>();

                resultList = new();

                foreach (JObject obj in infoList)
                {
                    resultList.Add(new GachaInfo(obj));
                }
            }

            return resultList;
        }
    }
}
