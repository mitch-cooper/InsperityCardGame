using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameState.Cards.Collection;
using GameState.GameRules;

namespace GameState
{
    public interface IDeck : IConsoleDrawable
    {
        List<Card> StartingCards { get; }
        List<Card> CurrentCards { get; }
        void ResetDeck();
        void Shuffle();
        void AddCard(Card card);
        void AddCards(List<Card> cards);
        Card Draw();
    }

    public class Deck : IDeck, IOwnable
    {
        public Guid OwnerId { get; private set; }
        public List<Card> StartingCards { get; private set; }
        public List<Card> CurrentCards { get; private set; }
        private int FatigueCount = 0;

        public Deck(Guid playerId, List<Card> cards)
        {
            OwnerId = playerId;
            StartingCards = new List<Card>(cards?.Take(Constants.BaseDeckSize));
            ResetDeck();
        }

        public void ResetDeck()
        {
            CurrentCards = new List<Card>(StartingCards);
            Shuffle();
            FatigueCount = 0;
        }

        public void Shuffle()
        {
            CurrentCards = CurrentCards.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public void AddCard(Card card)
        {
            CurrentCards.Add(card);
        }

        public void AddCards(List<Card> cards)
        {
            CurrentCards.AddRange(cards);
        }

        public Card Draw()
        {
            if (CurrentCards.Count > 0)
            {
                var topCard = CurrentCards.First();
                CurrentCards.RemoveAt(0);
                return topCard;
            }

            var fatigueCard = NonCollectibleCards.FatigueBuilder()
                .AddSpellValues(new List<int> { ++FatigueCount })
                .Build(OwnerId);
            return fatigueCard;
        }

        public List<string> GetDrawToConsoleLines()
        {
            var cardCountDisplayColor = CurrentCards.Count > 10 ? ConsoleColor.Green
                : CurrentCards.Count > 0 ? ConsoleColor.DarkYellow
                    : ConsoleColor.Red;
            var cardCountDisplay = ColorConsole.FormatEmbeddedColorPadLeft(CurrentCards.Count.ToString(), cardCountDisplayColor, 2, ' ');
            var maxAdditionalCardsToShowInDeck = 3;
            var deckVisualDisplayTop = new string('_', Math.Min(maxAdditionalCardsToShowInDeck, Math.Max(CurrentCards.Count - 1, 0))).PadLeft(maxAdditionalCardsToShowInDeck, ' ');
            if (CurrentCards.Count <= 1)
            {
                // TODO: fix last character of top of deck
                //deckVisualDisplayTop.Remove()
            }
            var deckVisualDisplay = new string('|', Math.Min(maxAdditionalCardsToShowInDeck, Math.Max(CurrentCards.Count - 1, 0))).PadLeft(maxAdditionalCardsToShowInDeck, ' ');
            var lines = new List<string>()
            {
                $"Deck:{cardCountDisplay}",
                $"{deckVisualDisplayTop}______",
                $"{deckVisualDisplay}|     |",
                $"{deckVisualDisplay}|     |",
                $"{deckVisualDisplay}|     |",
                $"{deckVisualDisplay}|     |",
                $"{deckVisualDisplay}|_____|",
                $"       "
            };

            if (CurrentCards.Count == 0)
            {
                for (var i = 1; i < lines.Count; i++)
                {
                    lines[i] = ColorConsole.FormatEmbeddedColor(lines[i], ConsoleColor.Red);
                }
            }

            return lines;
        }
    }
}
