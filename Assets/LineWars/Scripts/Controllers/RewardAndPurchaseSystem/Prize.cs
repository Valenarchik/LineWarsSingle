using System;
using Localization;
using UnityEngine;

namespace LineWars.Model
{
    [System.Serializable]
    public class Prize
    {
        [SerializeField] private PrizeType type;
        [SerializeField] private int amount;

        public PrizeType Type => type;
        public int Amount => amount;

        public Prize(PrizeType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }

        public override string ToString()
        {
            return $"<font=\"New Rocker\">{amount}</font> {type.ToString(amount)}";
        }
        
        public string ToString(LangName langName)
        {
            return $"<font=\"New Rocker\">{amount}</font> {type.ToString(amount, langName)}";
        }
    }
}