using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.Cards.Collection
{
    public static class NeutralCards
    {

        public static MinionBuilder Employee()
        {
            return new MinionBuilder("Employee", 2, 2, 3, "", Rarity.Common);
        }

        public static MinionBuilder Boss()
        {
            return new MinionBuilder("Boss", 4, 4, 5, "", Rarity.Common);
        }

        public static MinionBuilder Developer()
        {
            return new MinionBuilder("Developer", 3, 2, 2, "OnPlay: Draw a card.", Rarity.Rare)
                .AddOnPlay(playerId =>
                {
                    var player = GameState.GetPlayer(playerId);
                    player.Draw();
                });
        }

        public static MinionBuilder Payroller()
        {
            return new MinionBuilder("Payroller", 6, 3, 4, "OnPlay: Deal 1 damage to all enemies.", Rarity.Epic)
                .AddOnPlay(playerId =>
                {
                    var opponent = GameState.GetOpponent(playerId);
                    foreach (var enemyMinion in opponent.Board.GetAllMinions())
                    {
                        enemyMinion.TakeDamage(1);
                    }
                    opponent.TakeDamage(1);
                });
        } 
    }
}
