using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.Cards
{
    public class SpellBuilder
    {
        public string Name { get; }
        public int Cost { get; }
        public string Text { get; }
        public Rarity Rarity { get; }
        public int PlayerId { get; private set; }

        public List<int> SpellValues { get; protected set; } = new List<int>();
        public SpellType Type { get; protected set; } = SpellType.None;
        public Action<int> OnPlay = Constants.DoNothing;
        public Action<SpellBuilder, int> OnDraw = Constants.DoNothingSpell;

        public SpellBuilder(string name, int cost, string text, Rarity rarity)
        {
            Name = name;
            Cost = cost;
            Text = text;
            Rarity = rarity;
        }

        public SpellBuilder AddSpellValues(List<int> values)
        {
            SpellValues = values;
            return this;
        }

        public SpellBuilder AddSpellType(SpellType type)
        {
            Type = type;
            return this;
        }

        public SpellBuilder AddOnPlay(Action<int> action)
        {
            OnPlay = action;
            return this;
        }

        public SpellBuilder AddOnDraw(Action<SpellBuilder, int> action)
        {
            OnDraw = action;
            return this;
        }

        public Spell Build(int playerId)
        {
            PlayerId = playerId;
            return new Spell(this);
        }
    }
}
