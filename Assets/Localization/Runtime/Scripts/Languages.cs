using System;
using UnityEngine;

namespace Localization
{
    [Serializable]
    public class Languages
    {
        [Tooltip("RUSSIAN")] public bool ru = true;
        [Tooltip("ENGLISH")] public bool en = true;
        [Tooltip("TURKISH")] public bool tr = true;
        [Tooltip("AZERBAIJANIAN")] public bool az;
        [Tooltip("BELARUSIAN")] public bool be;
        [Tooltip("HEBREW")] public bool he;
        [Tooltip("ARMENIAN")] public bool hy;
        [Tooltip("GEORGIAN")] public bool ka;
        [Tooltip("ESTONIAN")] public bool et;
        [Tooltip("FRENCH")] public bool fr;
        [Tooltip("KAZAKH")] public bool kk;
        [Tooltip("KYRGYZ")] public bool ky;
        [Tooltip("LITHUANIAN")] public bool lt;
        [Tooltip("LATVIAN")] public bool lv;
        [Tooltip("ROMANIAN")] public bool ro;
        [Tooltip("TAJICK")] public bool tg;
        [Tooltip("TURKMEN")] public bool tk;
        [Tooltip("UKRAINIAN")] public bool uk;
        [Tooltip("UZBEK")] public bool uz;
        [Tooltip("SPANISH")] public bool es;
        [Tooltip("PORTUGUESE")] public bool pt;
        [Tooltip("ARABIAN")] public bool ar;
        [Tooltip("INDONESIAN")] public bool id;
        [Tooltip("JAPANESE")] public bool ja;
        [Tooltip("ITALIAN")] public bool it;
        [Tooltip("GERMAN")] public bool de;
        [Tooltip("HINDI")] public bool hi;

        public const int COUNT = 27;
        public readonly int Count = COUNT;
        public bool[] Values => new[]
        {
            ru, en, tr, az, be, he, hy, ka, et, fr, kk, ky, lt, lv, ro, tg, tk, uk, uz, es, pt, ar, id, ja, it, de, hi
        };
    }
}