using System;
using System.Collections.Generic;
using System.Text;
using GameState.GameRules;

namespace GameState
{
    public interface ICard/*<in T>*/ : IConsoleGameToString, IOwnable
    {
        Guid CardId { get; }
        string Name { get; }
        string Text { get; }
        Rarity Rarity { get; }
        CostValueState Cost { get; }
        Action</*T, */Guid> OnPlay { get; }
        Action</*T, */Guid> OnDraw { get; }
    }

    public abstract class Card : ConsoleGameToString, ICard//<Card>
    {
        public Guid CardId { get; protected set; }
        public Guid OwnerId { get; protected set; }
        public string Name { get; protected set; }
        public string Text { get; protected set; }
        public Rarity Rarity { get; protected set; }
        public CostValueState Cost { get; protected set; }
        public Action</*Card, */Guid> OnPlay { get; protected set; }
        public Action</*Card, */Guid> OnDraw { get; protected set; }

        protected Card()
        {
            CardId = Guid.NewGuid();
            Cost = new CostValueState(0);
        }

        protected Card(Guid ownerId, string name, string text, Rarity rarity, CostValueState cost) : this()
        {
            OwnerId = ownerId;
            Name = name;
            Text = text;
            Rarity = rarity;
            Cost = cost;
        }

        //public abstract string GameToString();

        public override string GameToString(Guid currentPlayerId)
        {
            var ownerPrefix = OwnerId == currentPlayerId ? $"{ColorConsole.FormatEmbeddedColor("Friend", ConsoleColor.Green)}" : $"{ColorConsole.FormatEmbeddedColor("Enemy", ConsoleColor.Red)}";
            return $"[{ownerPrefix} - {GameToString().TrimStart('[').TrimEnd(']')}]";
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
