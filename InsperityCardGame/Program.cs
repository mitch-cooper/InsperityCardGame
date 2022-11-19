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
            var player1 = new Player(player1Id, "Player 1", "1",
                ConsoleColor.DarkMagenta, PremadeDecks.BenefitsDeck1(player1Id));

            var player2Id = Guid.NewGuid();
            var player2 = new Player(player2Id, "Player 2", "2",
                ConsoleColor.DarkCyan, PremadeDecks.PayrollDeck1(player2Id));

            GameController.StartGame(player1, player2);
        }
    }
}
