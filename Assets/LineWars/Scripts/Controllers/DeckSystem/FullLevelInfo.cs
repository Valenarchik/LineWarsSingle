using UnityEngine;

namespace LineWars.Model
{
    public class FullLevelInfo
    {
        public Unit Unit { get; }
        public int Cost {get;}
        public UnitCost CostProgression { get; }
        public int CostToUpgrade { get; }

        public FullLevelInfo(
            Unit unit, 
            int cost,
            UnitCost costProgression,
            int costToUpgrade)
        {
            Unit = unit;
            Cost = cost;
            CostProgression = costProgression;
            CostToUpgrade = costToUpgrade;
        }

        public FullLevelInfo WithAdditionalInfo(AdditionalLevelInfo additionalLevelInfo)
        {
            var unit = additionalLevelInfo.OverrideUnitPrefab.Enabled ? 
                additionalLevelInfo.OverrideUnitPrefab.Value : Unit;
            var cost = additionalLevelInfo.OverrideCostInFight.Enabled ? 
                additionalLevelInfo.OverrideCostInFight.Value : Cost;
            var costProgression = additionalLevelInfo.OverrideCostProgression.Enabled ?
                additionalLevelInfo.OverrideCostProgression.Value : CostProgression;
            
            return new FullLevelInfo(
                unit,
                cost,
                costProgression,
                additionalLevelInfo.CostToUpgrade);

        }
    }
}
