using System;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/StribogBlessing")]
    public class StribogBlessing: BaseBlessing
    {
        [Tooltip("Ход - это вся последовательность действий до передачи управления ии")]
        [SerializeField] private int turnsCount; 
        
        private MonoGraph Graph => MonoGraph.Instance;
        public override event Action Completed;
        public override bool CanExecute()
        {
            return true;
        }

        public override void Execute()
        {
            if (turnsCount == 0)
                return;
            
            foreach (var node in Graph.Nodes)
                Player.SetAdditionalVisibleNodeForTurn(node, turnsCount);
            Player.RecalculateVisibility();
            Completed?.Invoke();
        }

        protected override object[] GetParams()
        {
            return new object[] {turnsCount};
        }
    }
}