using System;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/SvarogBlessing")]
    public class SvarogBlessing: BaseBlessing
    {
        [SerializeField] private int addedArmor;
        public override event Action Completed;
        public override bool CanExecute()
        {
            return Player.MyUnits.Any();
        }

        public override void Execute()
        {
            // TODO анимации
            foreach (var unit in Player.MyUnits)
            {
                unit.CurrentArmor += addedArmor;
            }
            
            Completed?.Invoke();
        }

        protected override object[] GetParams()
        {
            return new object[] {addedArmor};
        }
    }
}