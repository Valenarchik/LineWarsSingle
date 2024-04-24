using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    public class GameObjectLocalizationSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject ru, en, tr, az, be, he, hy, ka, et, fr, kk, ky, lt, lv, ro, tg, tk, uk, uz, es, pt, ar, id, ja, it, de, hi;
            
        private LocalizationSettings settings => LocalizationManager.Settings;
        private void OnEnable()
        {
            SwitchButton();
            LocalizationManager.SwitchLangEvent += SwitchButton;
        }

        private void OnDisable() => LocalizationManager.SwitchLangEvent -= SwitchButton;
        
        private void SwitchButton()
        {
            SwitchButton(LocalizationManager.lang);
        }

        private void SwitchButton(string lang)
        {
            if (!settings.LocalizationEnable)
                return;
            for (var i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null)
                {
                    if (lang == LangMethods.LangName(i))
                    {
                        buttons[i].SetActive(true);
                    }
                    else
                        buttons[i].SetActive(false);
                }
            }
        }
        
        public GameObject[] buttons
        {
            get
            {
                var s = new GameObject[27];

                s[0] = ru;
                s[1] = en;
                s[2] = tr;
                s[3] = az;
                s[4] = be;
                s[5] = he;
                s[6] = hy;
                s[7] = ka;
                s[8] = et;
                s[9] = fr;
                s[10] = kk;
                s[11] = ky;
                s[12] = lt;
                s[13] = lv;
                s[14] = ro;
                s[15] = tg;
                s[16] = tk;
                s[17] = uk;
                s[18] = uz;
                s[19] = es;
                s[20] = pt;
                s[21] = ar;
                s[22] = id;
                s[23] = ja;
                s[24] = it;
                s[25] = de;
                s[26] = hi;

                return s;
            }
            set
            {
                ru = value[0];
                en = value[1];
                tr = value[2];
                az = value[3];
                be = value[4];
                he = value[5];
                hy = value[6];
                ka = value[7];
                et = value[8];
                fr = value[9];
                kk = value[10];
                ky = value[11];
                lt = value[12];
                lv = value[13];
                ro = value[14];
                tg = value[15];
                tk = value[16];
                uk = value[17];
                uz = value[18];
                es = value[19];
                pt = value[20];
                ar = value[21];
                id = value[22];
                ja = value[23];
                it = value[24];
                de = value[25];
                hi = value[26];
            }
        }
    }
}
