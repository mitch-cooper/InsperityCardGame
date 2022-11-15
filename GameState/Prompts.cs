using System;
using System.Collections.Generic;
using System.Text;

namespace GameState
{
    public static class Prompts
    {
        public static bool WantToPlayAgain()
        {
            ColorConsole.WriteLine($"Want to play again? ({Constants.YesKey}/{Constants.NoKey})");
            var input = Console.ReadKey();
            if (input.Key == Constants.YesKey)
            {
                return true;
            }
            if (input.Key == Constants.NoKey)
            {
                return false;
            }
            ColorConsole.WriteLine("Unrecognized input. Try again.");
            return WantToPlayAgain();
        }

        public static ConsoleKey TurnActions(Dictionary<ConsoleKey, (string Label, Guid CallbackParam, Action<Guid> Callback)> actions)
        {
            ColorConsole.WriteLine("Turn Actions: ");
            foreach (var prompt in actions)
            {
                ColorConsole.WriteEmbeddedColorLine($"\t{prompt.Key} - {prompt.Value.Label}");
            }
            var input = Console.ReadKey();
            if (!actions.ContainsKey(input.Key))
            {
                ColorConsole.WriteLine("Unrecognized input. Try again.");
                return TurnActions(actions);
            }
            return input.Key;
        }
    }
}
