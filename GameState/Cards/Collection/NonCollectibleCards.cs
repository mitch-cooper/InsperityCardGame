using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameState.Cards.Collection
{
    public static class NonCollectibleCards
    {
        public static SpellBuilder FatigueBuilder()
        {
            return new SpellBuilder("Fatigue", 0, "Deal {0} damage to yourself. Increase damage with every draw.",
                Rarity.Common)
                .AddOnDraw((spell, playerId) =>
                {
                    var player = GameController.GetPlayer(playerId);
                    player.TakeDamage(spell.SpellValues.Single());
                    player.Hand.RemoveCard(spell.CardId);
                });
        }

        public static SpellBuilder Money()
        {
            return new SpellBuilder("Money", 0, "Gain 1 Coin this turn.", Rarity.Common)
                .AddOnPlay((spell, playerId) =>
                {
                    var player = GameController.GetPlayer(playerId);
                    player.Coins.AddToCurrentValue(1);
                });
        }

        public static MinionBuilder Token(string name, int cost, int attack, int health)
        {
            return new MinionBuilder(name, cost, attack, health, "", Rarity.Common);
        }
    }
}
