using GenshinInfo.Constants.Indexes;
using GenshinInfo.Enums;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace GenshinInfo.Models
{
    public class ResponseData : BaseData
    {
        public int RetCode { get; private set; }
        public string Message { get; private set; }

        private ResponseData() { }

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
