using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameState.GameRules;

namespace GameState
{
    public class PlayerBoard : IPlayerBoard, IGameEventEmitter, IConsoleDrawable, IOwnable
    {
        public Guid OwnerId { get; }
        protected List<Minion> Minions { get; private set; }
        public event IGameEventEmitter.GameEventHandler MinionSummonTriggered;
        public event IGameEventEmitter.GameEventHandler MinionAttackTriggered;
        public event IGameEventEmitter.GameEventHandler MinionDeathTriggered;
        public event IGameEventEmitter.GameEventHandler MinionRemovedFromBoardTriggered;
        public event IGameEventEmitter.GameEventHandler MinionAddedToBoardTriggered;

        public PlayerBoard(Guid playerId)
        {
            OwnerId = playerId;
            ResetBoard();
        }

        public List<Minion> GetAllMinions()
        {
            return Minions;
        }

        public Dictionary<PlayerInput, Minion> GetMinionsThatCanAttack()
        {
            var allMinions = new Dictionary<PlayerInput, Minion>();
            foreach (var (minion, index) in GetAllMinions().WithIndex())
            {
                allMinions[Constants.CurrentPlayersMinionKeys[index]] = minion;
            }
            return new Dictionary<PlayerInput, Minion>(allMinions.Where(x => x.Value.CanAttack()));
        }

        public Dictionary<PlayerInput, BoardCharacter> GetAttackableTargets()
        {
            return TargetMatcher.GetTargets(GameController.GetOpponent(OwnerId).OwnerId, TargetCategory.Enemies);
        }

        public void ResetBoard()
        {
            Minions = new List<Minion>();
            MinionSummonTriggered = null;
            MinionAttackTriggered = null;
            MinionDeathTriggered = null;
        }

        public bool HasMaxMinions()
        {
            return Minions.Count == Constants.MaxMinions;
        }

        public void SummonMinion(Minion minion)
        {
            if (!HasMaxMinions())
            {
                AddMinionToBoard(minion);
                minion.Summon();
                MinionSummonTriggered?.Invoke(new GameEvent(minion, GameEventType.MinionSummon, $"{minion.GameToString()} was summoned."));
            }
        }

        private void MinionDiedOnBoard(GameEvent gameEvent)
        {
            var minion = (Minion)gameEvent.Entity;
            RemoveMinionFromBoard(minion);
            MinionDeathTriggered?.Invoke(gameEvent);
        }

        public void AddMinionToBoard(Minion minion)
        {
            if (!HasMaxMinions())
            {
                minion.OwnerId = OwnerId;
                Minions.Add(minion);
                MinionAddedToBoardTriggered?.Invoke(new GameEvent(minion, GameEventType.MinionAddedToBoard,
                    $"{minion.GameToString()} was added to player board."));

                minion.AttackTriggered += (gameEvent) =>
                {
                    MinionAttackTriggered?.Invoke(gameEvent);
                };
                minion.DeathTriggered += MinionDiedOnBoard;
            }
        }

        public void RemoveMinionFromBoard(Minion minion)
        {
            Minions.Remove(minion);
            MinionRemovedFromBoardTriggered?.Invoke(new GameEvent(minion, GameEventType.MinionRemovedFromBoard, 
                $"{minion.GameToString()} was removed from player board."));
        }

        public List<string> GetDrawToConsoleLines()
        {
            var lines = GameToConsoleHelper.InitializeListWithValue("", 8);
            var isMyTurn = GameController.GetPlayer(OwnerId).IsMyTurn;

            foreach (var minion in Minions)
            {
                var minionLines = minion.GetDrawToConsoleLines().Select(x => $" {x}  ").ToList();
                for (int i = 0; i < minionLines.Count; i++)
                {
                    lines[i] += minionLines[i];
                }
            }

            for (int i = Minions.Count; i < Constants.MaxMinions; i++)
            {
                for (int j = 0; j < lines.Count - 2; j++)
                {
                    lines[j] += new string(' ', 10);
                }
            }

            var minionKeys = isMyTurn ? Constants.CurrentPlayersMinionKeys : Constants.OpponentsMinionKeys;
            for (int i = 0; i < Constants.MaxMinions; i++)
            {
                var canAttack = isMyTurn && i < Minions.Count && Minions[i].CanAttack();
                lines[^2] += ColorConsole.FormatEmbeddedColor("\\_______/ ", canAttack ? Constants.ActionColor : ConsoleColor.White);
                lines[^1] += ColorConsole.FormatEmbeddedColor($"   {minionKeys[i].ToString().PadRight(7, ' ')}", canAttack ? Constants.ActionColor : ConsoleColor.White);
            }

            return lines;
        }
    }
}
