using System;
using System.Collections.Generic;
using GameState;
using GameState.Cards.Collection;

namespace InsperityCardGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var player1Id = Guid.NewGuid();
            var player1 = new Player(player1Id, "Player 1", "1", new Deck(new List<Card>()
            {
                NeutralCards.Employee().Build(player1Id),
                NeutralCards.Employee().Build(player1Id),
                NeutralCards.Employee().Build(player1Id),
                NeutralCards.Employee().Build(player1Id),
                NeutralCards.Employee().Build(player1Id),
                NeutralCards.Employee().Build(player1Id),
                NeutralCards.Employee().Build(player1Id),
                NeutralCards.Employee().Build(player1Id),
                NeutralCards.Employee().Build(player1Id),
                NeutralCards.Boss().Build(player1Id),
                NeutralCards.Boss().Build(player1Id),
                NeutralCards.Boss().Build(player1Id),
                NeutralCards.Boss().Build(player1Id),
                NeutralCards.Boss().Build(player1Id),
                NeutralCards.Boss().Build(player1Id),
                NeutralCards.Boss().Build(player1Id),
                NeutralCards.Boss().Build(player1Id)
            }, player1Id));

            var player2Id = Guid.NewGuid();
            var player2 = new Player(player2Id, "Player 2", "2", new Deck(new List<Card>()
            {
                NeutralCards.Employee().Build(player2Id),
                NeutralCards.Employee().Build(player2Id),
                NeutralCards.Employee().Build(player2Id),
                NeutralCards.Employee().Build(player2Id),
                NeutralCards.Employee().Build(player2Id),
                NeutralCards.Employee().Build(player2Id),
                NeutralCards.Employee().Build(player2Id),
                NeutralCards.Boss().Build(player2Id),
                NeutralCards.Boss().Build(player2Id),
                NeutralCards.Boss().Build(player2Id),
                NeutralCards.Boss().Build(player2Id),
                NeutralCards.Boss().Build(player2Id),
                NeutralCards.Payroller().Build(player2Id),
                NeutralCards.Payroller().Build(player2Id),
                NeutralCards.Payroller().Build(player2Id),
                NeutralCards.Payroller().Build(player2Id),
                NeutralCards.Payroller().Build(player2Id)
            }, player2Id));

            GameState.GameController.StartGame(player1, player2);
        }
    }
}
