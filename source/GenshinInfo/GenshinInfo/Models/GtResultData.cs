using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GenshinInfo.Models
{
    public record GtResultData
    {
        [JsonPropertyName("risk_code")]
        public int RiskCode { get; init; }

        [JsonPropertyName("gt")]
        public string GtData { get; init; }

        [JsonPropertyName("challenge")]
        public string ChallengeData { get; init; }

        [JsonPropertyName("success")]
        public int SuccessCode { get; init; }

        public bool IsSuccess => SuccessCode is 1;

        [JsonPropertyName("is_risk")]
        public bool IsRisk { get; init; }
    }
}
