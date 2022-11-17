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
                "| {0}  {1} |",
                "\\______/",
                new string(' ', 8),
            };

            for (int i = 1; i < 6; i++)
            {
                var availableCoinDisplay = "$";
                var availableCoinColor = ConsoleColor.Yellow;

                var spentCoinDisplay = "0";
                var spentCoinColor = ConsoleColor.DarkRed;

                var noCoinDisplay = " ";
                var noCoinColor = ConsoleColor.White;

                var firstReplace = string.Empty;
                if (availableCoins > 0)
                {
                    firstReplace = ColorConsole.FormatEmbeddedColor(availableCoinDisplay, availableCoinColor);
                    availableCoins--;
                }
                else if (spentCoins > 0)
                {
                    firstReplace = ColorConsole.FormatEmbeddedColor(spentCoinDisplay, spentCoinColor);
                    spentCoins--;
                }
                else
                {
                    firstReplace = ColorConsole.FormatEmbeddedColor(noCoinDisplay, noCoinColor);
                }

                var secondReplace = string.Empty;
                if (availableCoins > 0)
                {
                    secondReplace = ColorConsole.FormatEmbeddedColor(availableCoinDisplay, availableCoinColor);
                    availableCoins--;
                }
                else if (spentCoins > 0)
                {
                    secondReplace = ColorConsole.FormatEmbeddedColor(spentCoinDisplay, spentCoinColor);
                    spentCoins--;
                }
                else
                {
                    secondReplace = ColorConsole.FormatEmbeddedColor(noCoinDisplay, noCoinColor);
                }

                lines[i] = string.Format(lines[i], firstReplace, secondReplace);
            }

            return lines;
        }
    }
    
}
