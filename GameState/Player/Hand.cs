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
        public List<Card> GetPlayableCards(int coinsAvailable);
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

        public List<Card> GetPlayableCards(int coinsAvailable)
        {
            return Cards.Where(x => x.Cost.CurrentValue <= coinsAvailable).ToList();
        }

        public void AddCard(Card card)
        {
            if (Cards.Count < Constants.MaxHandSize)
            {
                Cards.Add(card);
                switch (card)
                {
                    case Minion m:
                        m.OnDraw(OwnerId);
                        break;
                    case Spell s:
                        s.OnDraw(s, OwnerId);
                        break;
                }
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

            player.Coins.AddToCurrentValue(-1 * cardToPlay.Cost.CurrentValue);
            switch (cardToPlay)
            {
                case Minion m:
                    m.OnPlay(m.OwnerId);
                    RemoveCard(m.CardId);
                    MinionPlayTriggered?.Invoke(new GameEvent(this, GameEventType.MinionPlay, $"{m.Name} was played."));
                    player.Board.SummonMinion(m);
                    break;
                case Spell s:
                    s.OnPlay(s, s.OwnerId);
                    RemoveCard(s.CardId);
                    SpellPlayTriggered?.Invoke(new GameEvent(this, GameEventType.SpellPlay, $"{s.Name} was played."));
                    break;
            }
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
            };
            for (int i = 0; i < Cards.Count; i++)
            {
                for (int j = 0; j < lines.Count; j++)
                {
                    if (i == 0)
                    {
                        lines[j] = string.Empty;
                    }
                    if (j == 0)
                    {
                        lines[j] += " __";
                    }
                    else if (j < lines.Count - 1)
                    {
                        lines[j] += "|  ";
                    }
                    else
                    {
                        lines[j] += "|__";
                    }
                }

                if (i == Cards.Count - 1)
                {
                    for (int j = 0; j < lines.Count; j++)
                    {
                        if (j == 0)
                        {
                            lines[j] += "___ ";
                        }
                        else if (j == 1)
                        {
                            lines[j] += $"{Cards.Count}  |";
                        }
                        else if (j < lines.Count - 1)
                        {
                            lines[j] += "   |";
                        }
                        else
                        {
                            lines[j] += "___|";
                        }
                    }
                }
            }
            return lines;
        }
    }
}
