using System;
using System.Collections.Generic;
using System.Text;
using GameState.GameRules;

namespace GameState.Cards
{
    public static class CardBehaviors
    {
        public static Action<Guid> DrawCard = playerId =>
        {
            var player = GameController.GetPlayer(playerId);
            player.Draw();
        };

        public static Predicate<BoardCharacter> RushEffectAttackFilter = target => !(target is Player);
    }

    public enum BoardCharacterAttribute
    {
        Rush,
        Windfury,
        Lifesteal,
        Taunt,
        OnDeath,
        Frozen,
        DivineShield
    }
}
