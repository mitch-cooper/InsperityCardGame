using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameState.GameRules;

namespace GameState
{
    public interface IPlayerBoard
    {
        List<Minion> GetAllMinions();
        Dictionary<ConsoleKey, Minion> GetMinionsThatCanAttack();
        Dictionary<ConsoleKey, IBoardItem> GetAttackableTargets();
        void ResetBoard();
        bool HasMaxMinions();
    }
    public class PlayerBoard : IPlayerBoard, IConsoleDrawable
    {
        public int OwnerId { get; }
        protected List<Minion> Minions { get; private set; }

        public PlayerBoard(int playerId)
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
            boardItems[Constants.OpponentPlayerKey] = GameState.GetPlayer(OwnerId);
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

        public List<string> GetDrawToConsoleLines()
        {
            var lines = GameToConsoleHelper.InitializeListWithValue("", 8);
            var isMyTurn = GameState.GetPlayer(OwnerId).IsMyTurn;

            foreach (var minion in Minions)
            {
                var minionLines = minion.GetDrawToConsoleLines();
                minionLines.ForEach(x =>
                {
                    x = $" {x}  ";
                });
                for (int i = 0; i < lines.Count - 3; i++)
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
