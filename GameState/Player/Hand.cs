using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameState.GameRules;

namespace GameState
{
    public interface IHand
    {
        public List<Card> GetAllCards();
        public List<Card> GetPlayableCards();
        public bool IsCardPlayable(Card card);
        public void DrawCard(Card card);
        public void AddCard(Card card);
        public void RemoveCard(Guid cardId);
        public void PlayCard(Guid cardId);
        public void ResetHand();
        public void PrintHand();
    }

    public class Hand : IHand, IGameEventEmitter, IConsoleDrawable, IOwnable
    {
        public Guid OwnerId { get; }
        protected List<Card> Cards { get; private set; }
        public event IGameEventEmitter.GameEventHandler MinionPlayTriggered;
        public event IGameEventEmitter.GameEventHandler SpellPlayTriggered;

        public Hand(Guid playerId)
        {
            OwnerId = playerId;
            ResetHand();
        }

        public List<Card> GetAllCards()
        {
            return Cards;
        }

        public List<Card> GetPlayableCards()
        {
            return Cards.Where(IsCardPlayable).ToList();
        }

        public bool IsCardPlayable(Card card)
        {
            var player = GameController.GetPlayer(OwnerId);
            return Cards.Exists(x => x.CardId == card.CardId)
                && card.Cost.CurrentValue <= player.Coins.CurrentValue
                && (!(card is Minion) || card is Minion m && !player.Board.HasMaxMinions() && m.IsCardPlayable())
                && (!(card is Spell) || card is Spell s && s.IsCardPlayable());
        }

        public void DrawCard(Card card)
        {
            if (Cards.Count < Constants.MaxHandSize)
            {
                Cards.Add(card);
                switch (card)
                {
                    case Minion m:
                        m.OnDraw(m, OwnerId);
                        break;
                    case Spell s:
                        s.OnDraw(s, OwnerId);
                        break;
                }
            }
        }

        public void AddCard(Card card)
        {
            if (Cards.Count < Constants.MaxHandSize)
            {
                Cards.Add(card);
            }
        }

        public void RemoveCard(Guid cardId)
        {
            var card = Cards.FirstOrDefault(x => x.CardId == cardId);
            if (card != null)
            {
                Cards.Remove(card);
            }
        }

        public void PlayCard(Guid cardId)
        {
            var cardToPlay = Cards.SingleOrDefault(x => x.CardId == cardId);
            if (cardToPlay == null)
            {
                throw new Exception("Card not in hand");
            }

            var player = GameController.GetPlayer(cardToPlay.OwnerId);
            if (cardToPlay.Cost.CurrentValue > player.Coins.CurrentValue)
            {
                throw new Exception("Not enough coins");
            }

            if (!cardToPlay.CanProceedWithOnPlay())
            {
                return;
            }

            switch (cardToPlay)
            {
                case Minion m:
                    m.OnPlay(m, m.OwnerId);
                    RemoveCard(m.CardId);
                    MinionPlayTriggered?.Invoke(new GameEvent(m, GameEventType.MinionPlay, $"{m.Name} was played."));
                    player.Board.SummonMinion(m);
                    break;
                case Spell s:
                    s.OnPlay(s, s.OwnerId);
                    RemoveCard(s.CardId);
                    SpellPlayTriggered?.Invoke(new GameEvent(s, GameEventType.SpellPlay, $"{s.Name} was played."));
                    break;
            }
            player.Coins.AddToCurrentValue(-1 * cardToPlay.Cost.CurrentValue);
        }

        public void ResetHand()
        {
            Cards = new List<Card>();
        }

        public void PrintHand()
        {
            ColorConsole.WriteLine("Hand: ");
            foreach (var (card, index) in Cards.WithIndex())
            {
                ColorConsole.WriteEmbeddedColorLine($"\t{index + 1}: {card.GameToString()}");
            }
        }

        public List<string> GetDrawToConsoleLines()
        {
            var lines = new List<string>()
            {
                "       ",
                "  No   ",
                " cards ",
                "   in  ",
                "  hand ",
                "       ",
                "       ",
                "       ",
            };
            var owner = GameController.GetPlayer(OwnerId);
            foreach (var (card, i) in Cards.WithIndex())
            {
                var borderColor = !owner.IsMyTurn ? ConsoleColor.White
                    : (ConsoleColor) card.Rarity;
                var cardIndexColor = !owner.IsMyTurn ? ConsoleColor.White
                    : IsCardPlayable(card)
                        ? Constants.ActionColor
                        : Constants.InActionColor;

                for (int j = 0; j < lines.Count; j++)
                {
                    if (i == 0)
                    {
                        lines[j] = string.Empty;
                    }
                    if (j == 0)
                    {
                        lines[j] += ColorConsole.FormatEmbeddedColor(" __", borderColor);
                    }
                    else if (j == 1)
                    {
                        var costSlotString = owner.IsMyTurn
                            ? ColorConsole.FormatEmbeddedColorPadRight(card.Cost.GameToStringValues().Value, card.Cost.GameToStringValues().Color, 2, ' ')
                            : ColorConsole.FormatEmbeddedColor("  ", ConsoleColor.White);
                        lines[j] += $"{ColorConsole.FormatEmbeddedColor("|", borderColor)}{costSlotString}";
                    }
                    else if (j == 3)
                    {
                        var handIndexString = i == Cards.Count - 1
                            ? "  "
                            : ColorConsole.FormatEmbeddedColorPadRight((i + 1).ToString(), cardIndexColor, 2, ' ');
                        lines[j] += $"{ColorConsole.FormatEmbeddedColor("|", borderColor)}{handIndexString}";
                    }
                    else if (j == 2 || (j > 3 && j < 5))
                    {
                        lines[j] += ColorConsole.FormatEmbeddedColor("|  ", borderColor);
                    }
                    else if (j == 5)
                    {
                        lines[j] += ColorConsole.FormatEmbeddedColor("|__", borderColor);
                    }
                }

                if (i == Cards.Count - 1)
                {
                    for (int j = 0; j < lines.Count; j++)
                    {
                        if (j == 0)
                        {
                            lines[j] += ColorConsole.FormatEmbeddedColor("___ ", borderColor);
                        }
                        else if (j == 1 || j == 2 || j == 4)
                        {
                            lines[j] += $"{ColorConsole.FormatEmbeddedColor("   |", borderColor)}";
                        }
                        else if (j == 3)
                        {
                            lines[j] += $"{ColorConsole.FormatEmbeddedColorPadRight((i + 1).ToString(), cardIndexColor, 3, ' ')}{ColorConsole.FormatEmbeddedColor("|", borderColor)}";
                        }
                        else if (j == 5)
                        {
                            lines[j] += ColorConsole.FormatEmbeddedColor("___|", borderColor);
                        }
                    }
                }
            }
            return lines;
        }
    }
}
