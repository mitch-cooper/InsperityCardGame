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
                .AddOnDraw((fatigueCardBuilder, playerId) =>
                {
                    var player = GameState.GetPlayer(playerId);
                    player.TakeDamage(fatigueCardBuilder.SpellValues.Single());
                    player.Hand.RemoveCard("Fatigue");
                });
        }

        public static SpellBuilder Money()
        {
            return new SpellBuilder("Money", 0, "Gain 1 Coin this turn.", Rarity.Common)
                .AddOnPlay(playerId =>
                {
                    var player = GameState.GetPlayer(playerId);
                    player.Coins.AddToCurrentValue(1);
                });
        }
    }
}
