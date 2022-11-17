using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameState
{
    internal static class GameToConsoleHelper
    {
        public static List<string> InitializeListWithValue(string value, int rows)
        {
            return Enumerable.Repeat(value, rows).ToList();
        }
        public static List<string> FormatCoinsDrawToConsoleLines(int availableCoins, int spentCoins)
        {
            var lines = new List<string>()
            {
                $"Coins:{ColorConsole.FormatEmbeddedColorPadLeft(availableCoins.ToString(), availableCoins > 0 ? ConsoleColor.Green : ConsoleColor.DarkRed, 2, ' ')}",
                "| {0}  {1} |",
                "| {0}  {1} |",
                "| {0}  {1} |",
                "| {0}  {1} |",
                "|_{0}__{1}_|",
            };
            for (int i = 1; i < lines.Count; i++)
            {
                var firstReplace = string.Empty;
                if (availableCoins > 0)
                {
                    firstReplace = ColorConsole.FormatEmbeddedColor("$", ConsoleColor.Yellow);
                    availableCoins--;
                }
                else if (spentCoins > 0)
                {
                    firstReplace = ColorConsole.FormatEmbeddedColor("O", ConsoleColor.DarkRed);
                    spentCoins--;
                }
                else
                {
                    firstReplace = ColorConsole.FormatEmbeddedColor("_", ConsoleColor.White);
                }

                var secondReplace = string.Empty;
                if (availableCoins > 0)
                {
                    secondReplace = ColorConsole.FormatEmbeddedColor("$", ConsoleColor.Yellow);
                    availableCoins--;
                }
                else if (spentCoins > 0)
                {
                    secondReplace = ColorConsole.FormatEmbeddedColor("O", ConsoleColor.DarkRed);
                    spentCoins--;
                }
                else
                {
                    secondReplace = ColorConsole.FormatEmbeddedColor("_", ConsoleColor.White);
                }

                lines[i] = string.Format(lines[i], firstReplace, secondReplace);
            }

            return lines;
        }
    }
    
}
