using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.Cards
{
    public static class CardBehaviors
    {
        public static Action<Guid> DrawCard = (Guid playerId) =>
        {
            var player = GameController.GetPlayer(playerId);
            player.Draw();
        };
    }
}
