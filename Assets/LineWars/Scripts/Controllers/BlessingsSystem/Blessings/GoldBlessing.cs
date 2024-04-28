using System;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/GoldBlessing")]
    public class GoldBlessing: BaseBlessing
    {
        [SerializeField] private int addedGolds;
        
        public override event Action Completed;
        public override bool CanExecute()
        {
            return true;
        }

        public override void Execute()
        {
            Player.CurrentMoney += addedGolds;
            Completed?.Invoke();
        }
        
        protected override object[] GetParams()
        {
            return new object[] {addedGolds};
        }
    }
}