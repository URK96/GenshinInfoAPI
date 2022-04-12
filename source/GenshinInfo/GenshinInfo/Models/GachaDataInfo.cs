using GenshinInfo.Constants.Indexes;

using System;
using System.Text.Json;

namespace GenshinInfo.Models
{
    /// <summary>
    /// Response Data of Gacha Log
    /// </summary>
    public class GachaDataInfo
    {
        public string Uid { get; set; }
        public string GachaType { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; }
        public DateTime GachaTime { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string Id { get; set; }
        public string LangCode { get; set; }
        public int ItemRank { get; set; }

        public string GachaShortDateTime => GachaTime.ToString("yyyy,MM,dd,HH,mm,ss");

        public GachaDataInfo() { }

        public GachaDataInfo(JsonElement element)
        {
            Uid = element.GetProperty(GachaData.Uid).GetString();
            GachaType = element.GetProperty(GachaData.GachaType).GetString();
            ItemId = 0;
            Count = int.Parse(element.GetProperty(GachaData.Count).GetString());
            ItemName = element.GetProperty(GachaData.Name).GetString();
            ItemType = element.GetProperty(GachaData.ItemType).GetString();
            Id = element.GetProperty(GachaData.Id).GetString();
            LangCode = element.GetProperty(GachaData.Lang).GetString();
            ItemRank = int.Parse(element.GetProperty(GachaData.RankType).GetString());
            GachaTime = ExtractGachaTime(element.GetProperty(GachaData.Time).GetString());
        }

        private DateTime ExtractGachaTime(string timeStr)
        {
            string[] temp = timeStr.Split(' ');
            string[] dateSplit = temp[0].Split('-');
            string[] timeSplit = temp[1].Split(':');

            return new DateTime(
                int.Parse(dateSplit[0]),
                int.Parse(dateSplit[1]),
                int.Parse(dateSplit[2]),
                int.Parse(timeSplit[0]),
                int.Parse(timeSplit[1]),
                int.Parse(timeSplit[2]));
        }
    }
}
