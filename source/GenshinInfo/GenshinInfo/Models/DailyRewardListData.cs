using GenshinInfo.Constants.Indexes;

using System.Collections.Generic;
using System.Text.Json;

namespace GenshinInfo.Models
{
    public class DailyRewardListData : BaseData
    {
        public int TargetMonth { get; private set; }
        public List<DailyRewardListItemData> Rewards { get; private set; } = new();

        public DailyRewardListData(JsonElement element)
        {
            TargetMonth = element.GetProperty(DailyReward.AwardMonth).GetInt32();

            foreach (var itemElement in element.GetProperty(DailyReward.Awards).EnumerateArray())
            {
                Rewards.Add(new()
                {
                    IconUrl = itemElement.GetProperty(DailyReward.AwardIcon).GetString(),
                    ItemName = itemElement.GetProperty(DailyReward.AwardName).GetString(),
                    ItemCount = itemElement.GetProperty(DailyReward.AwardCount).GetInt32()
                });
            }
        }
    }
}
