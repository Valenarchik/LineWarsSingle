using System;
using UnityEngine;

namespace LineWars.Model
{
    //по-идее, можно было реализовать как и экшоны, но в данном случае это лишнее
    //не использовать состояния! это может плохо сказаться на игре
    public abstract class BaseBlessing: ScriptableObject
    {
        [SerializeField] private BlessingId blessingId;
        
        [SerializeField] private Optional<string> overrideName;
        [SerializeField] private Optional<string> overrideDescription;
        [SerializeField] private Sprite icon;
        
        protected Player Player => Player.LocalPlayer;
        public abstract event Action Completed;
        public abstract bool CanExecute();
        public abstract void Execute();

        protected virtual string DefaultName => DrawHelper.GetBlessingTypeName(blessingId.BlessingType);
        protected abstract string DefaultDescription { get; }

        public BlessingId BlessingId => blessingId;
        public string Name => overrideName.Enabled ? overrideName.Value : DefaultName;
        public string Description => overrideDescription.Enabled ? overrideDescription.Value : DefaultDescription;
        public Sprite Icon => icon;
    }
}