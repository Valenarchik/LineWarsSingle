using System;
using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    [Serializable]
    public class LanguageField<T>
    {
        [SerializeField] private string identity;
        [SerializeField] private float textHeight = 20;
        [SerializeField] private T ru, en, tr, az, be, he, hy, ka, et, fr, kk, ky, lt, lv, ro, tg, tk, uk, uz, es, pt, ar, id, ja, it, de, hi;

        public T this[LangName langName]
        {
            get
            {
                return langName switch
                {
                    LangName.ru => ru,
                    LangName.en => en,
                    LangName.tr => tr,
                    LangName.az => az,
                    LangName.be => be,
                    LangName.he => he,
                    LangName.hy => hy,
                    LangName.ka => ka,
                    LangName.et => et,
                    LangName.fr => fr,
                    LangName.kk => kk,
                    LangName.ky => ky,
                    LangName.lt => lt,
                    LangName.lv => lv,
                    LangName.ro => ro,
                    LangName.tg => tg,
                    LangName.tk => tk,
                    LangName.uk => uk,
                    LangName.uz => uz,
                    LangName.es => es,
                    LangName.pt => pt,
                    LangName.ar => ar,
                    LangName.id => id,
                    LangName.ja => ja,
                    LangName.it => it,
                    LangName.de => de,
                    LangName.hi => hi,
                    _ => throw new ArgumentOutOfRangeException(nameof(langName), langName, null)
                };
            }
            set
            {
                switch (langName)
                {
                    case LangName.ru:
                        ru = value;
                        break;
                    case LangName.en:
                        en = value;
                        break;
                    case LangName.tr:
                        tr = value;
                        break;
                    case LangName.az:
                        az = value;
                        break;
                    case LangName.be:
                        be = value;
                        break;
                    case LangName.he:
                        he = value;
                        break;
                    case LangName.hy:
                        hy = value;
                        break;
                    case LangName.ka:
                        ka = value;
                        break;
                    case LangName.et:
                        et = value;
                        break;
                    case LangName.fr:
                        fr = value;
                        break;
                    case LangName.kk:
                        kk = value;
                        break;
                    case LangName.ky:
                        ky = value;
                        break;
                    case LangName.lt:
                        lt = value;
                        break;
                    case LangName.lv:
                        lv = value;
                        break;
                    case LangName.ro:
                        ro = value;
                        break;
                    case LangName.tg:
                        tg = value;
                        break;
                    case LangName.tk:
                        tk = value;
                        break;
                    case LangName.uk:
                        uk = value;
                        break;
                    case LangName.uz:
                        uz = value;
                        break;
                    case LangName.es:
                        es = value;
                        break;
                    case LangName.pt:
                        pt = value;
                        break;
                    case LangName.ar:
                        ar = value;
                        break;
                    case LangName.id:
                        id = value;
                        break;
                    case LangName.ja:
                        ja = value;
                        break;
                    case LangName.it:
                        it = value;
                        break;
                    case LangName.de:
                        de = value;
                        break;
                    case LangName.hi:
                        hi = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(langName), langName, null);
                }
            }
        }
        public T CurrentValue
        {
            get => this[LocalizationManager.LangName];
            set => this[LocalizationManager.LangName] = value;
        }

        public T[] Values => new[]
        {
            ru, en, tr, az, be, he, hy, ka, et, fr, kk, ky, lt, lv, ro, tg, tk, uk, uz, es, pt, ar, id, ja, it, de, hi
        };

        public static implicit operator T(LanguageField<T> value)
        {
            return value.CurrentValue;
        }
    }
}