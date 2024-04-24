using System;
using UnityEngine;

namespace Localization
{
    public class LanguageGameObjectSwitcher: LanguageObject<GameObject>
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            SwitchObjects();
        }

        protected override void OnLanguageSwitch(LangName langName)
        {
            SwitchObjects();
        }

        private void SwitchObjects()
        {
            foreach (var o in Values)
            {
                if (o != null)
                    o.SetActive(false);
            }
            CurrentValue.SetActive(true);
        }
    }
}