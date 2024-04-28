using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/SpawnBlessing")]
    public class SpawnBlessing: BaseBlessing
    {
        [SerializeField] private List<Unit> unitPrefabsToSpawn;
        
        public override event Action Completed;
        public override bool CanExecute()
        {
            return Player.InitialSpawns
                .Any(node => node.Owner == Player.LocalPlayer 
                             && unitPrefabsToSpawn.Any(node.UnitIsFit));
        }

        public override void Execute()
        {
            var tempList = unitPrefabsToSpawn.ToList();
            
            foreach (var node in Player.InitialSpawns)
            {
                if (node.Owner != Player.LocalPlayer)
                    continue;
                
                var unitsToSpawn = tempList
                    .Where(unit => node.UnitIsFit(unit))
                    .ToArray();

                foreach (var unit in unitsToSpawn)
                {
                    Player.SpawnUnit(node, unit);
                    tempList.Remove(unit);
                    if (!node.AnyIsFree)
                        break;
                }
            }
            
            Completed?.Invoke();
        }

        protected override object[] GetParams()
        {
            var units = string.Join(
                ", ", 
                unitPrefabsToSpawn
                    .GroupBy(x => x.UnitName)
                    .Select(x =>
                    {
                        var count = x.Count();
                        if (count > 1)
                        {
                            return  $"юнита \"{x.Key}\" х{count}";
                        }
                        return $"юнита \"{x.Key}\"";
                    })
            );
            
            return new object[] {units};
        }
    }
}