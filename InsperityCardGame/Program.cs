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
            var player1 = new Player(player1Id, "Player 1", "1", new Deck(player1Id, new List<Card>()
            {
                BenefitsCards.NewHire().Build(player1Id),
                BenefitsCards.NewHire().Build(player1Id),
                BenefitsCards.NewHire().Build(player1Id),
                BenefitsCards.NewHire().Build(player1Id),
                NeutralCards.Employee().Build(player1Id),
                NeutralCards.Employee().Build(player1Id),
                NeutralCards.Employee().Build(player1Id),
                NeutralCards.Employee().Build(player1Id),
                NeutralCards.Employee().Build(player1Id),
                NeutralCards.Employee().Build(player1Id),
                BenefitsCards.HSAContributer().Build(player1Id),
                BenefitsCards.HSAContributer().Build(player1Id),
                BenefitsCards.HSAContributer().Build(player1Id),
                BenefitsCards.HSAContributer().Build(player1Id),
                NeutralCards.Boss().Build(player1Id),
                NeutralCards.Boss().Build(player1Id),
                NeutralCards.Boss().Build(player1Id),
                NeutralCards.Boss().Build(player1Id),
                NeutralCards.Boss().Build(player1Id)
            }));

            var player2Id = Guid.NewGuid();
            var player2 = new Player(player2Id, "Player 2", "2", new Deck(player2Id, new List<Card>()
            {
                PayrollCards.Paycheck().Build(player2Id),
                PayrollCards.Paycheck().Build(player2Id),
                PayrollCards.Paycheck().Build(player2Id),
                NeutralCards.Employee().Build(player2Id),
                NeutralCards.Employee().Build(player2Id),
                NeutralCards.Employee().Build(player2Id),
                NeutralCards.Employee().Build(player2Id),
                NeutralCards.Employee().Build(player2Id),
                PayrollCards.Healthcare().Build(player2Id),
                PayrollCards.Healthcare().Build(player2Id),
                PayrollCards.Healthcare().Build(player2Id),
                NeutralCards.Boss().Build(player2Id),
                NeutralCards.Boss().Build(player2Id),
                NeutralCards.Boss().Build(player2Id),
                NeutralCards.Boss().Build(player2Id),
                NeutralCards.Boss().Build(player2Id),
                PayrollCards.Payroller().Build(player2Id),
                PayrollCards.Payroller().Build(player2Id),
                PayrollCards.Payroller().Build(player2Id),
                PayrollCards.Payroller().Build(player2Id),
                PayrollCards.Payroller().Build(player2Id)
            }));

            GameController.StartGame(player1, player2);
        }
    }
}
