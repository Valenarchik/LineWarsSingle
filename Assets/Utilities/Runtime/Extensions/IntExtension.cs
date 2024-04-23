using System.Collections.Generic;

public static class IntExtension
{
    private static Dictionary<int, string> romanNumbers => new Dictionary<int, string>
    {
        {1, "I"},
        {2, "II"},
        {3, "III"},
        {4, "IV"},
        {5, "V"},
        {6,"VI"},
        {7,"VII"},
        {8,"VIII"},
        {9,"XI"},
        {10,"X"},
        {11, "XI"},
        {12, "XII" },
        {13, "XIII" },
        {14, "XIV" },
        {15, "XV" },
        {16, "XVI" },
        {17, "XVII" },
        {18, "XVIII" },
        {19, "XIX" },
        {20, "XX" }
    };

    public static string GetRoman(this int value)
    {
        if (!romanNumbers.ContainsKey(value))
            return value.ToString();
        return romanNumbers[value];
    }
    public static int GetValueInRind(this int value, int module)
    {
        var temp = value % module;
        return temp < 0 ? temp + module : temp;
    }

    public static string ToText(this int value)
    {
        return value switch
        {
            0 => "Ноль",
            1 => "Один",
            2 => "Два",
            3 => "Три",
            4 => "Четыре",
            5 => "Пять",
            6 => "Шесть",
            7 => "Семь",
            8 => "Восемь",
            9 => "Девять",
            10 => "Десять",
            11 => "Одиннадцать",
            12 => "Двенадцать",
            13 => "Тринадцать",
            14 => "Четырнадцать",
            15 => "Пятнадцать",
            16 => "Шестнадцать",
            17 => "Семнадцать",
            18 => "Восемнадцать",
            19 => "Девятнадцать",
            20 => "Двадцать",
            21 => "Двадцать один",
            _ => value.ToString() + "Incorrect"
        };
    }
    /// <summary>
    /// Возвращает слова в падеже, зависимом от заданного числа
    /// </summary>
    /// <param name="number">Число от которого зависит выбранное слово</param>
    /// <param name="nominativ">Именительный падеж слова. Например "день"</param>
    /// <param name="genetiv">Родительный падеж слова. Например "дня"</param>
    /// <param name="plural">Множественное число слова. Например "дней"</param>
    /// <returns></returns>
    public static string Pluralize(this int number, string nominativ, string genetiv, string plural) {
        var titles = new[] {nominativ, genetiv, plural};
        var cases = new[] {2, 0, 1, 1, 1, 2};
        return titles[number % 100 > 4 && number % 100 < 20 ? 2 : cases[(number % 10 < 5) ? number % 10 : 5]];
    }
    
    public static string ToChars(this int d, int charsAmount)
    {
        var str = d.ToString();
        if (str.Length >= charsAmount) return str;
        return new string('0', charsAmount - str.Length) + str;
    }
}