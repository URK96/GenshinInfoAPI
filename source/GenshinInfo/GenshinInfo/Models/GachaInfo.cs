using Newtonsoft.Json.Linq;

using System;

namespace GenshinInfo.Models
{
    public class GachaInfo
    {
        public string Uid { get; private set; }
        public string GachaType { get; private set; }
        public int ItemId { get; private set; }
        public int Count { get; private set; }
        public DateTime GachaTime { get; private set; }
        public string ItemName { get; private set; }
        public string ItemType { get; private set; }
        public string Id { get; private set; }
        public string LangCode { get; private set; }
        public int ItemRank { get; private set; }

        public string GachaShortDateTime => GachaTime.ToString("yyyy,MM,dd,HH,mm,ss");

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
