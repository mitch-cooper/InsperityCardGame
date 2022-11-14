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
        public void RemoveCard(string cardName);
        public void PlayCard(Guid cardId);
        public void ResetHand();
        public void PrintHand();
    }

    public class Hand : IHand, IConsoleDrawable
    {
        public int OwnerId { get; }
        protected List<Card> Cards { get; private set; }

        public Hand(int playerId)
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
            }
        }

        public void RemoveCard(string cardName)
        {
            var card = Cards.FirstOrDefault(x => x.Name == cardName);
            if (card != null)
            {
                Cards.Remove(card);
            }
        }

        public void PlayCard(Guid cardId)
        {
            var cardToPlay = Cards.SingleOrDefault(x => x.Id == cardId);
            if (cardToPlay == null)
            {
                throw new Exception("Card not in hand");
            }

            switch(cardToPlay)
            {
                case Minion m:
                    m.OnPlay(cardToPlay.OwnerId);
                    //m.
                    break;
                case Spell s:
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
            foreach (var card in Cards)
            {
                ColorConsole.WriteEmbeddedColorLine($"\t{card.GameToString()}");
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
