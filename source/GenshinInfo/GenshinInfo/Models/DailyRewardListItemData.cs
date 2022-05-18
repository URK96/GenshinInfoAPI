namespace GenshinInfo.Models
{
    public record DailyRewardListItemData
    {
        public string IconUrl { get; init; }
        public string ItemName { get; init; }
        public int ItemCount { get; init; }
    }
}
