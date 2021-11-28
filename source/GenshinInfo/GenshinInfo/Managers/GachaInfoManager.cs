using GenshinInfo.Constants;
using GenshinInfo.Models;
using GenshinInfo.Services;

using Newtonsoft.Json.Linq;

using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
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

        public async Task<string> GetGachaConfigList()
        {
            using HttpClient client = new();

            StringBuilder querySb = new();

            querySb.Append("?authkey_ver=1");
            querySb.Append($"&lang=ko");
            querySb.Append($"&authkey={authKey}");

            string url = $"https://hk4e-api-os.mihoyo.com/event/gacha_info/api/getConfigList{querySb}";

            return await client.GetStringAsync(url);
        }

        public string CreateGachaLogWebViewerUrl(string langShortCode = "en")
        {
            string url = string.Empty;

            if (!string.IsNullOrWhiteSpace(authKey))
            {
                url = $"{Urls.WebStaticUrl}hk4e/event/e20190909gacha/index.html";
                StringBuilder querySb = new();

                querySb.Append("?authkey_ver=1");
                querySb.Append("&sign_type=2");
                querySb.Append("&auth_appid=webview_gacha");
                querySb.Append($"&init_type={(int)GachaTypeNum.Permanent}");
                querySb.Append($"&lang={langShortCode}");
                querySb.Append("&device_type=mobile");
                querySb.Append($"&authkey={authKey}");
                querySb.Append("&game_biz=hk4e_global");

                url = $"{url}{querySb}#/log";
            }

            return url;
        }
    }
}
