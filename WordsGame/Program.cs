using System.Globalization;
using System.Text.RegularExpressions;
using WordsGame.Languages;

class Program
{
    public static void Main()
    {
        CreateMenu();
        Console.Clear();
        Console.WriteLine(Messages.InputWord);
        string? primaryWord = (Console.ReadLine() ?? "").ToLower();

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
            Console.WriteLine(Messages.Lose);
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
            Console.WriteLine(number % 2 == 0 ? "\n" + Messages.FirstPlayerTurn : "\n" + Messages.SecondPlayerTurn);
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000;
            timer.Elapsed += TimerElapsed;
            timer.Start();
            string? composedWord = (Console.ReadLine() ?? "").ToLower();

            if (!CheckComposedWord(composedWord))
            {
                break;
            }
            
            if (!MatchLetters(primaryWord, composedWord))
            {
                Console.WriteLine(Messages.IncorectCompose + ' ' + Messages.Lose);
                break;
            }

            if (usedWords.Contains(composedWord))
            {
                Console.WriteLine(Messages.WordIsUsed);
                break;
            }

            usedWords.Add(composedWord);
            number++;
        }
        Main();
    }

    public static bool MatchLetters(string primaryWord, string composedWord)
    {
        var primaryWordLetters = primaryWord
            .GroupBy(c => c)
            .Select(g => new { Letter = g.Key, Count = g.Count() });

        var composedWordLetters = composedWord
            .GroupBy(c => c)
            .Select(g => new { Letter = g.Key, Count = g.Count() });

        foreach (var c1 in primaryWordLetters)
        {
            foreach (var c2 in composedWordLetters)
            {
                if ((c1.Letter == c2.Letter && c1.Count < c2.Count) || !primaryWord.Contains(c2.Letter))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        Console.WriteLine(Messages.TimerElapsed + ' ' + Messages.Lose);
        Environment.Exit(0);
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

    public static void CreateMenu()
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