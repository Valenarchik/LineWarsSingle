using System;
using System.Linq;
using UnityEngine;
using Object = System.Object;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/AccelerationBlessing")]
    public class AccelerationBlessing: BaseBlessing
    {
        [SerializeField] private int addedActionPoints;
        
        public override event Action Completed;
        public override bool CanExecute()
        {
            return Player.MyUnits.Any();
        }

        public override void Execute()
        {
            foreach (var unit in Player.MyUnits)
                unit.CurrentActionPoints += addedActionPoints;
            Completed?.Invoke();
        }

        protected override object[] GetParams()
        {
            return new object[] {addedActionPoints};
        }
    }
}