using System;
using System.Collections.Generic;
using GameState.GameRules;

namespace GameState.Cards
{
    public interface ISpell// : ICard
    {
        Guid CardId { get; }
        Guid OwnerId { get; }
        string Name { get; }
        CostValueState Cost { get; }
        string Text { get; }
        Rarity Rarity { get; }
        List<int> SpellValues { get; }
        SpellType Type { get; }
        (TargetCategory Category, Predicate<BoardCharacter> Filter) TargetingParams { get; }
        BoardCharacter SelectedTarget { get; }
    }
}