using GenshinInfo.Constants;
using GenshinInfo.Enums;
using GenshinInfo.Models;
using GenshinInfo.Services;

using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GenshinInfo.Managers
{
    public class GachaInfoManager
    {
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

        public async Task<List<GachaDataInfo>> GetGachaInfos(GachaTypeNum gachaType, string endId = "0",
                                                             string langShortCode = "en")
        {
            (bool result, string jsonStr) = 
                await WebService.Instance.GetRequestGachaLogAsync((int)gachaType, endId, langShortCode, authKey);

            (_, GachaDataInfos gachaDataInfos) =
                ((ResponseData, GachaDataInfos))ResponseData.CreateData(result, jsonStr, DataType.GachaData);

            if (gachaDataInfos is null)
            {
                return null;
            }

            return gachaDataInfos.GachaDatas;
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
