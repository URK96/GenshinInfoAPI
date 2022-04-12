using GenshinInfo.Constants.Indexes;

using System.Collections.Generic;
using System.Text.Json;

namespace GenshinInfo.Models
{
    /// <summary>
    /// Game record card data of HoYoLAB
    /// </summary>
    public class GameRecordCardData : BaseData
    {
        public string BackgroundImage { get; }
        public List<Data> Datas { get; } = new();
        public List<DataSwitchInfo> DataSwitches { get; } = new();
        public int GameId { get; }
        public string GameRoleId { get; }
        public object H5DataSwitches { get; }
        public bool HasRole { get; }
        public bool IsPublic { get; }
        public int Level { get; }
        public string Nickname { get; }
        public string Region { get; }
        public string RegionName { get; }
        public string GameRecordUrl { get; }

        public GameRecordCardData(JsonElement element)
        {
            BackgroundImage = element.GetProperty(GameRecordCard.BackgroundImage).GetString();
            GameId = element.GetProperty(GameRecordCard.GameId).GetInt32();
            GameRoleId = element.GetProperty(GameRecordCard.GameRoleId).GetString();
            HasRole = element.GetProperty(GameRecordCard.HasRole).GetBoolean();
            IsPublic = element.GetProperty(GameRecordCard.IsPublic).GetBoolean();
            Level = element.GetProperty(GameRecordCard.Level).GetInt32();
            Nickname = element.GetProperty(GameRecordCard.Nickname).GetString();
            Region = element.GetProperty(GameRecordCard.Region).GetString();
            RegionName = element.GetProperty(GameRecordCard.RegionName).GetString();
            GameRecordUrl = element.GetProperty(GameRecordCard.Url).GetString();

            foreach (var dataElement in element.GetProperty(GameRecordCard.Data).EnumerateArray())
            {
                Datas.Add(new(dataElement));
            }

            foreach (var switchElement in element.GetProperty(GameRecordCard.DataSwitches).EnumerateArray())
            {
                DataSwitches.Add(new(switchElement));
            }
        }

        public class Data
        {
            public string Name { get; }
            public int Type { get; }
            public string Value { get; }

            public Data(JsonElement element)
            {
                Name = element.GetProperty("name").GetString();
                Type = element.GetProperty("type").GetInt32();
                Value = element.GetProperty("value").GetString();
            }
        }
    }
}
