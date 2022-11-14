using System;
using System.Collections.Generic;
using System.Text;
using GameState.GameRules;

namespace GameState
{
    public abstract class Card : IConsoleGameToString, IOwnable
    {
        public Guid Id { get; protected set; }
        public int OwnerId { get; protected set; }
        public string Name { get; protected set; }
        public string Text { get; protected set; }
        public Rarity Rarity { get; protected set; }
        public CostValueState Cost { get; protected set; }
        public Action<int> OnPlay { get; protected set; }
        //public delegate void OnPlay(int playerId);
        //public event OnPlay CardPlayed;
        public Action<int> OnDraw { get; protected set; }

        protected Card()
        {
            Id = Guid.NewGuid();
            Cost = new CostValueState(0);
        }

        protected Card(int ownerId, string name, string text, Rarity rarity, int cost) : this()
        {
            OwnerId = ownerId;
            Name = name;
            Text = text;
            Rarity = rarity;
            Cost = new CostValueState(cost);
        }

        //public void Play()
        //{
        //    CardPlayed?.Invoke(OwnerId);
        //}

        //protected abstract void StuffThatShouldHappenOnEveryPlay();

        public abstract string GameToString();

        public string GameToString(int currentPlayerId)
        {
            var ownerPrefix = OwnerId == currentPlayerId ? $"{ColorConsole.FormatEmbeddedColor("Friend", ConsoleColor.Green)}" : $"{ColorConsole.FormatEmbeddedColor("Enemy", ConsoleColor.Red)}";
            return $"[{ownerPrefix} - {GameToString().TrimStart('[')}";
        }

        public void PrintGameToString()
        {
            ColorConsole.WriteEmbeddedColorLine(GameToString());
        }
        public void PrintGameToString(int currentPlayerId)
        {
            ColorConsole.WriteEmbeddedColorLine(GameToString(currentPlayerId));
        }
    }

    public enum Rarity
    {
        Common = ConsoleColor.DarkGray,
        Rare = ConsoleColor.Blue,
        Epic = ConsoleColor.Magenta,
        Legendary = ConsoleColor.Yellow
    }
}
