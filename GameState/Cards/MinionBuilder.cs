using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.Cards
{
    public class MinionBuilder
    {
        public string Name { get; }
        public int Cost { get; }
        public int Attack { get; }
        public int Health { get; }
        public string Text { get; }
        public Rarity Rarity { get; }
        public Guid PlayerId { get; private set; }

        public Action<Guid> OnPlay = Constants.DoNothing;
        public Action<Guid> OnSummon = Constants.DoNothing;
        public Action<Guid> OnDeath = Constants.DoNothing;
        public Action<Guid> OnDraw = Constants.DoNothing;

        public MinionBuilder(string name, int cost, int attack, int health, string text, Rarity rarity)
        {
            Name = name;
            Cost = cost;
            Attack = attack;
            Health = health;
            Text = text;
            Rarity = rarity;
        }

        public MinionBuilder AddOnPlay(Action<Guid> action)
        {
            OnPlay = action;
            return this;
        }

        public MinionBuilder AddOnSummon(Action<Guid> action)
        {
            OnSummon = action;
            return this;
        }

        public MinionBuilder AddOnDeath(Action<Guid> action)
        {
            OnDeath = action;
            return this;
        }

        public MinionBuilder AddOnDraw(Action<Guid> action)
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
