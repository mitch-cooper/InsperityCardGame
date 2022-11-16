using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.Cards.Collection
{
    public static class BenefitsCards
    {
        public static MinionBuilder NewHire()
        {
            return new MinionBuilder("New Hire", 1, 1, 3, "OnPlay: Deal 2 damage to your player.", Rarity.Rare)
                .AddOnPlay(playerId =>
                {
                    var player = GameController.GetPlayer(playerId);
                    player.TakeDamage(2);
                });
        }

        public static MinionBuilder HSAContributer()
        {
            return new MinionBuilder("HSA Contributer", 3, 2, 2, "OnPlay: Draw a card.", Rarity.Rare)
                .AddOnPlay(playerId =>
                {
                    CardBehaviors.DrawCard(playerId);
                });
        }

        public static MinionBuilder CommunityLeader()
        {
            return new MinionBuilder("Community Leader", 5, 5, 4, "OnPlay: Gain +1/+1 for each friendly minion.", Rarity.Epic)
                .AddOnPlay(playerId =>
                {
                    var minionCount = GameController.GetPlayer(playerId).Board.GetAllMinions().Count;
                    // TODO: implement
                });
        }
    }
}
