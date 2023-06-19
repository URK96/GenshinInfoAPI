using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenshinInfo.Models
{
    public class DailyRewardSignInResultData : BaseData
    {
        [JsonPropertyName("code")]
        public string Code { get; init; }

        [JsonPropertyName("first_bind")]
        public bool IsFirstBind { get; init; }

        [JsonPropertyName("gt_result")]
        public GtResultData GtResult { get; init; }

        public static DailyRewardSignInResultData CreateData(JsonElement element)
        {
            DailyRewardSignInResultData data = null;

            try
            {
                data = JsonSerializer.Deserialize<DailyRewardSignInResultData>(element);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return data;
        }
    }
}
