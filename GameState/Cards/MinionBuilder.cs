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
        public int PlayerId { get; private set; }

        public Action<int> OnPlay = Constants.DoNothing;
        public Action<int> OnSummon = Constants.DoNothing;
        public Action<int> OnDeath = Constants.DoNothing;
        public Action<int> OnDraw = Constants.DoNothing;

        public MinionBuilder(string name, int cost, int attack, int health, string text, Rarity rarity)
        {
            Name = name;
            Cost = cost;
            Attack = attack;
            Health = health;
            Text = text;
            Rarity = rarity;
        }

        public MinionBuilder AddOnPlay(Action<int> action)
        {
            OnPlay = action;
            return this;
        }

        public MinionBuilder AddOnSummon(Action<int> action)
        {
            OnSummon = action;
            return this;
        }

        public MinionBuilder AddOnDeath(Action<int> action)
        {
            OnDeath = action;
            return this;
        }

        public MinionBuilder AddOnDraw(Action<int> action)
        {
            OnDraw = action;
            return this;
        }

        public Minion Build(int playerId)
        {
            PlayerId = playerId;
            return new Minion(this);
        }
    }
}
