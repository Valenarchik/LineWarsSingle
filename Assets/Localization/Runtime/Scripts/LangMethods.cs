using UnityEngine;
using TMPro;

namespace Localization
{
    public class LangMethods
    {
        public static bool[] LangArr(LocalizationSettings inf)
        {
            bool[] b = new bool[27];

            b[0] = inf.languages.ru;
            b[1] = inf.languages.en;
            b[2] = inf.languages.tr;
            b[3] = inf.languages.az;
            b[4] = inf.languages.be;
            b[5] = inf.languages.he;
            b[6] = inf.languages.hy;
            b[7] = inf.languages.ka;
            b[8] = inf.languages.et;
            b[9] = inf.languages.fr;
            b[10] = inf.languages.kk;
            b[11] = inf.languages.ky;
            b[12] = inf.languages.lt;
            b[13] = inf.languages.lv;
            b[14] = inf.languages.ro;
            b[15] = inf.languages.tg;
            b[16] = inf.languages.tk;
            b[17] = inf.languages.uk;
            b[18] = inf.languages.uz;
            b[19] = inf.languages.es;
            b[20] = inf.languages.pt;
            b[21] = inf.languages.ar;
            b[22] = inf.languages.id;
            b[23] = inf.languages.ja;
            b[24] = inf.languages.it;
            b[25] = inf.languages.de;
            b[26] = inf.languages.hi;

            return b;
        }

        public static string LangName(int i)
        {
            return i switch
            {
                0 => "ru",
                1 => "en",
                2 => "tr",
                3 => "az",
                4 => "be",
                5 => "he",
                6 => "hy",
                7 => "ka",
                8 => "et",
                9 => "fr",
                10 => "kk",
                11 => "ky",
                12 => "lt",
                13 => "lv",
                14 => "ro",
                15 => "tg",
                16 => "tk",
                17 => "uk",
                18 => "uz",
                19 => "es",
                20 => "pt",
                21 => "ar",
                22 => "id",
                23 => "ja",
                24 => "it",
                25 => "de",
                26 => "hi",
                _ => null
            };
        }

        public static string LangName(LangName langName)
        {
            return LangName((int) langName);
        }
        
        public static TMP_FontAsset[] GetFontTMP(int i, LocalizationSettings inf)
        {
            return i switch
            {
                0 => inf.fonts.ru,
                1 => inf.fonts.en,
                2 => inf.fonts.tr,
                3 => inf.fonts.az,
                4 => inf.fonts.be,
                5 => inf.fonts.he,
                6 => inf.fonts.hy,
                7 => inf.fonts.ka,
                8 => inf.fonts.et,
                9 => inf.fonts.fr,
                10 => inf.fonts.kk,
                11 => inf.fonts.ky,
                12 => inf.fonts.lt,
                13 => inf.fonts.lv,
                14 => inf.fonts.ro,
                15 => inf.fonts.tg,
                16 => inf.fonts.tk,
                17 => inf.fonts.uk,
                18 => inf.fonts.uz,
                19 => inf.fonts.es,
                20 => inf.fonts.pt,
                21 => inf.fonts.ar,
                22 => inf.fonts.id,
                23 => inf.fonts.ja,
                24 => inf.fonts.it,
                25 => inf.fonts.de,
                26 => inf.fonts.hi,
                _ => null
            };
        }

        public static int[] GetFontSize(int i, LocalizationSettings inf)
        {
            return i switch
            {
                0 => inf.fontsSizeCorrect.ru,
                1 => inf.fontsSizeCorrect.en,
                2 => inf.fontsSizeCorrect.tr,
                3 => inf.fontsSizeCorrect.az,
                4 => inf.fontsSizeCorrect.be,
                5 => inf.fontsSizeCorrect.he,
                6 => inf.fontsSizeCorrect.hy,
                7 => inf.fontsSizeCorrect.ka,
                8 => inf.fontsSizeCorrect.et,
                9 => inf.fontsSizeCorrect.fr,
                10 => inf.fontsSizeCorrect.kk,
                11 => inf.fontsSizeCorrect.ky,
                12 => inf.fontsSizeCorrect.lt,
                13 => inf.fontsSizeCorrect.lv,
                14 => inf.fontsSizeCorrect.ro,
                15 => inf.fontsSizeCorrect.tg,
                16 => inf.fontsSizeCorrect.tk,
                17 => inf.fontsSizeCorrect.uk,
                18 => inf.fontsSizeCorrect.uz,
                19 => inf.fontsSizeCorrect.es,
                20 => inf.fontsSizeCorrect.pt,
                21 => inf.fontsSizeCorrect.ar,
                22 => inf.fontsSizeCorrect.id,
                23 => inf.fontsSizeCorrect.ja,
                24 => inf.fontsSizeCorrect.it,
                25 => inf.fontsSizeCorrect.de,
                26 => inf.fontsSizeCorrect.hi,
                _ => null
            };
        }

