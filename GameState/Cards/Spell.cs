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
        public List<int> SpellValues { get; protected set; }
        public SpellType Type { get; protected set; }

        public Spell(SpellBuilder builder)
        {
            OwnerId = builder.OwnerId;
            Name = builder.Name;
            Cost = builder.Cost;
            Text = builder.Text;
            Rarity = builder.Rarity;

            SpellValues = builder.SpellValues ?? new List<int>();
            Type = builder.Type;
            OnPlay = builder.OnPlay;
            OnDraw = builder.OnDraw;
        }

        public string DisplayText => string.Format(Text, SpellValues.Select(x => x.ToString()).ToArray());

        public override string GameToString()
        {
            var cost = Cost.GameToStringValues();
            return
                $"[{ColorConsole.FormatEmbeddedColor(Name, (ConsoleColor) Rarity)} ({ColorConsole.FormatEmbeddedColor(cost.Value, cost.Color)}) {DisplayText}]";
        }
    }

    public enum SpellType
    {
        None,
        Community,
        Illness,
        Restorative
    }
}
