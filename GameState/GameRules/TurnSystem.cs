using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameState.Cards.Collection;

namespace GameState.GameRules
{
    internal class TurnSystem
    {
        public int TurnCount { get; private set; } = 0;
        public Player GoesFirst { get; private set; }
        public Player GoesSecond { get; private set; }

        public void ResetTurns(Player player1, Player player2)
        {
            TurnCount = 0;
            InitializePlayers(player1, player2);
        }

        private void InitializePlayers(Player player1, Player player2)
        {
            var randomNumber = new Random().Next(2);

            GoesFirst = randomNumber == 1 ? player1 : player2;
            GoesSecond = randomNumber == 1 ? player2 : player1;

            GoesFirst.Draw();
            GoesFirst.Draw();
            GoesFirst.Draw();

            GoesSecond.Draw();
            GoesSecond.Draw();
            GoesSecond.Draw();
            GoesSecond.Draw();
            GoesSecond.Hand.AddCard(NonCollectibleCards.Money().Build(GoesSecond.OwnerId));
        }

        public void StartTurn(Player player)
        {
            if(player.OwnerId == GoesFirst.OwnerId) TurnCount++;
            player.IsMyTurn = true;
            player.Draw();
            player.Coins.AddToBaseValue(1);
            player.Coins.AddToCurrentValue(player.Coins.BaseValue - player.Coins.CurrentValue);
            var minions = player.Board.GetAllMinions().ToList();
            minions.ForEach(x => x.AttacksThisTurn = 0);

            player.PromptTurnActions();

            EndTurn(player);
        }

        public void EndTurn(Player player)
        {
            player.IsMyTurn = false;
            var minions = player.Board.GetAllMinions().Where(x => x.SleepTurnTimer > 0).ToList();
            minions.ForEach(x => x.SleepTurnTimer--);
        }
    }
}
