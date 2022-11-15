using System;
using System.Collections.Generic;
using System.Text;
using GameState.GameRules;

namespace GameState.Cards
{
    public interface ISpell : IOwnable
    {
        Guid CardId { get; }
        Guid OwnerId { get; }
        string Name { get; }
        CostValueState Cost { get; }
        string Text { get; }
        Rarity Rarity { get; }
        List<int> SpellValues { get; }
        SpellType Type { get; }
    }

    public class SpellBuilder : ISpell
    {
        public Guid CardId { get; }
        public Guid OwnerId { get; private set; }
        public string Name { get; }
        public CostValueState Cost { get; }
        public string Text { get; }
        public Rarity Rarity { get; }

        public List<int> SpellValues { get; protected set; } = new List<int>();
        public SpellType Type { get; protected set; } = SpellType.None;
        public Action<ISpell, Guid> OnPlay = Constants.DoNothingSpell;
        public Action<ISpell, Guid> OnDraw = Constants.DoNothingSpell;

        public SpellBuilder(string name, int cost, string text, Rarity rarity)
        {
            CardId = Guid.NewGuid();
            Name = name;
            Cost = new CostValueState(cost);
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

        public SpellBuilder AddOnPlay(Action<ISpell, Guid> action)
        {
            OnPlay = action;
            return this;
        }

        public SpellBuilder AddOnDraw(Action<ISpell, Guid> action)
        {
            OnDraw = action;
            return this;
        }

        public Spell Build(Guid playerId)
        {
            OwnerId = playerId;
            return new Spell(this);
        }
    }
}
