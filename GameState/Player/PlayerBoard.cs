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
        public event IGameEventEmitter.GameEventHandler GameEventTriggered;

        public PlayerBoard(Guid playerId)
        {
            OwnerId = playerId;
            ResetBoard();
        }
        public List<Minion> GetAllMinions()
        {
            return Minions;
        }

        public Dictionary<ConsoleKey, Minion> GetMinionsThatCanAttack()
        {
            var allMinions = new Dictionary<ConsoleKey, Minion>();
            foreach (var (minion, index) in GetAllMinions().WithIndex())
            {
                allMinions[Constants.CurrentPlayersMinionKeys[index]] = minion;
            }
            return new Dictionary<ConsoleKey, Minion>(allMinions.Where(x => x.Value.CanAttack()));
        }

        public Dictionary<ConsoleKey, IBoardItem> GetAttackableTargets()
        {
            // TODO: implement taunt
            var boardItems = new Dictionary<ConsoleKey, IBoardItem>();
            foreach (var (minion, index) in GetAllMinions().WithIndex())
            {
                boardItems[Constants.OpponentsMinionKeys[index]] = minion;
            }
            boardItems[Constants.OpponentPlayerKey] = GameController.GetPlayer(OwnerId);
            return boardItems;
        }

        public void ResetBoard()
        {
            Minions = new List<Minion>();
        }

        public bool HasMaxMinions()
        {
            return Minions.Count == Constants.MaxMinions;
        }

        public void SummonMinion(Minion minion)
        {
            if (!HasMaxMinions())
            {
                Minions.Add(minion);
                GameEventTriggered?.Invoke(new GameEvent(minion, GameEventType.MinionSummon, $"{minion.Name} summoned."));
            }
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
                lines[^2] += ColorConsole.FormatEmbeddedColor("\\_______/ ", canAttack ? ConsoleColor.Green : ConsoleColor.White);
                lines[^1] += ColorConsole.FormatEmbeddedColor($"   {minionKeys[i].ToString().PadRight(7, ' ')}", canAttack ? ConsoleColor.Green : ConsoleColor.White);
            }

            return lines;
        }
    }
}
