using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameState;
using GameState.Cards;
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
            var endTurnKey = Constants.EndTurnKey;
            do
            {
                GameController.PrintGame();
                //Hand.PrintHand();
                var actions = new Dictionary<PlayerInput, (string Label, Guid CallbackParam, Action<Guid> Callback)>();

                //var allCardsInHand = Hand.GetAllCards();
                //foreach (var playableCard in Hand.GetPlayableCards())
                //{
                //    actions[Constants.HandCardKeys[allCardsInHand.FindIndex(x => x.CardId == playableCard.CardId)]] =
                //        ($"Play {playableCard.GameToString()}", playableCard.CardId, Hand.PlayCard);
                //}
                
                foreach (var (cardInHand, index) in Hand.GetAllCards().WithIndex())
                {
                    var isPlayable = Hand.IsCardPlayable(cardInHand);
                    var methodCallback = isPlayable
                        ? (Action<Guid>)Hand.PlayCard
                        : Prompts.CardIsUnplayable;
                    var promptKey = Constants.HandCardKeys[index];
                    promptKey.Color = isPlayable ? Constants.ActionColor : Constants.InActionColor;
                    actions[promptKey] = (cardInHand.GameToString(), cardInHand.CardId, methodCallback);
                }

                foreach (var attackableMinion in Board.GetMinionsThatCanAttack())
                {
                    var attackableMinionKey = attackableMinion.Key;
                    attackableMinionKey.Color = Constants.ActionColor;
                    actions[attackableMinionKey] =
                        ($"Attack with {attackableMinion.Value.GameToString()}", OwnerId,
                            attackableMinion.Value.PromptAttackAndAttack);
                }
                
                actions.Add(Constants.EventHistoryKey, ("View history", Guid.NewGuid(), Prompts.HistoryView));

                endTurnKey.Color = actions.Keys.All(x => x.Color != Constants.ActionColor) ? Constants.ActionColor : Constants.SecondaryActionColor;
                actions.Add(endTurnKey, ("End turn", Guid.NewGuid(), (x) => { }));

                actionSelected = Prompts.TurnActions(actions);
                actions[actionSelected].Callback(actions[actionSelected].CallbackParam);
            } while (!Equals(actionSelected, endTurnKey));
        }
        
        public void Draw()
        {
            var card = Deck.Draw();
            Hand.DrawCard(card);
        }

        public void AddCardFromDeckToHand()
        {
            var card = Deck.Draw();
            Hand.AddCard(card);
        }

        public List<string> GetDrawToConsoleLines()
        {
            var health = Health.GameToStringValues();
            var attack = Attack.GameToStringValues();
            var borderColor = HasAttribute(BoardCharacterAttribute.Frozen) ? Constants.FrozenColor : Color;

            var attackText = attack.Value == "0" ? ColorConsole.FormatEmbeddedColor("__", borderColor) : ColorConsole.FormatEmbeddedColorPadRight(attack.Value, attack.Color, 2, '_', borderColor);
            var healthText = ColorConsole.FormatEmbeddedColorPadLeft(health.Value, health.Color, 3, '_', borderColor);

            var canAttackColor = IsMyTurn && CanAttack() ? Constants.ActionColor : ConsoleColor.White;
            var inputPrompt = IsMyTurn ? Constants.CurrentPlayerKey : Constants.OpponentPlayerKey;

            var lines = new List<string>()
            {
                ColorConsole.FormatEmbeddedColor($"   ___   ", borderColor),
                ColorConsole.FormatEmbeddedColor($"  /   \\  ", borderColor),
                ColorConsole.FormatEmbeddedColor($" |     | ", borderColor),
                ColorConsole.FormatEmbeddedColor($" | P {_displayNumber} | ", borderColor),
                ColorConsole.FormatEmbeddedColor($" |     | ", borderColor),
                $" {ColorConsole.FormatEmbeddedColor("|", borderColor)}{attackText}{healthText}{ColorConsole.FormatEmbeddedColor("|", borderColor)} ",
                ColorConsole.FormatEmbeddedColor("\\_______/", canAttackColor),
                ColorConsole.FormatEmbeddedColor($"    {inputPrompt.ToString().PadRight(5, ' ')}", canAttackColor)
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
