using Newtonsoft.Json.Linq;

using System;

namespace GenshinInfo.Models
{
    public class GachaInfo
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

        public GachaInfo() { }

        public GachaInfo(JObject obj)
        {
            Uid = obj["uid"].Value<string>();
            GachaType = obj["gacha_type"].Value<string>();
            ItemId = 0;
            Count = obj["count"].Value<int>();
            ItemName = obj["name"].Value<string>();
            ItemType = obj["item_type"].Value<string>();
            Id = obj["id"].Value<string>();
            LangCode = obj["lang"].Value<string>();
            ItemRank = obj["rank_type"].Value<int>();

            GachaTime = ExtractGachaTime(obj["time"].Value<string>());
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
