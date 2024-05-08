using UnityEngine;

namespace Localization
{
    [CreateAssetMenu(menuName = "Localization/Settings", fileName = "LocalizationSettings")]
    public class LocalizationSettings : ScriptableObject
    {
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
}
