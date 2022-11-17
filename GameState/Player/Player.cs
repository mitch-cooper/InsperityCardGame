using System;
using System.Collections.Generic;
using System.Text;
using GameState;
using GameState.GameRules;

namespace GameState
{
    public class Player : BoardCharacter, IConsoleDrawable
    {
        private readonly string _displayNumber;

        public Player(Guid ownerId, string name, string displayNumber, ConsoleColor color, IDeck deck)
            : base(ownerId, name, string.Empty, Rarity.Legendary, new CostValueState(99),
                new AttackValueState(0), new HealthValueState(Constants.BasePlayerHealth))
        {
            _displayNumber = displayNumber;
            Deck = deck;
            Color = color;
            Hand = new Hand(ownerId);
            Board = new PlayerBoard(ownerId);
            Coins = new CoinValueState(0);
            IsMyTurn = false;
        }

        public IDeck Deck { get; private set; }
        public Hand Hand { get; private set; }
        public PlayerBoard Board { get; private set; }
        public CoinValueState Coins { get; private set; }
        public bool IsMyTurn { get; set; }
        public ConsoleColor Color { get; set; }

        public void ResetPlayer()
        {
            Deck.ResetDeck();
            Hand.ResetHand();
            Board.ResetBoard();
            Coins = new CoinValueState(0);
            Attack = new AttackValueState(0);
            Health = new HealthValueState(Constants.BasePlayerHealth);
            ResetCharacter();
        }
        protected override void OnDeathEvent()
        {
            OnCharacterDeathEvent(new GameEvent(this, GameEventType.PlayerDeath, $"{Name} has died."));
        }

        public void PromptTurnActions()
        {
            PlayerInput actionSelected = new PlayerInput(ConsoleKey.Spacebar);
            do
            {
                GameController.PrintGame();
                // PrintPlayersTurnDisplay();
                Hand.PrintHand();
                var actions = new Dictionary<PlayerInput, (string Label, Guid CallbackParam, Action<Guid> Callback)>();
                
                var allCardsInHand = Hand.GetAllCards();
                foreach (var playableCard in Hand.GetPlayableCards(Coins.CurrentValue))
                {
                    actions[Constants.HandCardKeys[allCardsInHand.FindIndex(x => x.CardId == playableCard.CardId)]] =
                        ($"Play {playableCard.GameToString()}", playableCard.CardId, Hand.PlayCard);
                }

                foreach (var attackableMinion in Board.GetMinionsThatCanAttack())
                {
                    actions[attackableMinion.Key] =
                        ($"Attack with {attackableMinion.Value.GameToString()}", OwnerId,
                            attackableMinion.Value.PromptAttackAndAttack);
                }
                
                actions.Add(Constants.EventHistoryKey, ("View history", Guid.NewGuid(), (x) =>
                {
                    Console.Clear();
                    GameController.HistoryLog.PrintNthEvents(40);
                    ColorConsole.WriteLine($"\nPress any key to return:");
                    Console.ReadKey();
                }
                ));

                actions.Add(Constants.EndTurnKey, ("End turn", Guid.NewGuid(), (x) => { }));

                actionSelected = Prompts.TurnActions(actions);
                actions[actionSelected].Callback(actions[actionSelected].CallbackParam);
            } while (!Equals(actionSelected, Constants.EndTurnKey));
        }
        
        public void Draw()
        {
            var card = Deck.Draw();
            if (Hand.GetAllCards().Count < Constants.MaxHandSize)
            {
                Hand.AddCard(card);
            }
        }

        public List<string> GetDrawToConsoleLines()
        {
            var health = Health.GameToStringValues();
            var attack = Attack.GameToStringValues();
            var attackText = attack.Value == "0" ? ColorConsole.FormatEmbeddedColor("__", Color) : ColorConsole.FormatEmbeddedColorPadRight(attack.Value, attack.Color, 2, '_', Color);
            var healthText = ColorConsole.FormatEmbeddedColorPadLeft(health.Value, health.Color, 2, '_', Color);
            var lines = new List<string>()
            {
                ColorConsole.FormatEmbeddedColor($"  ___  ", Color),
                ColorConsole.FormatEmbeddedColor($" /   \\ ", Color),
                ColorConsole.FormatEmbeddedColor($"|     |", Color),
                ColorConsole.FormatEmbeddedColor($"| P {_displayNumber} |", Color),
                ColorConsole.FormatEmbeddedColor($"|     |", Color),
                $"{ColorConsole.FormatEmbeddedColor("|", Color)}{attackText}{ColorConsole.FormatEmbeddedColor("_", Color)}{healthText}{ColorConsole.FormatEmbeddedColor("|", Color)}",
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

        public string GetNameColorFormatted()
        {
            return ColorConsole.FormatEmbeddedColor(Name, Color);
        }

        public override string GameToString()
        {
            var health = Health.GameToStringValues();
            var attack = Attack.GameToStringValues();
            var attackText = attack.Value == "0" ? string.Empty : $"{ColorConsole.FormatEmbeddedColor(attack.Value, attack.Color)}/";
            return $"[{GetNameColorFormatted()} ({attackText}{ColorConsole.FormatEmbeddedColor(health.Value, health.Color)})]";
        }

        public override string GameToString(Guid currentPlayerId)
        {
            var ownerPrefix = OwnerId == currentPlayerId ? $"{ColorConsole.FormatEmbeddedColor("Friend", ConsoleColor.Green)}" : $"{ColorConsole.FormatEmbeddedColor("Enemy", ConsoleColor.Red)}";
            return $"[{ownerPrefix} - {GameToString()}]";
        }

        public void PrintPlayersTurnDisplay()
        {
            var spacerLength = 25;
            ColorConsole.WriteEmbeddedColorLine(" _,.-'~'-.,__,.-'~'-.,__,.-'~'-.,__,.-'~'-.,__,.-'~'-.,__,.-'~'-.,_ ", Color);
            ColorConsole.WriteEmbeddedColorLine($"|                                                                  |", Color);
            ColorConsole.WriteEmbeddedColorLine($"|{new string(' ', spacerLength)}{ Name }'s Turn {new string(' ', spacerLength)}|", Color);
            ColorConsole.WriteEmbeddedColorLine($"|                                                                  |", Color);
            ColorConsole.WriteEmbeddedColorLine("|_,.-'~'-.,__,.-'~'-.,__,.-'~'-.,__,.-'~'-.,__,.-'~'-.,__,.-'~'-.,_|", Color);
        }

    }
}
