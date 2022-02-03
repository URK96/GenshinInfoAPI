using GenshinInfo.Constants.Indexes;

using System.Text.Json;

namespace GenshinInfo.Models
{
    public class DataSwitchInfo
    {
        public bool IsPublic { get; }
        public int SwitchId { get; }
        public string SwitchName { get; }

        public DataSwitchInfo(JsonElement element)
        {
            IsPublic = element.GetProperty(DataSwitch.IsPublic).GetBoolean();
            SwitchId = element.GetProperty(DataSwitch.SwitchId).GetInt32();
            SwitchName = element.GetProperty(DataSwitch.SwitchName).GetString();
        }
    }
}
