using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/RandomBlessing")]
    public class RandomBlessing: BaseBlessing
    {
        [SerializeField] private BlessingStorage storage; 
        public override event Action Completed;
        
        private BaseBlessing currentBlessing;
        
        public override bool CanExecute()
        {
            return currentBlessing == null 
                   && storage.Values
                       .Where(x => x != this)
                       .Any(x => x.CanExecute());
        }

        public override void Execute()
        {
            var allBlessings = storage.Values
                .Where(x => x != null && x != this && x.CanExecute())
                .ToArray();
            
            var rnd = Random.Range(0, allBlessings.Length);
            currentBlessing = allBlessings[rnd];
            if (currentBlessing.CanExecute())
            {
                currentBlessing.Completed += BlissingOnCompleted;
                currentBlessing.Execute();
            }
        }
        
        private void BlissingOnCompleted()
        {
            currentBlessing.Completed -= BlissingOnCompleted;
            currentBlessing = null;
            Completed?.Invoke();
        }

        protected override object[] GetParams()
        {
            return Array.Empty<object>();
        }
    }
}