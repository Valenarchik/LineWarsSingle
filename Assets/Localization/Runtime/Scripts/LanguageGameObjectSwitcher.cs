using System;
using UnityEngine;

namespace Localization
{
    public class LanguageGameObjectSwitcher: LanguageObject<GameObject>
    {
        protected override void OnLanguageSwitch()
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
            if (CurrentValue != null)
                CurrentValue.SetActive(true);
        }
    }
}