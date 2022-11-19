using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameState.Cards.Collection;
using GameState.GameRules;

namespace GameState.GameRules
{
    internal class TurnSystem : IGameEventEmitter
    {
        public int TurnCount { get; private set; } = 0;
        public Player GoesFirst { get; private set; }
        public Player GoesSecond { get; private set; }
        public Player CurrentTurnsPlayer { get; private set; }
        public event IGameEventEmitter.GameEventHandler TurnStartTriggered;
        public event IGameEventEmitter.GameEventHandler TurnEndTriggered;

        public void ResetTurns(Player player1, Player player2)
        {
            TurnCount = 0;
            TurnStartTriggered = null;
            TurnEndTriggered = null;
            InitializePlayers(player1, player2);
        }

        private void InitializePlayers(Player player1, Player player2)
        {
            var randomNumber = new Random().Next(2);

            GoesFirst = randomNumber == 1 ? player1 : player2;
            GoesSecond = randomNumber == 1 ? player2 : player1;

            GoesFirst.AddCardFromDeckToHand();
            GoesFirst.AddCardFromDeckToHand();
            GoesFirst.AddCardFromDeckToHand();

            GoesSecond.AddCardFromDeckToHand();
            GoesSecond.AddCardFromDeckToHand();
            GoesSecond.AddCardFromDeckToHand();
            GoesSecond.AddCardFromDeckToHand();

            // TODO: implement mulligan?

            GoesSecond.Hand.AddCard(NonCollectibleCards.Money().Build(GoesSecond.OwnerId));
        }

        public void StartTurn(Player player)
        {
            CurrentTurnsPlayer = player;
            if (player.OwnerId == GoesFirst.OwnerId) TurnCount++;
            player.IsMyTurn = true;
            player.Draw();
            player.Coins.AddToBaseValue(1);
            player.Coins.AddToCurrentValue(player.Coins.BaseValue - player.Coins.CurrentValue);
            
            // TODO: Can remove later, handled by minion event subscription
            //var minions = player.Board.GetAllMinions().ToList();
            //minions.ForEach(x => x.AttacksThisTurn = 0);

            TurnStartTriggered?.Invoke(new GameEvent(player, GameEventType.TurnStart, $"{TurnCount.GetOrdinalSuffix()} turn has started."));

            player.PromptTurnActions();
        }

        public void EndTurn(Player player)
        {
            player.IsMyTurn = false;

            // TODO: Can remove later, handled by minion event subscription
            //var minions = player.Board.GetAllMinions().Where(x => x.SleepTurnTimer > 0).ToList();
            //minions.ForEach(x => x.SleepTurnTimer--);

            TurnEndTriggered?.Invoke(new GameEvent(player, GameEventType.TurnEnd, $"{TurnCount.GetOrdinalSuffix()} turn has ended."));
        }
    }
}
