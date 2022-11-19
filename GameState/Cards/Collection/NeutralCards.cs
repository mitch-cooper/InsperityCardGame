using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameState.GameRules;

namespace GameState.Cards.Collection
{
    public static class NeutralCards
    {
        public static MinionBuilder Intern()
        {
            return new MinionBuilder("Intern", 1, 2, 2, "OnPlay: Deal 2 damage to your player.", Rarity.Common)
                .AddOnPlay((minion, playerId) =>
                {
                    var player = GameController.GetPlayer(playerId);
                    player.TakeDamage(2);
                });
        }
        
        public static MinionBuilder Assistant()
        {
            return new MinionBuilder("Assistant", 2, 3, 2, "Whenever you summon a minion, deal 1 damage to a random enemy.", Rarity.Rare)
                .AddOnSummon((minion, playerId) =>
                {
                    var board = GameController.GetPlayer(playerId).Board;
                    IGameEventEmitter.GameEventHandler effectFunction = (gameEvent) =>
                    {
                        if (((Minion)gameEvent.Entity).CardId != minion.CardId)
                        {
                            var enemies = TargetMatcher.GetTargets(playerId, TargetCategory.Enemies);
                            var randomEnemy = enemies.ToList()[new Random().Next(enemies.Count)].Value;
                            randomEnemy.TakeDamage(1);
                        }
                    };
                    board.MinionSummonTriggered += effectFunction;
                    minion.OnDeath = (minion1, guid) =>
                    {
                        //minion.OnDeath(minion1, guid);
                        board.MinionSummonTriggered -= effectFunction;
                    };
                }); ;
        }

        public static MinionBuilder Employee()
        {
            return new MinionBuilder("Employee", 3, 3, 4, "", Rarity.Common);
        }

        public static MinionBuilder Developer()
        {
            return new MinionBuilder("Developer", 3, 3, 2, "OnPlay: Summon a 1/2 QA.", Rarity.Rare)
                .AddOnPlay((minion, playerId) =>
                {
                    var board = GameController.GetPlayer(playerId).Board;
                    board.SummonMinion(NonCollectibleCards.Token("QA", 1, 1, 2).Build(playerId));
                });
        }

        public static MinionBuilder Boss()
        {
            return new MinionBuilder("Boss", 4, 4, 5, "", Rarity.Common);
        }
        
        public static MinionBuilder Manager()
        {
            return new MinionBuilder("Manager", 4, 4, 3, "Your other minions have +1 attack.", Rarity.Epic)
                .AddOnSummon((minion, playerId) =>
                {
                    var board = GameController.GetPlayer(playerId).Board;
                    foreach (var friendlyMinion in TargetMatcher.GetTargets(playerId, TargetCategory.FriendlyMinions).Select(x => x.Value))
                    {
                        friendlyMinion.Attack.AddToCurrentValue(1);
                    }
                    IGameEventEmitter.GameEventHandler effectFunction = (gameEvent) =>
                    {
                        var minionSummoned = ((Minion) gameEvent.Entity);
                        if (minionSummoned.CardId != minion.CardId)
                        {
                            minionSummoned.Attack.AddToCurrentValue(1);
                        }
                    };
                    board.MinionSummonTriggered += effectFunction;
                    minion.OnDeath = (minion1, playerId1) =>
                    {
                        //minion.OnDeath(minion1, playerId1);
                        var board = GameController.GetPlayer(playerId1).Board;
                        foreach (var friendlyMinion in board.GetAllMinions())
                        {
                            friendlyMinion.Attack.AddToCurrentValue(-1);
                        }
                        board.MinionSummonTriggered -= effectFunction;
                    };
                });
        }

        public static MinionBuilder CEO()
        {
            return new MinionBuilder("CEO", 8, 3, 5, "OnPlay: Deal 10 damage randomly split amongst other minions.", Rarity.Legendary)
                .AddOnPlay((minion, playerId) =>
                {
                    var board = TargetMatcher.GetTargets(playerId, TargetCategory.Minions);
                    for (var i = 0; i < 10; i++)
                    {
                        var aliveMinions = board.Select(x => x.Value).Where(x => x.Health.CurrentValue > 0).ToList();
                        if (aliveMinions.Any())
                        {
                            var randomAliveMinion = aliveMinions[new Random().Next(aliveMinions.Count)];
                            randomAliveMinion.TakeDamage(1);
                        }
                    }
                });
        }
    }
}
