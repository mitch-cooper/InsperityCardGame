using System;
using System.Collections.Generic;
using System.Text;
using GameState.GameRules;

namespace GameState
{
    public interface ICard : IConsoleGameToString, IOwnable
    {
        Guid CardId { get; }
        string Name { get; }
        string Text { get; }
        Rarity Rarity { get; }
        CostValueState Cost { get; }
        (TargetCategory Category, Predicate<BoardCharacter> Filter) TargetingParams { get; }
        BoardCharacter SelectedTarget { get; }
    }

    public abstract class Card : ConsoleGameToString, ICard
    {
        public Guid CardId { get; protected set; }
        public Guid OwnerId { get; set; }
        public string Name { get; protected set; }
        public string Text { get; protected set; }
        public Rarity Rarity { get; protected set; }
        public CostValueState Cost { get; protected set; }
        public (TargetCategory Category, Predicate<BoardCharacter> Filter) TargetingParams { get; protected set; }
        public BoardCharacter SelectedTarget { get; protected set; }
        public Func<Guid, bool> BoardStateRequirementsToPlayMet { get; protected set; }
        protected bool TargetRequiredToPlay { get; set; }

        protected Card(Guid ownerId, string name, string text, Rarity rarity, CostValueState cost)
        {
            CardId = Guid.NewGuid();
            OwnerId = ownerId;
            Name = name;
            Text = text;
            Rarity = rarity;
            Cost = cost;
        }

        public bool IsCardPlayable()
        {
            return (TargetingParams.Category == TargetCategory.None
                   || !TargetRequiredToPlay
                   || TargetMatcher.GetTargets(OwnerId, TargetingParams.Category, TargetingParams.Filter).Count != 0)
                   && BoardStateRequirementsToPlayMet(OwnerId);
        }

        public bool CanProceedWithOnPlay()
        {
            if (TargetingParams.Category == TargetCategory.None)
            {
                return true;
            }
            var character = Prompts.SelectTarget(OwnerId, TargetingParams.Category, TargetingParams.Filter);
            if (character == null && TargetRequiredToPlay)
            {
                return false;
            }
            SelectedTarget = character;
            return true;
        }

        public abstract override string GameToString();

        public override string GameToString(Guid currentPlayerId)
        {
            var ownerPrefix = OwnerId == currentPlayerId ? $"{ColorConsole.FormatEmbeddedColor("Friend", ConsoleColor.Green)}" : $"{ColorConsole.FormatEmbeddedColor("Enemy", ConsoleColor.Red)}";
            return $"[{ownerPrefix} - {GameToString()}]";
        }
    }

    public enum Rarity
    {
        Common = ConsoleColor.Gray,
        Rare = ConsoleColor.Blue,
        Epic = ConsoleColor.Magenta,
        Legendary = ConsoleColor.Yellow
    }
}
