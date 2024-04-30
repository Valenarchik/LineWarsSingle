using System;
using Localization;

namespace LineWars.Model
{
    public enum PrizeType
    {
        Gold,
        Diamonds,
        UpgradeCards
    }

    public static class PrizeTypeHelper
    {
        public static CostType ToCostType(this PrizeType prizeType)
        {
            return prizeType switch
            {
                PrizeType.Gold => CostType.Gold,
                PrizeType.Diamonds => CostType.Diamond,
                _ => throw new ArgumentOutOfRangeException(nameof(prizeType), prizeType, null)
            };
        }

        public static string ToString(this PrizeType prizeType, int amount, LangName langName = LangName.ru)
        {
            return langName switch
            {
                LangName.ru => prizeType switch
                {
                    PrizeType.Gold => amount.Pluralize("монета", "монеты", "монет"),
                    PrizeType.Diamonds => amount.Pluralize("кристалл", "кристалла", "кристаллов"),
                    PrizeType.UpgradeCards => amount.Pluralize("карта улучшений", "карты улучшений", "карт улучшений"),
                    _ => throw new ArgumentOutOfRangeException()
                },
                LangName.en => prizeType switch
                {
                    PrizeType.Gold => "coins",
                    PrizeType.Diamonds => "diamonds",
                    PrizeType.UpgradeCards => "upgrade cards",
                    _ => throw new ArgumentOutOfRangeException()
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}