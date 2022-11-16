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
    }
}
