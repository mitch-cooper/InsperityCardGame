using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameState.Cards;
using GameState.GameRules;

namespace GameState
{
    public class Spell : Card
    {
        //public Action<int> OnPlay { get; protected set; }
        public new Action<SpellBuilder, int> OnDraw { get; protected set; }
        public List<int> SpellValues { get; protected set; }
        public SpellType Type { get; protected set; }

        public Spell(SpellBuilder builder)
        {
            Name = builder.Name;
            Cost = new CostValueState(builder.Cost);
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
                $"[ {ColorConsole.FormatEmbeddedColor(Name, (ConsoleColor) Rarity)} ({ColorConsole.FormatEmbeddedColor(cost.Value, cost.Color)}) {DisplayText} ]";
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
