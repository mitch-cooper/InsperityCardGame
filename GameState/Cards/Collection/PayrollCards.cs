using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameState.GameRules;

namespace GameState.Cards.Collection
{
    public static class PayrollCards
    {

        public static SpellBuilder Paycheck()
        {
            return new SpellBuilder("Paycheck", 0, "Gain 1 Coin this turn for each friendly minion.", Rarity.Common)
                .AddSpellType(SpellType.Business)
                .AddOnPlay((spell, playerId) =>
                {
                    var player = GameController.GetPlayer(playerId);
                    player.Coins.AddToCurrentValue(player.Board.GetAllMinions().Count);
                });
        }

        public static MinionBuilder BusinessAnalyst()
        {
            return new MinionBuilder("Business Analyst", 1, 1, 3, "Whenever you play a Business spell, deal 2 damage to a random enemy minion.", Rarity.Epic)
                .AddOnSummon((minion, playerId) =>
                {
                    var hand = GameController.GetPlayer(playerId).Hand;
                    IGameEventEmitter.GameEventHandler effectFunction = (gameEvent) =>
                    {
                        var spell = (Spell) gameEvent.Entity;
                        if (spell.Type == SpellType.Business)
                        {
                            var enemyMinions = TargetMatcher.GetTargets(playerId, TargetCategory.EnemyMinions);
                            var randomEnemyMinion = enemyMinions.ToList()[new Random().Next(enemyMinions.Count)].Value;
                            randomEnemyMinion.TakeDamage(2);
                        }
                    };
                    hand.SpellPlayTriggered += effectFunction;
                    minion.OnDeath = (minion1, guid) =>
                    {
                        //minion.OnDeath(minion1, guid);
                        hand.SpellPlayTriggered -= effectFunction;
                    };
                }); ;
        }

        public static MinionBuilder QuickWithdrawer()
        {
            return new MinionBuilder("Quick Withdrawer", 2, 2, 3, "Windfury", Rarity.Common)
                .AddAttribute(BoardCharacterAttribute.Windfury);
        }

        public static SpellBuilder FourZeroOneKInvestment()
        {
            return new SpellBuilder("401k Investment", 3, "Gain 1 additional Coin for each of your future turns.", Rarity.Common)
                .AddSpellType(SpellType.Business)
                .AddOnPlay((spell, playerId) =>
                {
                    var player = GameController.GetPlayer(playerId);
                    player.Coins.AddToBaseValue(1);
                    player.Coins.AddToCurrentValue(-1);
                });
        }

        public static SpellBuilder Healthcare()
        {
            return new SpellBuilder("Healthcare", 3, "Give a friendly minion +2 health. Restore 3 health to all friendly characters.", Rarity.Rare)
                .AddSpellType(SpellType.Restorative)
                .AddTargetingParams(TargetCategory.FriendlyMinions)
                .AddOnPlay((spell, playerId) =>
                {
                    spell.SelectedTarget.Health.AddToBaseValue(2);
                    var friendlyCharacters = TargetMatcher.GetTargets(playerId, TargetCategory.Friends);
                    foreach (var friend in friendlyCharacters)
                    {
                        friend.Value.Health.AddToCurrentValue(3);
                    }
                });
        }

        public static MinionBuilder Payroller()
        {
            return new MinionBuilder("Payroller", 6, 3, 4, "OnPlay: Deal 1 damage to all enemies.", Rarity.Epic)
                .AddOnPlay((minion, playerId) =>
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
