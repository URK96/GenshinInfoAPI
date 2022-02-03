using GenshinInfo.Constants.Indexes;
using GenshinInfo.Enums;

using System;
using System.Text.Json;

namespace GenshinInfo.Models
{
    public class ExpeditionDetail
    {
        public string AvatarSideIconUrl { get; }
        public ExpeditionStatus Status { get; }
        public TimeSpan RemainedTime { get; }

        public ExpeditionDetail(JsonElement element)
        {
            AvatarSideIconUrl = element.GetProperty(RTNote.ExpeditionAvatarSideIcon).GetString();
            Status = (element.GetProperty(RTNote.ExpeditionStatus).GetString() is "Ongoing") ?
                ExpeditionStatus.Ongoing :
                ExpeditionStatus.Finished;
            RemainedTime = Utils.ConvertRemainTime(
                element.GetProperty(RTNote.ExpeditionRemainedTime).GetString());
        }
    }
}
