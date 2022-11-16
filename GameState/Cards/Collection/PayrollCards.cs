using System;
using System.Collections.Generic;
using System.Text;
using GameState.GameRules;

namespace GameState.Cards.Collection
{
    public static class PayrollCards
    {
        public static SpellBuilder Healthcare()
        {
            return new SpellBuilder("Healthcare", 3, "Give a friendly minion +3 health. Restore 3 health to all friendly characters.", Rarity.Rare)
                .AddSpellType(SpellType.Restorative)
                .AddTargetingParams(TargetCategory.FriendlyMinions)
                .AddOnPlay((spell, playerId) =>
                {
                    spell.SelectedTarget.Health.AddToBaseValue(3);
                    var friendlyCharacters = TargetMatcher.GetTargets(playerId, TargetCategory.Friends);
                    foreach (var friend in friendlyCharacters)
                    {
                        friend.Value.Health.AddToCurrentValue(3);
                    }
                });
        }

        public static SpellBuilder Paycheck()
        {
            return new SpellBuilder("Paycheck", 0, "Gain 1 coin this turn for each friendly minion.", Rarity.Common)
                .AddSpellType(SpellType.Business)
                .AddOnPlay((spell, playerId) =>
                {
                    var player = GameController.GetPlayer(playerId);
                    player.Coins.AddToCurrentValue(player.Board.GetAllMinions().Count);
                });
        }

        public static MinionBuilder Payroller()
        {
            return new MinionBuilder("Payroller", 6, 3, 4, "OnPlay: Deal 1 damage to all enemies.", Rarity.Epic)
                .AddOnPlay(playerId =>
                {
                    var targets = TargetMatcher.GetTargets(playerId, TargetCategory.Enemies);
                    foreach (var target in targets)
                    {
                        target.Value.TakeDamage(1);
                    }
                });
        }
    }
}
