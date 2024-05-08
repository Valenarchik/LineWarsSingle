using System;
using UnityEngine;

namespace Localization
{
    [Serializable]
    public class FontsSizeCorrect
    {
        [Tooltip("RUSSIAN")] public int[] ru;
        [Tooltip("ENGLISH")] public int[] en;
        [Tooltip("TURKISH")] public int[] tr;
        [Tooltip("AZERBAIJANIAN")] public int[] az;
        [Tooltip("BELARUSIAN")] public int[] be;
        [Tooltip("HEBREW")] public int[] he;
        [Tooltip("ARMENIAN")] public int[] hy;
        [Tooltip("GEORGIAN")] public int[] ka;
        [Tooltip("ESTONIAN")] public int[] et;
        [Tooltip("FRENCH")] public int[] fr;
        [Tooltip("KAZAKH")] public int[] kk;
        [Tooltip("KYRGYZ")] public int[] ky;
        [Tooltip("LITHUANIAN")] public int[] lt;
        [Tooltip("LATVIAN")] public int[] lv;
        [Tooltip("ROMANIAN")] public int[] ro;
        [Tooltip("TAJICK")] public int[] tg;
        [Tooltip("TURKMEN")] public int[] tk;
        [Tooltip("UKRAINIAN")] public int[] uk;
        [Tooltip("UZBEK")] public int[] uz;
        [Tooltip("SPANISH")] public int[] es;
        [Tooltip("PORTUGUESE")] public int[] pt;
        [Tooltip("ARABIAN")] public int[] ar;
        [Tooltip("INDONESIAN")] public int[] id;
        [Tooltip("JAPANESE")] public int[] ja;
        [Tooltip("ITALIAN")] public int[] it;
        [Tooltip("GERMAN")] public int[] de;
        [Tooltip("HINDI")] public int[] hi;
        
        public int[][] Values => new[]
        {
            ru, en, tr, az, be, he, hy, ka, et, fr, kk, ky, lt, lv, ro, tg, tk, uk, uz, es, pt, ar, id, ja, it, de, hi
        };
    }
}