using System;
using System.Collections.Generic;
using System.Text;
using GameState.GameRules;

namespace GameState.Cards
{
    public class MinionBuilder// : IBoardItemCard
    {
        public string Name { get; }
        public CostValueState Cost { get; }
        public AttackValueState Attack { get; }
        public HealthValueState Health { get; }
        public string Text { get; }
        public Rarity Rarity { get; }
        public Guid PlayerId { get; private set; }

        public Action< /*IBoardItemCard, */Guid> OnPlay = Constants.DoNothing;//BoardItemCard;
        public Action</*IBoardItemCard, */Guid> OnSummon = Constants.DoNothing;//BoardItemCard;
        public Action</*IBoardItemCard, */Guid> OnDeath = Constants.DoNothing;//BoardItemCard;
        public Action</*IBoardItemCard, */Guid> OnDraw = Constants.DoNothing;//BoardItemCard;

        public MinionBuilder(string name, int cost, int attack, int health, string text, Rarity rarity)
        {
            Name = name;
            Cost = new CostValueState(cost);
            Attack = new AttackValueState(attack);
            Health = new HealthValueState(health);
            Text = text;
            Rarity = rarity;
        }

        public MinionBuilder AddOnPlay(Action</*IBoardItemCard, */Guid> action)
        {
            OnPlay = action;
            return this;
        }

        public MinionBuilder AddOnSummon(Action</*IBoardItemCard, */Guid> action)
        {
            OnSummon = action;
            return this;
        }

        public MinionBuilder AddOnDeath(Action</*IBoardItemCard, */Guid> action)
        {
            OnDeath = action;
            return this;
        }

        public MinionBuilder AddOnDraw(Action</*IBoardItemCard, */Guid> action)
        {
            OnDraw = action;
            return this;
        }

        public Minion Build(Guid playerId)
        {
            PlayerId = playerId;
            return new Minion(this);
        }
    }
}
