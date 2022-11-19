using System;
using System.Collections.Generic;
using System.Text;
using GameState.GameRules;

namespace GameState.Cards
{

    public class MinionBuilder
    {
        public string Name { get; }
        public CostValueState Cost { get; }
        public AttackValueState Attack { get; }
        public HealthValueState Health { get; }
        public HashSet<BoardCharacterAttribute> Attributes { get; }
        public string Text { get; }
        public Rarity Rarity { get; }
        public Guid OwnerId { get; private set; }

        public (TargetCategory Category, Predicate<BoardCharacter> Filter) TargetingParams { get; protected set; }
        public BoardCharacter SelectedTarget { get; }
        public Action<Minion, Guid> OnPlay { get; protected set; } = Constants.DoNothingMinion;
        public Action<Minion, Guid> OnSummon { get; protected set; } = Constants.DoNothingMinion;
        public Action<Minion, Guid> OnDeath { get; protected set; } = Constants.DoNothingMinion;
        public Action<Minion, Guid> OnDraw { get; protected set; } = Constants.DoNothingMinion;

        public MinionBuilder(string name, int cost, int attack, int health, string text, Rarity rarity)
        {
            Name = name;
            Cost = new CostValueState(cost);
            Attack = new AttackValueState(attack);
            Health = new HealthValueState(health);
            Text = text;
            Rarity = rarity;
            Attributes = new HashSet<BoardCharacterAttribute>();
            TargetingParams = (TargetCategory.None, null);
        }

        public MinionBuilder AddTargetingParams(TargetCategory category, Predicate<BoardCharacter> filter = null)
        {
            TargetingParams = (category, filter);
            return this;
        }

        public MinionBuilder AddOnPlay(Action<Minion, Guid> action)
        {
            OnPlay = action;
            return this;
        }

        public MinionBuilder AddOnSummon(Action<Minion, Guid> action)
        {
            OnSummon = action;
            return this;
        }

        public MinionBuilder AddOnDeath(Action<Minion, Guid> action)
        {
            Attributes.Add(BoardCharacterAttribute.OnDeath);
            OnDeath = action;
            return this;
        }

        public MinionBuilder AddOnDraw(Action<Minion, Guid> action)
        {
            OnDraw = action;
            return this;
        }

        public MinionBuilder AddAttribute(BoardCharacterAttribute attribute)
        {
            Attributes.Add(attribute);
            return this;
        }

        public Minion Build(Guid playerId)
        {
            OwnerId = playerId;
            return new Minion(this);
        }
    }
}
