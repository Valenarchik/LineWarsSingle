using System;
using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    public abstract class LanguageObject<T> : MonoBehaviour, ISerializationCallbackReceiver
    {
        [HideInInspector] public T ru,
            en,
            tr,
            az,
            be,
            he,
            hy,
            ka,
            et,
            fr,
            kk,
            ky,
            lt,
            lv,
            ro,
            tg,
            tk,
            uk,
            uz,
            es,
            pt,
            ar,
            id,
            ja,
            it,
            de,
            hi;

        private Dictionary<LangName, Func<T>> getDictionary;
        private Dictionary<LangName, Action<T>> setDictionary;
        public LanguageObject<T> Value => this;
        public T CurrentValue => Value[LocalizationManager.LangName];
        
        public T this[LangName lang]
        {
            get => getDictionary[lang].Invoke();
            set => setDictionary[lang].Invoke(value);
        }

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
        
        public virtual void OnBeforeSerialize()
        {
        }

        public virtual void OnAfterDeserialize()
        {
            getDictionary = new Dictionary<LangName, Func<T>>
            {
                {LangName.ru, () => ru},
                {LangName.en, () => en},
                {LangName.tr, () => tr},
                {LangName.az, () => az},
                {LangName.be, () => be},
                {LangName.he, () => he},
                {LangName.hy, () => hy},
                {LangName.ka, () => ka},
                {LangName.et, () => et},
                {LangName.fr, () => fr},
                {LangName.kk, () => kk},
                {LangName.ky, () => ky},
                {LangName.lt, () => lt},
                {LangName.lv, () => lv},
                {LangName.ro, () => ro},
                {LangName.tg, () => tg},
                {LangName.tk, () => tk},
                {LangName.uk, () => uk},
                {LangName.uz, () => uz},
                {LangName.es, () => es},
                {LangName.pt, () => pt},
                {LangName.ar, () => ar},
                {LangName.id, () => id},
                {LangName.ja, () => ja},
                {LangName.it, () => it},
                {LangName.de, () => de},
                {LangName.hi, () => hi}
            };

            setDictionary = new Dictionary<LangName, Action<T>>()
            {
                {LangName.ru, (value) => ru = value},
                {LangName.en, (value) => en = value},
                {LangName.tr, (value) => tr = value},
                {LangName.az, (value) => az = value},
                {LangName.be, (value) => be = value},
                {LangName.he, (value) => he = value},
                {LangName.hy, (value) => hy = value},
                {LangName.ka, (value) => ka = value},
                {LangName.et, (value) => et = value},
                {LangName.fr, (value) => fr = value},
                {LangName.kk, (value) => kk = value},
                {LangName.ky, (value) => ky = value},
                {LangName.lt, (value) => lt = value},
                {LangName.lv, (value) => lv = value},
                {LangName.ro, (value) => ro = value},
                {LangName.tg, (value) => tg = value},
                {LangName.tk, (value) => tk = value},
                {LangName.uk, (value) => uk = value},
                {LangName.uz, (value) => uz = value},
                {LangName.es, (value) => es = value},
                {LangName.pt, (value) => pt = value},
                {LangName.ar, (value) => ar = value},
                {LangName.id, (value) => id = value},
                {LangName.ja, (value) => ja = value},
                {LangName.it, (value) => it = value},
                {LangName.de, (value) => de = value},
                {LangName.hi, (value) => hi = value}
            };
        }
    }
}