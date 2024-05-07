using UnityEngine;
using TMPro;

namespace Localization
{
    public class LangMethods
    {
        public static bool[] GetLangArr(LocalizationSettings settings)
        {
            return settings.languages.Values;
        }
        
        public static string GetLangName(LangName langName)
        {
            return GetLangName((int) langName);
        }
        
        public static string GetLangName(int i)
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
        
        public static TMP_FontAsset[] GetFontTMP(LangName langName, LocalizationSettings settings)
        {
            return GetFontTMP((int) langName, settings);
        }
        
        public static TMP_FontAsset[] GetFontTMP(int i, LocalizationSettings settings)
        {
            return i switch
            {
                0 => settings.fonts.ru,
                1 => settings.fonts.en,
                2 => settings.fonts.tr,
                3 => settings.fonts.az,
                4 => settings.fonts.be,
                5 => settings.fonts.he,
                6 => settings.fonts.hy,
                7 => settings.fonts.ka,
                8 => settings.fonts.et,
                9 => settings.fonts.fr,
                10 => settings.fonts.kk,
                11 => settings.fonts.ky,
                12 => settings.fonts.lt,
                13 => settings.fonts.lv,
                14 => settings.fonts.ro,
                15 => settings.fonts.tg,
                16 => settings.fonts.tk,
                17 => settings.fonts.uk,
                18 => settings.fonts.uz,
                19 => settings.fonts.es,
                20 => settings.fonts.pt,
                21 => settings.fonts.ar,
                22 => settings.fonts.id,
                23 => settings.fonts.ja,
                24 => settings.fonts.it,
                25 => settings.fonts.de,
                26 => settings.fonts.hi,
                _ => null
            };
        }

        public static int[] GetFontSize(LangName langName, LocalizationSettings settings)
        {
            return GetFontSize((int) langName, settings);
        }

        public static int[] GetFontSize(int i, LocalizationSettings settings)
        {
            return i switch
            {
                0 => settings.fontsSizeCorrect.ru,
                1 => settings.fontsSizeCorrect.en,
                2 => settings.fontsSizeCorrect.tr,
                3 => settings.fontsSizeCorrect.az,
                4 => settings.fontsSizeCorrect.be,
                5 => settings.fontsSizeCorrect.he,
                6 => settings.fontsSizeCorrect.hy,
                7 => settings.fontsSizeCorrect.ka,
                8 => settings.fontsSizeCorrect.et,
                9 => settings.fontsSizeCorrect.fr,
                10 => settings.fontsSizeCorrect.kk,
                11 => settings.fontsSizeCorrect.ky,
                12 => settings.fontsSizeCorrect.lt,
                13 => settings.fontsSizeCorrect.lv,
                14 => settings.fontsSizeCorrect.ro,
                15 => settings.fontsSizeCorrect.tg,
                16 => settings.fontsSizeCorrect.tk,
                17 => settings.fontsSizeCorrect.uk,
                18 => settings.fontsSizeCorrect.uz,
                19 => settings.fontsSizeCorrect.es,
                20 => settings.fontsSizeCorrect.pt,
                21 => settings.fontsSizeCorrect.ar,
                22 => settings.fontsSizeCorrect.id,
                23 => settings.fontsSizeCorrect.ja,
                24 => settings.fontsSizeCorrect.it,
                25 => settings.fontsSizeCorrect.de,
                26 => settings.fontsSizeCorrect.hi,
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
        
        public static string[] FullNameLanguages()
        {
            var s = new string[27];

            s[0] = "RUSSIAN";
            s[1] = "ENGLISH";
            s[2] = "TURKISH";
            s[3] = "AZERBAIJANIAN";
            s[4] = "BELARUSIAN";
            s[5] = "HEBREW";
            s[6] = "ARMENIAN";
            s[7] = "GEORGIAN";
            s[8] = "ESTONIAN";
            s[9] = "FRENCH";
            s[10] = "KAZAKH";
            s[11] = "KYRGYZ";
            s[12] = "LITHUANIAN";
            s[13] = "LATVIAN";
            s[14] = "ROMANIAN";
            s[15] = "TAJICK";
            s[16] = "TURKMEN";
            s[17] = "UKRAINIAN";
            s[18] = "UZBEK";
            s[19] = "SPANISH";
            s[20] = "PORTUGUESE";
            s[21] = "ARABIAN";
            s[22] = "INDONESIAN";
            s[23] = "JAPANESE";
            s[24] = "ITALIAN";
            s[25] = "GERMAN";
            s[26] = "HINDI";

            return s;
        }
        
        public static TMP_FontAsset GetCurrentFont(LangName langName, int fontNumber, LocalizationSettings settings)
        {
            var fontArray = GetFontTMP(langName, settings);

            if (fontArray.Length >= fontNumber + 1 && fontArray[fontNumber])
            {
                return fontArray[fontNumber];
            }

            if (settings.fonts.defaultFont.Length >= fontNumber + 1 && settings.fonts.defaultFont[fontNumber])
            {
                return settings.fonts.defaultFont[fontNumber];
            }

            if (settings.fonts.defaultFont.Length >= 1 && settings.fonts.defaultFont[0])
            {
                return settings.fonts.defaultFont[0];
            }

            return null;
        }

        public static int GetCurrentFontSizeCorrect(LangName langName, int fontNumber, LocalizationSettings settings)
        {
            var fontSizeArray = GetFontSize(langName, settings);
            if (fontSizeArray.Length != 0 && fontSizeArray.Length - 1 >= fontNumber)
            { 
                return fontSizeArray[fontNumber];
            }

            return 0;
        }
    }
}