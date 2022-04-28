class WordsGame
{
    public static void Main(string[] args)
    {
        LanguageKey key = ConsoleMenu();
        string[] language = SetInterfaceLanguage(key);

        Console.WriteLine(language[0]);
        
    }

    public static LanguageKey ConsoleMenu()
    {
        int enumLength = Enum.GetNames(typeof(LanguageKey)).Length;
        int index = 0;
        Console.CursorVisible = false;
        while (true)
        {
            PrintMenuElements(enumLength, index);
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    index--;
                    break;
                case ConsoleKey.DownArrow:
                    index++;
                    break;
                case ConsoleKey.Enter:
                    Console.CursorVisible = true;
                    return (LanguageKey)Enum.GetValues(typeof(LanguageKey)).GetValue(index);
            }
            index = (index + enumLength) % enumLength;
        }
    }

    public static void PrintMenuElements(int enumLength, int index)
    {
        Console.Clear();
        Console.WriteLine("Choose interface language:");
        for (int i = 0; i < enumLength; i++)
        {
            Console.WriteLine("{0} {1}", Enum.GetNames(typeof(LanguageKey))[i], i == index ? "<<--" : "");
        }
    }

    public enum LanguageKey
    {
        Russian,
        English
    }

    public static string[] SetInterfaceLanguage(LanguageKey key)
    {
        string[] russian =
        {
            "Введите начально слово (длина от 8 до 30 букв):",
            "Нет"
        };

        string[] english =
        {
            "Yes",
            "No"
        };

        Dictionary<LanguageKey, string[]> languages = new Dictionary<LanguageKey, string[]>
        {
            { LanguageKey.Russian, russian },
            { LanguageKey.English, english }
        };
        return languages[key];
    }


}