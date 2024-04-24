using System;
using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    public abstract class LanguageObject<T> : MonoBehaviour
    {
        public LanguageField<T> Value;
        public T CurrentValue => Value.CurrentValue;
        public T[] Values => Value.Values;
        

        protected virtual void OnEnable()
        {
            LocalizationManager.SwitchLangEvent += LocalizationManagerOnSwitchLangEvent;
        }

        protected virtual void OnDisable()
        {
            LocalizationManager.SwitchLangEvent -= LocalizationManagerOnSwitchLangEvent;
        }

        private void LocalizationManagerOnSwitchLangEvent()
        {
            OnLanguageSwitch(LocalizationManager.LangName);
        }
        
        protected abstract void OnLanguageSwitch(LangName langName);
    }
}