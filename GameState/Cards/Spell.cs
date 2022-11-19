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
        public Action<ISpell, Guid> OnDraw { get; protected set; }
        public Action<ISpell, Guid> OnPlay { get; protected set; }
        public List<int> SpellValues { get; protected set; }
        public SpellType Type { get; protected set; }

        public Spell(SpellBuilder builder) : base(builder.OwnerId, builder.Name, builder.Text, builder.Rarity, builder.Cost)
        {
            SpellValues = builder.SpellValues ?? new List<int>();
            Type = builder.Type;
            OnPlay = builder.OnPlay;
            OnDraw = builder.OnDraw;
            TargetingParams = builder.TargetingParams;
            TargetRequiredToPlay = true;
        }

        public string DisplayText => string.Format(Text, SpellValues.Select(x => x.ToString()).ToArray());

        public override string GameToString()
        {
            var cost = Cost.GameToStringValues();
            var spellTypeDisplay = Type == SpellType.None ? string.Empty : $" ({Type})";
            return $"[({ColorConsole.FormatEmbeddedColor(cost.Value, cost.Color)}) {ColorConsole.FormatEmbeddedColor(Name, (ConsoleColor) Rarity)} - {DisplayText}{spellTypeDisplay}]";
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
