﻿using System;
using System.Collections.Generic;
using System.Text;
using GameState.GameRules;

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

        public static BoardCharacter SelectTarget(Guid currentPlayerId, TargetCategory targetCategory,
            Predicate<BoardCharacter> filterPredicate = null)
        {
            var targetDictionary = TargetMatcher.GetTargets(currentPlayerId, targetCategory, filterPredicate);
            targetDictionary.Add(Constants.CancelKey, null);
            ColorConsole.WriteLine("Select Target: ");
            foreach (var prompt in targetDictionary)
            {
                ColorConsole.WriteEmbeddedColorLine($"\t{prompt.Key} - {prompt.Value.GameToString(currentPlayerId)}");
            }
            var input = Console.ReadKey();
            if (!targetDictionary.ContainsKey(input.Key))
            {
                ColorConsole.WriteLine("Unrecognized input. Try again.");
                return SelectTarget(currentPlayerId, targetCategory, filterPredicate);
            }
            return targetDictionary[input.Key];
        }
    }
}