        public static string UnauthorizedTextTranslate(string languageTranslate)
        {
            string name;

            switch (languageTranslate)
            {
                case "ru":
                    name = "неавторизованный";
                    break;
                case "en":
                    name = "unauthorized";
                    break;
                case "tr":
                    name = "yetkisiz";
                    break;
                case "az":
                    name = "icazəsiz";
                    break;
                case "be":
                    name = "неаўтарызаваны";
                    break;
                case "he":
                    name = "---";
                    break;
                case "hy":
                    name = "---";
                    break;
                case "ka":
                    name = "---";
                    break;
                case "et":
                    name = "loata";
                    break;
                case "fr":
                    name = "non autorisé";
                    break;
                case "kk":
                    name = "рұқсат етілмеген";
                    break;
                case "ky":
                    name = "уруксатсыз";
                    break;
                case "lt":
                    name = "neleistinas";
                    break;
                case "lv":
                    name = "neleistinas";
                    break;
                case "ro":
                    name = "neautorizat";
                    break;
                case "tg":
                    name = "беиҷозат";
                    break;
                case "tk":
                    name = "yetkisiz";
                    break;
                case "uk":
                    name = "несанкціонований";
                    break;
                case "uz":
                    name = "ruxsatsiz";
                    break;
                case "es":
                    name = "autorizado";
                    break;
                case "pt":
                    name = "autorizado";
                    break;
                case "ar":
                    name = "---";
                    break;
                case "id":
                    name = "tidak sah";
                    break;
                case "ja":
                    name = "---";
                    break;
                case "it":
                    name = "autorizzato";
                    break;
                case "de":
                    name = "unerlaubter";
                    break;
                case "hi":
                    name = "---";
                    break;
                default:
                    name = "unauthorized";
                    break;
            }

            return name;
        }
        public static string IsHiddenTextTranslate(string languageTranslate)
        {
            string name;

            switch (languageTranslate)
            {
                case "ru":
                    name = "скрыт";
                    break;
                case "en":
                    name = "is hidden";
                    break;
                case "tr":
                    name = "gizli";
                    break;
                case "az":
                    name = "gizlidir";
                    break;
                case "be":
                    name = "скрыты";
                    break;
                case "he":
                    name = "---";
                    break;
                case "hy":
                    name = "---";
                    break;
                case "ka":
                    name = "---";
                    break;
                case "et":
                    name = "on peidetud";
                    break;
                case "fr":
                    name = "est caché";
                    break;
                case "kk":
                    name = "жасырылған";
                    break;
                case "ky":
                    name = "жашыруун";
                    break;
                case "lt":
                    name = "yra paslėpta";
                    break;
                case "lv":
                    name = "ir paslēpts";
                    break;
                case "ro":
                    name = "este ascuns";
                    break;
                case "tg":
                    name = "пинҳон аст";
                    break;
                case "tk":
                    name = "gizlenendir";
                    break;
                case "uk":
                    name = "прихований";
                    break;
                case "uz":
                    name = "yashiringan";
                    break;
                case "es":
                    name = "Está oculto";
                    break;
                case "pt":
                    name = "está escondido";
                    break;
                case "ar":
                    name = "---";
                    break;
                case "id":
                    name = "tersembunyi";
                    break;
                case "ja":
                    name = "---";
                    break;
                case "it":
                    name = "è nascosto";
                    break;
                case "de":
                    name = "ist versteckt";
                    break;
                case "hi":
                    name = "---";
                    break;
                default:
                    name = "is hidden";
                    break;
            }

            return name;
        }
    }
}