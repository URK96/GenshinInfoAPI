using System.Collections.Generic;
using System.Text.Json;

namespace GenshinInfo.Models
{
    public class GachaDataInfos : BaseData
    {
        public int Page { get; }
        public int Size { get; }
        public int Total { get; }
        public string Region { get; }
        public List<GachaDataInfo> GachaDatas { get; } = new();

        public GachaDataInfos(JsonElement element)
        {
            Page = int.Parse(element.GetProperty("page").GetString());
            Size = int.Parse(element.GetProperty("size").GetString());
            Total = int.Parse(element.GetProperty("total").GetString());
            Region = element.GetProperty("region").GetString();

            foreach (var gachaElement in element.GetProperty("list").EnumerateArray())
            {
                GachaDatas.Add(new(gachaElement));
            }
        }
    }
}
