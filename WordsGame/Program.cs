﻿using System.Globalization;
using System.Text.RegularExpressions;
using WordsGame.Languages;

class Program
{
    public static void Main()
    {
        DisplayLanguageMenu();
        Console.Clear();
        Console.WriteLine(Language.InitialWord);
        string? primaryWord = InputPrimaryWord();

        if (primaryWord == null)
        {
            return;
        }

        StartGameplay(primaryWord);

    }

    public static string? InputPrimaryWord()
    {
        string primarylWord = Console.ReadLine() ?? "";

        if ((primarylWord?.Length is < 8 or > 30) || primarylWord == string.Empty)
        {
            Console.WriteLine(Language.InitialWordLength);
            return null;
        }

        if (!Regex.IsMatch(primarylWord, Language.LettersRegex))
        {
            Console.WriteLine(Language.OnlyLetters);
            return null;
        }
        return primarylWord;
    }

    public static void StartGameplay(string primaryWord)
    {
        List<string> usedWords = new List<string>();
        int number = 0;
        string[] uw;

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

            string? composedWord = Console.ReadLine();

            if (composedWord == string.Empty)
            {
                Console.WriteLine(Language.Lose);
                return;
            }

            if (!Regex.IsMatch(composedWord, Language.LettersRegex))
            {
                Console.WriteLine(Language.OnlyLetters);
                return;
            }

            bool result = CheckWord(primaryWord, composedWord);
            if (!result)
            {
                Console.WriteLine(Language.Lose);
                return;
            }

            if (usedWords.Contains(composedWord))
            {
                Console.WriteLine("Такое слово уже было введено");
                Console.WriteLine(Language.Lose);
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
        Console.WriteLine(Language.SelectLanguage);
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
        int index = 0;
        string key = menuElements[index];

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
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(languages[key]);
                    return;
            }
            key = menuElements[index];
            index = (index + menuElements.Length) % menuElements.Length;
        }
    }
}