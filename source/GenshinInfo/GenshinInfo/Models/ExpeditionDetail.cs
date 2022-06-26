using GenshinInfo.Constants.Indexes;
using GenshinInfo.Enums;

using System;
using System.Linq;
using System.Text.Json;

namespace GenshinInfo.Models
{
    /// <summary>
    /// Expedition detail info in Real-Time Note data
    /// </summary>
    public class ExpeditionDetail
    {
        public string AvatarSideIconUrl { get; }
        public string AvatarName { get; }
        public ExpeditionStatus Status { get; }
        public TimeSpan RemainedTime { get; }

        public ExpeditionDetail(JsonElement element)
        {
            AvatarSideIconUrl = element.GetProperty(RTNote.ExpeditionAvatarSideIcon).GetString();
            AvatarName = GetAvatarName(AvatarSideIconUrl.Split('/').LastOrDefault());
            Status = (element.GetProperty(RTNote.ExpeditionStatus).GetString() is "Ongoing") ?
                ExpeditionStatus.Ongoing :
                ExpeditionStatus.Finished;
            RemainedTime = Utils.ConvertRemainTime(
                element.GetProperty(RTNote.ExpeditionRemainedTime).GetString());
        }

        private string GetAvatarName(string imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName))
            {
                return string.Empty;
            }

            string tempName = imageName.Split('.')[0].Split('_').LastOrDefault();

            return tempName switch
            {
                "Ambor" => "Amber",
                "Itto" => "Arataki Itto",
                "Ayato" => "Kamisato Ayato",
                "Sara" => "Kujou Sara",
                "Noel" => "Noelle",
                "Kokomi" => "Sangonomiya Kokomi",
                "Yae" => "Yae Miko",
                "Hutao" => "HuTao",
                "Feiyan" => "Yanfei",
                "Qin" => "Jean",
                "Tohma" => "Thoma",
                "Yunjin" => "Yun Jin",
                "Shinobu" => "Kuki Shinobu",
                //라이덴쇼군
                _ => tempName
            };
        }
    }
}