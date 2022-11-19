using System;
using System.Collections.Generic;
using System.Text;
using GameState.GameRules;

namespace GameState.Cards
{
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

        public (TargetCategory Category, Predicate<BoardCharacter> Filter) TargetingParams { get; protected set; }
        public BoardCharacter SelectedTarget { get; }
        public Action<ISpell, Guid> OnPlay { get; protected set; } = Constants.DoNothingSpell;
        public Action<ISpell, Guid> OnDraw { get; protected set; } = Constants.DoNothingSpell;

        public SpellBuilder(string name, int cost, string text, Rarity rarity)
        {
            CardId = Guid.NewGuid();
            Name = name;
            Cost = new CostValueState(cost);
            Text = text;
            Rarity = rarity;
            TargetingParams = (TargetCategory.None, null);
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

        public SpellBuilder AddTargetingParams(TargetCategory category, Predicate<BoardCharacter> filter = null)
        {
            TargetingParams = (category, filter);
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
