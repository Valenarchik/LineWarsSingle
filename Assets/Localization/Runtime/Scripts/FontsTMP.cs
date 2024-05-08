using System;
using TMPro;
using UnityEngine;

namespace Localization
{
    [Serializable]
    public class FontsTMP
    {
        [Tooltip("Стандартный шрифт")] public TMP_FontAsset[] defaultFont;
        [Tooltip("RUSSIAN")] public TMP_FontAsset[] ru;
        [Tooltip("ENGLISH")] public TMP_FontAsset[] en;
        [Tooltip("TURKISH")] public TMP_FontAsset[] tr;
        [Tooltip("AZERBAIJANIAN")] public TMP_FontAsset[] az;
        [Tooltip("BELARUSIAN")] public TMP_FontAsset[] be;
        [Tooltip("HEBREW")] public TMP_FontAsset[] he;
        [Tooltip("ARMENIAN")] public TMP_FontAsset[] hy;
        [Tooltip("GEORGIAN")] public TMP_FontAsset[] ka;
        [Tooltip("ESTONIAN")] public TMP_FontAsset[] et;
        [Tooltip("FRENCH")] public TMP_FontAsset[] fr;
        [Tooltip("KAZAKH")] public TMP_FontAsset[] kk;
        [Tooltip("KYRGYZ")] public TMP_FontAsset[] ky;
        [Tooltip("LITHUANIAN")] public TMP_FontAsset[] lt;
        [Tooltip("LATVIAN")] public TMP_FontAsset[] lv;
        [Tooltip("ROMANIAN")] public TMP_FontAsset[] ro;
        [Tooltip("TAJICK")] public TMP_FontAsset[] tg;
        [Tooltip("TURKMEN")] public TMP_FontAsset[] tk;
        [Tooltip("UKRAINIAN")] public TMP_FontAsset[] uk;
        [Tooltip("UZBEK")] public TMP_FontAsset[] uz;
        [Tooltip("SPANISH")] public TMP_FontAsset[] es;
        [Tooltip("PORTUGUESE")] public TMP_FontAsset[] pt;
        [Tooltip("ARABIAN")] public TMP_FontAsset[] ar;
        [Tooltip("INDONESIAN")] public TMP_FontAsset[] id;
        [Tooltip("JAPANESE")] public TMP_FontAsset[] ja;
        [Tooltip("ITALIAN")] public TMP_FontAsset[] it;
        [Tooltip("GERMAN")] public TMP_FontAsset[] de;
        [Tooltip("HINDI")] public TMP_FontAsset[] hi;
        
        public TMP_FontAsset[][] Values => new[]
        {
            ru, en, tr, az, be, he, hy, ka, et, fr, kk, ky, lt, lv, ro, tg, tk, uk, uz, es, pt, ar, id, ja, it, de, hi
        };
    }
}