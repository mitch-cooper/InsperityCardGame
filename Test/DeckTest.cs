using System.Collections.Generic;
using GameState;
using GameState.Cards.Collection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class DeckTest
    {
        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public void Draw_RemovesACardFromDeck()
        {
            // Arrange
            var playerId = 1;
            var sut = new Deck(new List<Card>()
            {
                NeutralCards.Employee().Build(playerId),
                NeutralCards.Employee().Build(playerId),
                NeutralCards.Boss().Build(playerId)
            }, playerId);

            // Act
            var card = sut.Draw();

            // Assert
            Assert.AreEqual(2, sut.CurrentCards.Count);
        }

        [TestMethod]
        public void Draw_WithEmptyDeck_DrawsFatigueCard()
        {
            // Arrange
            var playerId = 1;
            var sut = new Deck(new List<Card>(), playerId);

            // Act
            var card = sut.Draw();

            // Assert
            Assert.AreEqual(0, sut.CurrentCards.Count);
            Assert.AreEqual(NonCollectibleCards.FatigueBuilder().Build(playerId).Name, card.Name);
        }

        [TestMethod]
        public void Draw_WithEmptyDeck_DrawsFatigueCardWithIncreasingDamage()
        {
            // Arrange
            var playerId = 1;
            var sut = new Deck(new List<Card>(), playerId);
            var fatigueDraws = new List<Spell>();
            var iterations = 5;

            // Act
            for (var i = 0; i < iterations; i++)
            {
                fatigueDraws.Add(sut.Draw() as Spell);
            }

            // Assert
            Assert.AreEqual(0, sut.CurrentCards.Count);
            for (int i = 0; i < iterations; i++)
            {
                Assert.IsTrue(fatigueDraws[i].DisplayText.StartsWith($"Deal {i + 1} damage to yourself."));
            }
        }
    }
}
