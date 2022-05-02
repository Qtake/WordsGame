using System.Globalization;
using System.Text.RegularExpressions;
using WordsGame.Languages;

class Program
{
    public static void Main()
    {
        DisplayLanguageMenu();
        Console.Clear();
        Console.WriteLine(Messages.InputWord);
        string? primaryWord = Console.ReadLine() ?? "";

        if (!CheckPrimaryWord(primaryWord))
        {
            return;
        }

        StartGameplay(primaryWord);
    }

    public static bool CheckPrimaryWord(string primaryWord)
    {
        if ((primaryWord?.Length is < 8 or > 30) || primaryWord == string.Empty)
        {
            Console.WriteLine(Messages.WordLengthError);
            return false;
        }
        return CheckCharacters(primaryWord);
    }

    public static bool CheckComposedWord(string composedWord)
    {
        if (composedWord == string.Empty)
        {
            Console.WriteLine("Проиграл");
            return false;
        }
        return CheckCharacters(composedWord);
    }

    public static bool CheckCharacters(string word)
    {
        if (!Regex.IsMatch(word, Messages.LettersRegex))
        {
            Console.WriteLine(Messages.WordCharactersError);
            return false;
        }
        return true;
    }

    public static void StartGameplay(string primaryWord)
    {
        List<string> usedWords = new List<string>();
        int number = 0;

        while (true)
        {
            if (number % 2 == 0)
            {
                Console.WriteLine(Messages.FirstPlayerTurn);
            }
            else
            {
                Console.WriteLine(Messages.SecondPlayerTurn);
            }

            string? composedWord = Console.ReadLine() ?? "";
            
            if (!CheckWord(primaryWord, composedWord))
            {
                Console.WriteLine(Messages.Lose);
                return;
            }

            if (usedWords.Contains(composedWord))
            {
                Console.WriteLine(Messages.WordIsUsed);
                return;
            }

            usedWords.Add(composedWord);
            number++;
        }
    }

    public static bool CheckWord(string primaryWord, string composedWord)
    {
        Dictionary<char, int> primaryWordLetters = new Dictionary<char, int>();
        Dictionary<char, int> composedWordLetters = new Dictionary<char, int>();

        foreach (char c in primaryWord)
        {
            if (!primaryWordLetters.ContainsKey(c))
            {
                primaryWordLetters.Add(c, 0);
            }
            else
            {
                primaryWordLetters[c]++;
            }
        }

        foreach (char c in composedWord)
        {
            if (!composedWordLetters.ContainsKey(c))
            {
                composedWordLetters.Add(c, 0);
            }    
            else
            {
                composedWordLetters[c]++;
            }
        }

        foreach (var c1 in primaryWordLetters)
        {
            foreach (var c2 in composedWordLetters)
            {
                if ((c1.Key == c2.Key && c1.Value < c2.Value) || !primaryWordLetters.ContainsKey(c2.Key))
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
        Console.WriteLine(Messages.SelectLanguage);
        for (int i = 0; i < menuElements.Length; i++)
        {
            Console.WriteLine("{0} {1}", menuElements[i], i == index ? "<<--" : "");
        }
    }

    public static void DisplayLanguageMenu()
    {
        Dictionary<string, string> languages = new Dictionary<string, string>()
        {
            { "English", "en-US" },
            { "Русский", "ru-RU" }
        };
        string[] menuElements = languages.Keys.ToArray();
        string key;
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
                    key = menuElements[index];
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(languages[key]);
                    return;
            }
            index = (index + menuElements.Length) % menuElements.Length;
        }
    }
}