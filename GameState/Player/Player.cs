using System;
using System.Collections.Generic;
using System.Text;
using GameState;
using GameState.GameRules;

namespace GameState
{
    public class Player : BoardCharacter, IConsoleDrawable
    {
        public Player(int ownerId, string name, IDeck deck)
        {
            OwnerId = ownerId;
            Name = name;
            Deck = deck;
            Hand = new Hand(ownerId);
            Board = new PlayerBoard(ownerId);
            Coins = new CoinValueState(0);
            Attack = new AttackValueState(0);
            Health = new HealthValueState(Constants.BasePlayerHealth);
            IsMyTurn = false;
        }

        public IDeck Deck { get; private set; }
        public Hand Hand { get; private set; }
        public PlayerBoard Board { get; private set; }
        public CoinValueState Coins { get; private set; }
        public bool IsMyTurn { get; set; }

        public void ResetPlayer()
        {
            Deck.ResetDeck();
            Hand.ResetHand();
            Board.ResetBoard();
            Coins = new CoinValueState(0);
            Attack = new AttackValueState(0);
            Health = new HealthValueState(Constants.BasePlayerHealth);
        }

        public void PromptTurnActions()
        {
            ConsoleKey actionSelected = ConsoleKey.Spacebar;
            do
            {
                GameState.PrintGame();
                PrintPlayersTurnDisplay();
                Hand.PrintHand();
                var actions = new Dictionary<ConsoleKey, (string Label, Action<int> Callback)>();

                foreach (var (playableCard, index) in Hand.GetPlayableCards(Coins.CurrentValue).WithIndex())
                {
                    actions[Constants.HandCardKeys[index]] =
                        ($"Play {playableCard.GameToString()}", Hand.PlayCard(playableCard.Id));
                }

                foreach (var attackableMinion in Board.GetMinionsThatCanAttack())
                {
                    actions[attackableMinion.Key] =
                        ($"Attack with {attackableMinion.Value.GameToString()}",
                            attackableMinion.Value.PromptAttackAndAttack);
                }

                actions.Add(Constants.EndTurnKey, ("End turn", Constants.DoNothing));

                actionSelected = Prompts.TurnActions(actions);
                actions[actionSelected].Callback(OwnerId);
            } while (actionSelected != Constants.EndTurnKey);
        }
        
        public void Draw()
        {
            var card = Deck.Draw();
            if (Hand.GetAllCards().Count < Constants.MaxHandSize)
            {
                // TODO: fix
                // card.OnDraw(OwnerId);
                Hand.AddCard(card);
            }
        }

        public List<string> GetDrawToConsoleLines()
        {
            var health = Health.GameToStringValues();
            var attack = Attack.GameToStringValues();
            var attackText = attack.Value == "0" ? "__" : $"{ColorConsole.FormatEmbeddedColor(attack.Value.PadRight(2, '_'), attack.Color)}";
            var lines = new List<string>()
            {
                $"  ___  ",
                $" /   \\ ",
                $"|     |",
                $"| P {OwnerId} |",
                $"|     |",
                $"|{attackText}_{ColorConsole.FormatEmbeddedColor(health.Value.PadLeft(2, '_'), health.Color)}|",
            };
            return lines;
        }

        public List<string> GetCoinsDrawToConsoleLines()
        {
            var availableCoins = Coins.CurrentValue;
            var spentCoins = Coins.BaseValue - Coins.CurrentValue;
            var lines = GameToConsoleHelper.FormatCoinsDrawToConsoleLines(availableCoins, spentCoins);
            return lines;
        }

        public override string GameToString()
        {
            var health = Health.GameToStringValues();
            var attack = Attack.GameToStringValues();
            var attackText = attack.Value == "0" ? string.Empty : $"{ColorConsole.FormatEmbeddedColor(attack.Value, attack.Color)}/";
            return $"[ {Name} ({attackText}{ColorConsole.FormatEmbeddedColor(health.Value, health.Color)}) ]";
        }

        public void PrintPlayersTurnDisplay()
        {
            var spacerLength = 25;
            ColorConsole.WriteLine(" _,.-'~'-.,__,.-'~'-.,__,.-'~'-.,__,.-'~'-.,__,.-'~'-.,__,.-'~'-.,_ ");
            ColorConsole.WriteLine($"|                                                                  |");
            ColorConsole.WriteLine($"|{new string(' ', spacerLength)}{ Name }'s Turn {new string(' ', spacerLength)}|");
            ColorConsole.WriteLine($"|                                                                  |");
            ColorConsole.WriteLine("|_,.-'~'-.,__,.-'~'-.,__,.-'~'-.,__,.-'~'-.,__,.-'~'-.,__,.-'~'-.,_|");
        }
    }
}
