using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/PerunBlessing")]
    public class PerunBlessing: BaseBlessing
    {
        [SerializeField] private int damage;
        public override event Action Completed;
        
        public override bool CanExecute()
        {
            return Player.GetAllEnemiesUnits().Any();
        }

        public override void Execute()
        {
            // TODO анимации
            foreach (var enemy in Player.GetAllEnemiesUnits())
            {
                enemy.DealDamageThroughArmor(damage);
            }

            Completed?.Invoke();
        }

        protected override object[] GetParams()
        {
            return new object[] {damage};
        }
    }
}