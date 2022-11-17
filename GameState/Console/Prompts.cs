using System;
using System.Collections.Generic;
using System.Text;
using GameState.GameRules;

namespace GameState
{
    public static class Prompts
    {
        public static readonly string InputErrorMessage = "Unrecognized input. Try again.";
        public static readonly string PressAnyKeyMessage = "Press any key to return:";
        public static bool WantToPlayAgain()
        {
            ColorConsole.WriteLine($"Want to play again? ({Constants.YesKey}/{Constants.NoKey})");
            var input = new PlayerInput(Console.ReadKey().Key);
            if (Equals(input, Constants.YesKey))
            {
                return true;
            }
            if (Equals(input, Constants.NoKey))
            {
                return false;
            }
            ColorConsole.WriteLine(InputErrorMessage);
            return WantToPlayAgain();
        }

        public static PlayerInput TurnActions(Dictionary<PlayerInput, (string Label, Guid CallbackParam, Action<Guid> Callback)> actions)
        {
            ColorConsole.WriteLine("Cards in hand/turn actions: ");
            foreach (var prompt in actions)
            {
                ColorConsole.WriteEmbeddedColorLine($"\t{prompt.Key.ToStringFormattedColor()} - {prompt.Value.Label}");
            }
            var input = new PlayerInput(Console.ReadKey().Key);
            if (!actions.ContainsKey(input))
            {
                ColorConsole.WriteLine(InputErrorMessage);
                return TurnActions(actions);
            }
            return input;
        }

        public static BoardCharacter SelectAttackTarget(Guid playerId)
        {
            var targets = GameController.GetOpponent(playerId).Board.GetAttackableTargets();
            targets.Add(Constants.CancelKey, null);
            ColorConsole.WriteLine($"Choose a target to attack: ");
            foreach (var target in targets)
            {
                var targetDisplay = string.Empty;
                switch (target.Value)
                {
                    case Minion m:
                        targetDisplay = m.GameToString();
                        break;
                    case Player p:
                        targetDisplay = p.GameToString();
                        break;
                }
                ColorConsole.WriteEmbeddedColorLine(Equals(target.Key, Constants.CancelKey)
                    ? $"\t{target.Key} - Cancel attack"
                    : $"\t{target.Key} - {targetDisplay}");
            }

            var input = new PlayerInput(Console.ReadKey().Key);
            if (!targets.ContainsKey(input))
            {
                ColorConsole.WriteLine(InputErrorMessage);
                return SelectAttackTarget(playerId);
            }

            return targets[input];
        }

        public static BoardCharacter SelectTarget(Guid currentPlayerId, TargetCategory targetCategory,
            Predicate<BoardCharacter> filterPredicate = null)
        {
            var targetDictionary = TargetMatcher.GetTargets(currentPlayerId, targetCategory, filterPredicate);
            targetDictionary.Add(Constants.CancelKey, null);
            ColorConsole.WriteLine("\nSelect Target: ");
            foreach (var prompt in targetDictionary)
            {
                switch (prompt.Value)
                {
                    case Minion m:
                        ColorConsole.WriteEmbeddedColorLine($"\t{prompt.Key} - {m.GameToString(currentPlayerId)}");
                        break;
                    case Player p:
                        ColorConsole.WriteEmbeddedColorLine($"\t{prompt.Key} - {p.GameToString(currentPlayerId)}");
                        break;
                    case null:
                        ColorConsole.WriteEmbeddedColorLine($"\t{prompt.Key} - Cancel");
                        break;
                }
            }
            var input = new PlayerInput(Console.ReadKey().Key);
            if (!targetDictionary.ContainsKey(input))
            {
                ColorConsole.WriteLine(InputErrorMessage);
                return SelectTarget(currentPlayerId, targetCategory, filterPredicate);
            }
            return targetDictionary[input];
        }

        public static void HistoryView(Guid irrelevant)
        {
            Console.Clear();
            GameController.HistoryLog.PrintNthEvents(40);
            ColorConsole.WriteLine($"\n{PressAnyKeyMessage}");
            Console.ReadKey();
        }

        public static void CardIsUnplayable(Guid irrelevant)
        {
            ColorConsole.WriteLine($"\nYou cannot play that card. {PressAnyKeyMessage}");
            Console.ReadKey();
        }
    }
}
