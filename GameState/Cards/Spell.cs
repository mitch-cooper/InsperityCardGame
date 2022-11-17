using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameState.Cards;
using GameState.GameRules;

namespace GameState
{
    public class Spell : Card, ISpell
    {
        public new Action<ISpell, Guid> OnDraw { get; protected set; }
        public new Action<ISpell, Guid> OnPlay { get; protected set; }
        public (TargetCategory Category, Predicate<BoardCharacter> Filter) TargetingParams { get; protected set; }
        public BoardCharacter SelectedTarget { get; protected set; }
        public List<int> SpellValues { get; protected set; }
        public SpellType Type { get; protected set; }

        public Spell(SpellBuilder builder) : base(builder.OwnerId, builder.Name, builder.Text, builder.Rarity, builder.Cost)
        {
            SpellValues = builder.SpellValues ?? new List<int>();
            Type = builder.Type;
            OnPlay = builder.OnPlay;
            OnDraw = builder.OnDraw;
            TargetingParams = builder.TargetingParams;
        }

        public string DisplayText => string.Format(Text, SpellValues.Select(x => x.ToString()).ToArray());

        public override string GameToString()
        {
            var cost = Cost.GameToStringValues();
            return $"[({ColorConsole.FormatEmbeddedColor(cost.Value, cost.Color)}) {ColorConsole.FormatEmbeddedColor(Name, (ConsoleColor) Rarity)} {DisplayText}]";
        }

        public bool CanProceedWithOnPlay()
        {
            if (TargetingParams.Category == TargetCategory.None)
            {
                return true;
            }
            var character = Prompts.SelectTarget(OwnerId, TargetingParams.Category, TargetingParams.Filter);
            if (character == null)
            {
                return false;
            }
            SelectedTarget = character;
            return true;
        }
    }

    public enum SpellType
    {
        None,
        Community,
        Illness,
        Restorative,
        Business
    }
}
