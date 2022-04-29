using System.Globalization;
using System.Text.RegularExpressions;
using WordsGame;

class Task1
{
    public static void Main(string[] args)
    {
        int index = SelectLanguageMenu();
        ExecuteMenuElement(index);
        Console.WriteLine(Language.InitialWord);
        string? initialWord = Console.ReadLine();

        if (initialWord?.Length is < 8 or > 30)
        {
            Console.WriteLine(Language.InitialWordLength);
            return;
        }

        string pattern = @$"{Language.LettersRegex}";
        if (!Regex.IsMatch(initialWord, pattern))
        {
            Console.WriteLine(Language.OnlyLetters);
            return;
        }
        else
        {
            Console.WriteLine("Норм");
            return;
        }

    }

    public static void PrintMenuElements(string[] menuElements, int index)
    {
        Console.Clear();
        Console.WriteLine(Language.SelectLanguage);
        for (int i = 0; i < menuElements.Length; i++)
        {
            Console.WriteLine("{0} {1}", menuElements[i], i == index ? "<<--" : "");
        }
    }

    public static int SelectLanguageMenu()
    {
        string[] menuElements = { "English", "Russian", "Exit"};
        int index = 0;
        Console.CursorVisible = false;
        while (true)
        {
            PrintMenuElements(menuElements, index);
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
                    return index;
            }
            index = (index + menuElements.Length) % menuElements.Length;
        }
    }

    public static void ExecuteMenuElement(int index)
    {
        switch (index)
        {
            case 0:
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                break;
            case 1:
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru-RU");
                break;
            case 2:
                Environment.Exit(0);
                break;
        }
    }


}