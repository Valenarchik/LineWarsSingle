using System;
using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    public abstract class LanguageObject<T> : MonoBehaviour
    {
        public LanguageField<T> Value;
        protected T CurrentValue => Value.CurrentValue;
        protected T[] Values => Value.Values;
        

        protected virtual void OnEnable()
        {
            LocalizationManager.SwitchLangEvent += LocalizationManagerOnSwitchLangEvent;
            OnLanguageSwitch();
        }

        protected virtual void OnDisable()
        {
            LocalizationManager.SwitchLangEvent -= LocalizationManagerOnSwitchLangEvent;
        }

        private void LocalizationManagerOnSwitchLangEvent()
        {
            OnLanguageSwitch();
        }
        
        protected abstract void OnLanguageSwitch();
    }
}