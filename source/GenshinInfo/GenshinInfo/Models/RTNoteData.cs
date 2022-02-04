using GenshinInfo.Constants.Indexes;

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace GenshinInfo.Models
{
    /// <summary>
    /// Real-Time Note data
    /// </summary>
    public class RTNoteData : BaseData
    {
        public int CurrentResin { get; }
        public int MaxResin { get; }
        public TimeSpan ResinRecoveryTime { get; }

        public int FinishedTaskNum { get; }
        public int TotalTaskNum { get; }
        public bool IsExtraTaskRewardReceived { get; }

        public int RemainResinDiscountNum { get; }
        public int ResinDiscountNumLimit { get; }

        public int CurrentExpeditionNum { get; }
        public int MaxExpeditionNum { get; }
        public List<ExpeditionDetail> Expeditions { get; }

        public int CurrentHomeCoin { get; }
        public int MaxHomeCoin { get; }
        public TimeSpan HomeCoinRecoveryTime { get; }

        public RTNoteData(JsonElement element)
        {
            // Resin Info
            CurrentResin = element.GetProperty(RTNote.CurrentResin).GetInt32();
            MaxResin = element.GetProperty(RTNote.MaxResin).GetInt32();
            ResinRecoveryTime = Utils.ConvertRemainTime(
                element.GetProperty(RTNote.ResinRecoveryTime).GetString());

            // Task Info
            FinishedTaskNum = element.GetProperty(RTNote.FinishedTaskNum).GetInt32();
            TotalTaskNum = element.GetProperty(RTNote.TotalTaskNum).GetInt32();
            IsExtraTaskRewardReceived = element.GetProperty(RTNote.IsExtraTaskRewardReceived).GetBoolean();

            // Resin Discount Info
            RemainResinDiscountNum = element.GetProperty(RTNote.RemainResinDiscountNum).GetInt32();
            ResinDiscountNumLimit = element.GetProperty(RTNote.ResinDiscountNumLimit).GetInt32();

            // Expedition Info
            CurrentExpeditionNum = element.GetProperty(RTNote.CurrentExpeditionNum).GetInt32();
            MaxExpeditionNum = element.GetProperty(RTNote.MaxExpeditionNum).GetInt32();

            Expeditions = new();

            foreach (var expElement in element.GetProperty(RTNote.Expeditions).EnumerateArray())
            {
                Expeditions.Add(new(expElement));
            }

            // Realm Home Coin Info
            CurrentHomeCoin = element.GetProperty(RTNote.CurrentRealmHomeCoin).GetInt32();
            MaxHomeCoin = element.GetProperty(RTNote.MaxRealmHomeCoin).GetInt32();
            HomeCoinRecoveryTime = Utils.ConvertRemainTime(
                element.GetProperty(RTNote.RealmHomeCoinRecoveryTime).GetString());
        }
    }
}
