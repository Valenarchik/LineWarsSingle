using System;
using Localization;
using UnityEngine;

namespace LineWars.Model
{
    //по-идее, можно было реализовать как и экшоны, но в данном случае это лишнее
    //не использовать состояния! это может плохо сказаться на игре
    public abstract class BaseBlessing: ScriptableObject
    {
        [SerializeField] private BlessingId blessingId;
        
        [SerializeField] private LanguageField<string> blessingName;
        [SerializeField, LanguageTextArea] private LanguageField<string> blessingDescription;
        [SerializeField] private Sprite icon;
        
        protected Player Player => Player.LocalPlayer;
        public abstract event Action Completed;
        public abstract bool CanExecute();
        public abstract void Execute();
        protected abstract object[] GetParams();
        
        public BlessingId BlessingId => blessingId;
        public string Name => blessingName.CurrentValue;
        public string Description => string.Format(blessingDescription.CurrentValue, GetParams());
        public Sprite Icon => icon;
    }
}