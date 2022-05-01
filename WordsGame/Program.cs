using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using WordsGame;

class Task1
{
    public static void Main(string[] args)
    {
        int index = SelectionLanguageMenu();
        ExecuteMenuElement(index);
        Console.Clear();
        Console.WriteLine(Language.InitialWord);
        string initialWord = Console.ReadLine();

        if ((initialWord?.Length is < 8 or > 30) || initialWord == string.Empty)
        {
            Console.WriteLine(Language.InitialWordLength);
            return;
        }

        if (!Regex.IsMatch(initialWord, Language.LettersRegex))
        {
            Console.WriteLine(Language.OnlyLetters);
            return;
        }

        int number = 0;
        while (true)
        {
            if (number % 2 == 0)
            {
                Console.WriteLine('1' + Language.InputWord);
            }
            else
            {
                Console.WriteLine('2' + Language.InputWord);
            }

            string? inputedWord = Console.ReadLine();

            if (inputedWord == string.Empty)
            {
                Console.WriteLine(Language.Lose);
                return;
            }

            if (!Regex.IsMatch(inputedWord, Language.LettersRegex))
            {
                Console.WriteLine(Language.OnlyLetters);
                return;
            }

            bool result = CheckWord(initialWord, inputedWord);
            if (!result)
            {
                Console.WriteLine(Language.Lose);
                return;
            }
            number++;
        }
    }

    public static bool CheckWord(string initialWord, string inputedWord)
    {
        Dictionary<char, int> initialLetters = new Dictionary<char, int>();
        Dictionary<char, int> inputedLetters = new Dictionary<char, int>();

        foreach (char c in initialWord)
        {
            if (!initialLetters.ContainsKey(c))
            {
                initialLetters.Add(c, 0);
            }
            else
            {
                initialLetters[c]++;
            }
        }

        foreach (char c in inputedWord)
        {
            if (!inputedLetters.ContainsKey(c))
            {
                inputedLetters.Add(c, 0);
            }    
            else
            {
                inputedLetters[c]++;
            }
        }

        foreach (var c1 in initialLetters)
        {
            foreach (var c2 in inputedLetters)
            {
                if ((c1.Key == c2.Key && c1.Value < c2.Value) || !initialLetters.ContainsKey(c2.Key))
                {
                    return false;
                }
            }
        }
        return true;
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

    public static int SelectionLanguageMenu()
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