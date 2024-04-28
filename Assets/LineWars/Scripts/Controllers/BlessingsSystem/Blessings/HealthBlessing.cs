using System;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/HealthBlessing")]
    public class HealthBlessing: BaseBlessing
    {
        [SerializeField] private int addedHealth;
        
        public override event Action Completed;
        public override bool CanExecute()
        {
            return Player.MyUnits.Any();
        }

        public override void Execute()
        {
            // TODO анимации
            foreach (var unit in Player.MyUnits)
                unit.CurrentHp += addedHealth;
            Completed?.Invoke();
        }

        protected override object[] GetParams()
        {
            return new object[] {addedHealth};
        }
    }
}