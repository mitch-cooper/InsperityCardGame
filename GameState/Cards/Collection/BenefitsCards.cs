using System;
using System.Collections.Generic;
using System.Text;
using GameState.GameRules;

namespace GameState.Cards.Collection
{
    public static class BenefitsCards
    {
        public static SpellBuilder MissedOpenEnrollment()
        {
            return new SpellBuilder("Missed Open Enrollment", 1, "Deal 1 damage to a minion. Increase by 1 for each friendly minion.", Rarity.Rare)
                .AddSpellType(SpellType.Illness)
                .AddTargetingParams(TargetCategory.Minions)
                .AddOnPlay((spell, playerId) =>
                {
                    var friendlyMinionCount = GameController.GetPlayer(playerId).Board.GetAllMinions().Count;
                    spell.SelectedTarget.TakeDamage(1 + friendlyMinionCount);
                });
        }

        public static MinionBuilder FSAFreezer()
        {
            return new MinionBuilder("FSA Freezer", 2, 2, 2, "OnPlay: Freeze an enemy.", Rarity.Rare)
                .AddTargetingParams(TargetCategory.Enemies)
                .AddOnPlay((minion, playerId) =>
                {
                    minion.SelectedTarget?.Freeze();
                });
        }

        public static MinionBuilder HSAContributer()
        {
            return new MinionBuilder("HSA Contributer", 3, 2, 3, "OnPlay: Restore 2 health. Draw a card.", Rarity.Rare)
                .AddTargetingParams(TargetCategory.All)
                .AddOnPlay((minion, playerId) =>
                {
                    minion.SelectedTarget?.Health.AddToCurrentValue(2);
                    CardBehaviors.DrawCard(playerId);
                });
        }

        public static SpellBuilder FreezeContributions()
        {
            return new SpellBuilder("Freeze Contributions", 3, "Freeze all enemy minions.", Rarity.Rare)
                .AddOnPlay((spell, playerId) =>
                {
                    var enemyMinions = GameController.GetOpponent(playerId).Board.GetAllMinions();
                    foreach (var minion in enemyMinions)
                    {
                        minion.Freeze();
                    }
                });
        }

        public static MinionBuilder WorkedToDeath()
        {
            return new MinionBuilder("Worked to Death", 4, 3, 3, "OnDeath: Summon a 4/1 Accidental Life Insurance with Rush.", Rarity.Epic)
                .AddOnDeath((minion, playerId) =>
                {
                    var board = GameController.GetPlayer(playerId).Board;
                    board.SummonMinion(NonCollectibleCards.Token("Accidental Life Insurance", 4, 4, 1).AddAttribute(BoardCharacterAttribute.Rush).Build(playerId));
                });
        }

        public static MinionBuilder CommunityLeader()
        {
            return new MinionBuilder("Community Leader", 5, 5, 4, "OnPlay: Gain +1/+1 for each friendly minion.", Rarity.Epic)
                .AddOnPlay((minion, playerId) =>
                {
                    var minionCount = GameController.GetPlayer(playerId).Board.GetAllMinions().Count;
                    minion.Attack.AddToCurrentValue(minionCount);
                    minion.Health.AddToBaseValue(minionCount);
                });
        }
    }
}
