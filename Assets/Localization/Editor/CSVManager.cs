using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using TMPro;

namespace Localization
{
    public class CSVManager
    {
        public static readonly string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

        // Чтение CSV файла. Поиск переводов по ключу
        public static string[] ImportTransfersByKey(LanguageText lang)
        {
            TextAsset data = Resources.Load(lang.Settings.CSVFileTranslate.name) as TextAsset;

            string[] keys = Regex.Split(CommaFormat(data.text), LINE_SPLIT_RE);
            string[] result = new string[lang.languages.Length];

            bool complete = false;

            for (int i = 1; i < keys.Length; i++)
            {
                string[] translates = Regex.Split(keys[i], ",");

                if (translates[0] == GetKeyForLangYG(lang))
                {
                    for (int i2 = 0; i2 < lang.languages.Length; i2++)
                    {
                        if (LangMethods.LangArr(lang.Settings)[i2])
                        {
                            result[i2] = translates[i2 + 1].Replace("*", ",").Replace(@"\n", "\n");
                            complete = true;
                        }
                    }
                }
            }

            if (complete)
                return result;
            else
            {
                Debug.Log("(en) Couldn't find a translation for this object! (ru) Не удалось найти перевод для данного объекта!", lang);
                return null;
            }
        }



        // Запись всего текста
        public static void WriteCSVFile(LocalizationSettings settings, string[,] keys, string[] idArray)
        {
            string textFile = "";
            string[] oldIDs = null;

            if (!File.Exists(Patch(settings))) // если файла нет
            {
                // Записываем первую строку
                textFile = CreateFirstLine(settings);
            }

            else // Если файл есть
            {
                oldIDs = File.ReadAllLines(Patch(settings));
                oldIDs = CommaFormat(oldIDs);

                // Записываем строки старых ключ-значений
                for (int i = 0; i < oldIDs.Length; i++)
                {
                    oldIDs[i] = RedLineFormat(AsteriskFormat(oldIDs[i]));
                    textFile += oldIDs[i] + "\n";
                }

                // Создаем из поля oldIDs массив с ID старых переводов
                for (int i = 1; i < oldIDs.Length; i++)
                {
                    oldIDs[i] = oldIDs[i];
                    int remIndex = oldIDs[i].IndexOf(",") + 1;
                    oldIDs[i] = oldIDs[i].Remove(remIndex);
                }
            }

            // Запускаем цикл для записи новых ключ-значений
            for (int column = 0; column < idArray.Length; column++)
            {
                // Проверка существует ли уже такой ключ
                bool clone = false;

                if (oldIDs != null)
                {
                    for (int i = 0; i < oldIDs.Length; i++)
                    {
                        if (idArray[column] + "," == oldIDs[i])
                        {
                            clone = true;
                        }
                    }
                }

                // Запись текста
                if (!clone)
                {
                    for (int line = 0; line < LangMethods.LangArr(settings).Length + 1; line++)
                    {
                        keys[column, line] = RedLineFormat(AsteriskFormat(keys[column, line])).Replace(",", "*");
                        textFile += keys[column, line];

                        if (line != LangMethods.LangArr(settings).Length)
                            textFile += ",";
                    }

                    textFile += "\n";
                }
            }

            WriteCSV(settings, textFile);
        }



        // Запись одного текста
        public static void SetIDLineFile(LocalizationSettings settings, LanguageText locObj)
        {
            if (!File.Exists(Patch(settings))) // Eсли файла нет
            {
                // Создаём CSV файл и первую строчку
                using (FileStream file = new FileStream(Patch(settings), FileMode.Create))
                using (StreamWriter stream = new StreamWriter(file))
                    stream.Write(CreateFirstLine(settings), Patch(settings));
            }

            // Сздаём массив новых ключ-значений
            string[] lines = File.ReadAllLines(Patch(settings));
            lines = CommaFormat(lines);

            string replace = null;

            // Запускаем цикл для поиска клона
            for (int i = 1; i < lines.Length; i++)
            {
                // Если в станых строках обнаружится совпадение с именем ID
                if (lines[i].StartsWith(GetKeyForLangYG(locObj) + ","))
                {
                    // То мы вписываем в replace ID
                    replace = AsteriskFormat(GetKeyForLangYG(locObj));

                    // И вписываем все переводы
                    for (int i2 = 0; i2 < LangMethods.LangArr(settings).Length; i2++)
                    {
                        replace += "," + RedLineFormat(locObj.languages[i2]).Replace(",", "*");
                    }

                    // Меняем старый ключ-значение на новый
                    lines[i] = replace;
                    break;
                }
            }

            string result = "";

            // Записываем имеющийся результат в result
            for (int i = 0; i < lines.Length; i++)
            {
                result += lines[i] + "\n";
            }

            // Если не было дубляжа, то записываем ключ-значение на новой строкче
            if (replace == null)
            {
                // Вписываем в новой строчке сначала ID нового ключ-зачения
                result += RedLineFormat(AsteriskFormat(GetKeyForLangYG(locObj)));

                // Потом записываем все переводы
                for (int i = 0; i < LangMethods.LangArr(settings).Length; i++)
                {
                    result += "," + RedLineFormat(locObj.languages[i]).Replace(",", "*");
                }

                result += "\n";
            }

            WriteCSV(settings, result);
        }



        static string Patch(LocalizationSettings settings)
        {
            string patch = Application.dataPath + "/Resources/";

            if (!File.Exists(patch))
                Directory.CreateDirectory(patch);

            return patch + settings.CSVFileTranslate.name + ".csv";
        }

        static string CreateFirstLine(LocalizationSettings infoYG)
        {
            string firstLine = "KEYLANGUAGE";

            for (int i = 0; i < LangMethods.LangArr(infoYG).Length; i++)
            {
                firstLine += "," + FullNameLanguages()[i];
            }

            firstLine += "\n";

            return firstLine;
        }

        static void WriteCSV(LocalizationSettings infoYG, string data)
        {
            if (infoYG.CSVFileTranslate.format == FileFormat.ExcelOffice)
                data = SemicolonFormat(data);

            data = AsteriskFormat(data);

            using (FileStream file = new FileStream(Patch(infoYG), FileMode.Create))
            using (StreamWriter stream = new StreamWriter(file))
                stream.Write(data, Patch(infoYG));
        }

        public static string GetKeyForLangYG(LanguageText lang)
        {
            string key = null;

            if (!lang.componentTextField)
            {
                key = lang.text;
            }
            else if (lang.textMPComponent)
            {
                key = lang.textMPComponent.text;
            }

            return key.Replace(",", "*");
        }

        public static string CommaFormat(string line)
        {
            return AsteriskFormat(line.Replace(";", ","));
        }

        static string[] CommaFormat(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = CommaFormat(lines[i]);
            }

            return lines;
        }

        static string SemicolonFormat(string line)
        {
            if (line.Length > 0)
                return line.Replace(",", ";");
            else
                return "";
        }

        static string AsteriskFormat(string line)
        {
            if (line.Length > 0)
                return line.Replace(", ", "* ");
            else
                return "";
        }

        static string RedLineFormat(string line)
        {
            return string.IsNullOrEmpty(line) ? "" : line.Replace("\n", @"\n");
        }

        public static string[] FullNameLanguages()
        {
            string[] s = new string[27];

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
    }
}