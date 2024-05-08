using System;
using UnityEngine;

namespace Localization
{
    [Serializable]
    public class CSVTranslate
    {
        [Tooltip("Формат scv файла. \nGoogleSheets - Создаст файл с разделительной запятой (,) \nExcelOffice - Создаст файл с разделительной точкой с запятой (;).")]
        public FileFormat format;

        [Tooltip("Имя CSV файла.")]
        public string name = "TranslateCSV";
    }
}