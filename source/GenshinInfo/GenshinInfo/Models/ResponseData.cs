﻿using GenshinInfo.Constants.Indexes;
using GenshinInfo.Enums;

using System.Text.Json;

namespace GenshinInfo.Models
{
    /// <summary>
    /// Basic response data of miHoYo server API
    /// </summary>
    public class ResponseData : BaseData
    {
        public int RetCode { get; private set; }
        public string Message { get; private set; }

        private ResponseData() { }

        /// <summary>
        /// Create response data with extra data
        /// </summary>
        /// <param name="responseResult">Response result of API request</param>
        /// <param name="jsonStr">Response string (JSON)</param>
        /// <param name="dataType">Extra data type</param>
        /// <returns>Tuple type : (response data object, extra data object)</returns>
        public static (ResponseData, BaseData) CreateData(bool responseResult, string jsonStr, DataType dataType)
        {
            if (!responseResult ||
                string.IsNullOrWhiteSpace(jsonStr))
            {
                return (null, null);
            }

            JsonElement rootElement;
            JsonElement dataElement;

            try
            {
                rootElement = JsonDocument.Parse(jsonStr).RootElement;
                dataElement = rootElement.GetProperty(Response.Data);
            }
            catch
            {
                return (null, null);
            }

            ResponseData responseData = new();
            BaseData extraData = default;

            responseData.RetCode = rootElement.GetProperty(Response.RetCode).GetInt32();
            responseData.Message = rootElement.GetProperty(Response.Message).GetString();

            if (dataElement.ValueKind is not JsonValueKind.Null)
            {
                extraData = dataType switch
                {
                    DataType.RTNote => new RTNoteData(dataElement),
                    DataType.GameRecordCard => CreateGenshinRecordCardData(dataElement),
                    DataType.GachaData => new GachaDataInfos(dataElement),
                    DataType.DailyRewardList => new DailyRewardListData(dataElement),
                    DataType.DailyRewardStatus => new DailyRewardStatusData(dataElement),
                    DataType.DailyRewardSingInResult => DailyRewardSignInResultData.CreateData(dataElement),

                    _ => default
                };
            }

            return (responseData, extraData);
        }

        private static GameRecordCardData CreateGenshinRecordCardData(JsonElement cardElements)
        {
            JsonElement genshinCardElement = default;

            foreach (var cardElement in cardElements.GetProperty("list").EnumerateArray())
            {
                if (cardElement.GetProperty(GameRecordCard.GameId).GetInt32() is 2)
                {
                    genshinCardElement = cardElement;

                    break;
                }
            }

            return (genshinCardElement.ValueKind is not JsonValueKind.Undefined) ?
                new GameRecordCardData(genshinCardElement) :
                null;
        }
    }
}
