using System;
using System.Collections.Generic;
using System.Text;
using GameState.GameRules;

namespace GameState
{
    public abstract class Card : ConsoleGameToString, IOwnable
    {
        public Guid CardId { get; protected set; }
        public Guid OwnerId { get; protected set; }
        public string Name { get; protected set; }
        public string Text { get; protected set; }
        public Rarity Rarity { get; protected set; }
        public CostValueState Cost { get; protected set; }
        public Action<Guid> OnPlay { get; protected set; }
        public Action<Guid> OnDraw { get; protected set; }

        protected Card()
        {
            CardId = Guid.NewGuid();
            Cost = new CostValueState(0);
        }

        protected Card(Guid ownerId, string name, string text, Rarity rarity, int cost) : this()
        {
            OwnerId = ownerId;
            Name = name;
            Text = text;
            Rarity = rarity;
            Cost = new CostValueState(cost);
        }

        public abstract override string GameToString();

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
