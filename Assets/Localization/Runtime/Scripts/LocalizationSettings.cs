using System;
using TMPro;
using UnityEngine;

namespace Localization
{
    [CreateAssetMenu(menuName = "Localization/Settings", fileName = "LocalizationSettings")]
    public class LocalizationSettings : ScriptableObject
    {
        public bool LocalizationEnable;
        [Tooltip("Метод перевода. \nAutoLocalization - Автоматический перевод через интернет с помощью Google Translate \nManual - Ручной режим. Вы сами записываете перевод в компоненте LanguageYG \nCSVFile - Перевод с помощью Excel файла.")]
        public TranslateMethod translateMethod;
        
        [Tooltip("Выберите языки, на которые будет переведена Ваша игра.")]
        public Languages languages;

        [Tooltip("Здесь вы можете выбрать одельные шрифты для каждого языка.")]
        public FontsTMP fonts;
        
        [Tooltip("Вы можете скорректировать размер шрифта для каждого языка. Допустим, для Японского языка вы можете указать -3. В таком случае, если бы базовый размер был бы, например, 10, то для японского языка он бы стал равен 7.")]
        public FontsSizeCorrect fontsSizeCorrect;

        [Tooltip("Домен с которого будет скачиваться перевод. Если у вас возникли проблемы с авто-переводом, попробуйте поменять домен.")]
        public string domainAutoLocalization = "com";
        
        [Tooltip("Настройки для метода локализации с помощью CSV файла. Это подразоумивает перевод по ключам всех текстов игры в таблице Excel или Google Sheets.")]
        public CSVTranslate CSVFileTranslate;
    }
    
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
    }
    
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
    }
    
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
    }

    public enum TranslateMethod
    {
        AutoLocalization, 
        Manual,
        CSVFile
    }

    public enum FileFormat
    {
        GoogleSheets,
        ExcelOffice
    };
    
    [Serializable]
    public class CSVTranslate
    {
        [Tooltip("Формат scv файла. \nGoogleSheets - Создаст файл с разделительной запятой (,) \nExcelOffice - Создаст файл с разделительной точкой с запятой (;).")]
        public FileFormat format;

        [Tooltip("Имя CSV файла.")]
        public string name = "TranslateCSV";
    }
}
