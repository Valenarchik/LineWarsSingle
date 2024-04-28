using System;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/SlowdownBlessing")]
    public class SlowdownBlessing: BaseBlessing
    {
        [SerializeField] private int removedActionPoints;
        
        public override event Action Completed;
        public override bool CanExecute()
        {
            return Player.GetAllEnemiesUnits().Any();
        }

        public override void Execute()
        {
            foreach (var unit in Player.GetAllEnemiesUnits())
                unit.CurrentActionPoints -= removedActionPoints;
            Completed?.Invoke();
        }

        protected override object[] GetParams()
        {
            return new object[] {removedActionPoints};
        }
    }
}