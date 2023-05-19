using GenshinInfo.Constants.Indexes;

using System;
using System.Text.Json;

namespace GenshinInfo.Models
{
    public class DailyRewardStatusData : BaseData
    {
        public int TotalSignDayCount { get; private set; }
        public DateTime TodayDate { get; private set; }
        public bool IsSign { get; private set; }
        public bool IsFirstBind { get; private set; }
        public bool IsSub { get; private set; }
        public string Region { get; private set; }

        public DailyRewardStatusData(JsonElement element)
        {
            TotalSignDayCount = element.GetProperty(DailyReward.TotalSignDay).GetInt32();

            string[] todaySplits = element.GetProperty(DailyReward.TodayDate).GetString().Split('-');

            TodayDate = new DateTime(int.Parse(todaySplits[0]), int.Parse(todaySplits[1]), int.Parse(todaySplits[2]));
            IsSign = element.GetProperty(DailyReward.IsSign).GetBoolean();

            try
            {
                IsFirstBind = element.GetProperty(DailyReward.FirstBind).GetBoolean();
            }
            catch
            {
                IsFirstBind = false;
            }

            IsSub = element.GetProperty(DailyReward.IsSub).GetBoolean();
            Region = element.GetProperty(DailyReward.Region).GetString();
        }
    }
}
