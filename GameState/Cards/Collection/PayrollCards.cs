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

        public static SpellBuilder Overtime()
        {
            return new SpellBuilder("Overtime", 0, "Spend your coins to deal that much damage to a minion. Any excess damage goes to your opponent.", Rarity.Common)
                .AddSpellType(SpellType.Business)
                .AddTargetingParams(TargetCategory.Minions)
                .AddOnPlay((spell, playerId) =>
                {
                    var coinsSpent = GameController.GetPlayer(playerId).Coins.CurrentValue;
                    GameController.GetPlayer(playerId).Coins.AddToCurrentValue(-1 * coinsSpent);
                    var targetHealth = spell.SelectedTarget.Health.CurrentValue;
                    spell.SelectedTarget.TakeDamage(coinsSpent);
                    if (coinsSpent > targetHealth)
                    {
                        GameController.GetOpponent(playerId).TakeDamage(coinsSpent - targetHealth);
                    }
                });
        }

        public static SpellBuilder Bonus()
        {
            return new SpellBuilder("Bonus", 0, "Spend your coins to summon that many 1/1 minions. For each that can't fit, give a random friendly minion +2/+1.", Rarity.Epic)
                .AddOnPlay((spell, playerId) =>
                {
                    var coinsSpent = GameController.GetPlayer(playerId).Coins.CurrentValue;
                    GameController.GetPlayer(playerId).Coins.AddToCurrentValue(-1 * coinsSpent);
                    var playerBoard = GameController.GetPlayer(playerId).Board;
                    var originalBoardSize = playerBoard.GetAllMinions().Count;
                    for (int i = 0; i + originalBoardSize < Constants.MaxMinions && coinsSpent > 0; i++, coinsSpent--)
                    {
                        playerBoard.SummonMinion(NonCollectibleCards.Token("Bonus Dollar", 1, 1, 1).Build(playerId));
                    }
                    if (coinsSpent > 0)
                    {
                        playerBoard = GameController.GetPlayer(playerId).Board;
                        for (int i = 0; i < coinsSpent; i++)
                        {
                            var randomMinion =
                                playerBoard.GetAllMinions()[new Random().Next(playerBoard.GetAllMinions().Count)];
                            randomMinion.Attack.AddToCurrentValue(2);
                            randomMinion.Health.AddToBaseValue(1);
                        }
                    }
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
                            var enemyMinions = TargetMatcher.GetTargets(spell.OwnerId, TargetCategory.EnemyMinions);
                            var randomEnemyMinion = enemyMinions.ToList()[new Random().Next(enemyMinions.Count)].Value;
                            randomEnemyMinion.TakeDamage(2);
                        }
                    };
                    hand.SpellPlayTriggered += effectFunction;
                    minion.OnDeath = (minion1, guid) =>
                    {
                        hand.SpellPlayTriggered -= effectFunction;
                    };
                }); ;
        }

        public static SpellBuilder ReportedToHR()
        {
            return new SpellBuilder("Reported to HR", 2, "Destroy an enemy minion with 5 or more attack.", Rarity.Rare)
                .AddSpellType(SpellType.Community)
                .AddTargetingParams(TargetCategory.EnemyMinions, character => character.Attack.CurrentValue >= 5)
                .AddOnPlay((spell, playerId) =>
                {
                    spell.SelectedTarget.TakeDamage(spell.SelectedTarget.Health.CurrentValue);
                });
        }

        public static MinionBuilder QuickWithdrawer()
        {
            return new MinionBuilder("Quick Withdrawer", 2, 2, 3, "Windfury (can attack twice per turn)", Rarity.Common)
                .AddAttribute(BoardCharacterAttribute.Windfury);
        }

        public static SpellBuilder FourZeroOneKInvestment()
        {
            return new SpellBuilder("401k Investment", 3, "Gain 1 additional Coin for each of your future turns. If you have 10 coins already, draw a card instead.", Rarity.Common)
                .AddSpellType(SpellType.Business)
                .AddOnPlay((spell, playerId) =>
                {
                    var player = GameController.GetPlayer(playerId);
                    if (player.Coins.BaseValue < 10)
                    {
                        player.Coins.AddToBaseValue(1);
                        player.Coins.AddToCurrentValue(-1);
                    }
                    else
                    {
                        player.Draw();
                    }
                });
        }

        public static SpellBuilder Healthcare()
        {
            return new SpellBuilder("Healthcare", 3, "Restore 5 health to your player. Draw a card.", Rarity.Rare)
                .AddSpellType(SpellType.Restorative)
                //.AddTargetingParams(TargetCategory.FriendlyMinions)
                .AddOnPlay((spell, playerId) =>
                {
                    //spell.SelectedTarget.Health.AddToBaseValue(2);
                    //var friendlyCharacters = TargetMatcher.GetTargets(playerId, TargetCategory.Friends);
                    //foreach (var friend in friendlyCharacters)
                    //{
                    //    friend.Value.Health.AddToCurrentValue(3);
                    //}
                    var player = GameController.GetPlayer(playerId);
                    player.RestoreHealth(5);
                    player.Draw();
                });
        }

        public static SpellBuilder Layoffs()
        {
            return new SpellBuilder("Layoffs", 5, "Deal 4 damage to all minions.", Rarity.Rare)
                .AddSpellType(SpellType.Business)
                .AddOnPlay((spell, playerId) =>
                {
                    var minions = TargetMatcher.GetTargets(playerId, TargetCategory.Minions);
                    foreach (var minion in minions)
                    {
                        minion.Value.TakeDamage(4);
                    }
                });
        }

        public static MinionBuilder BigPresentation()
        {
            return new MinionBuilder("Big Presentation", 7, 4, 9, "At the end of your turn, restore 6 health to your player. At the start of your turn, draw a card.", Rarity.Epic)
                .AddOnSummon((minion, playerId) =>
                {
                    var turnSystem = GameController.TurnSystem;
                    IGameEventEmitter.GameEventHandler endTurnFunction = (gameEvent) =>
                    {
                        if (gameEvent.Entity.OwnerId == playerId)
                        {
                            GameController.GetPlayer(playerId).RestoreHealth(6);
                        }
                    };
                    IGameEventEmitter.GameEventHandler startTurnFunction = (gameEvent) =>
                    {
                        if (gameEvent.Entity.OwnerId == playerId)
                        {
                            GameController.GetPlayer(playerId).Draw();
                        }
                    };
                    turnSystem.TurnEndTriggered += endTurnFunction;
                    turnSystem.TurnStartTriggered += startTurnFunction;
                    minion.OnDeath = (minion1, guid) =>
                    {
                        turnSystem.TurnEndTriggered -= endTurnFunction;
                        turnSystem.TurnStartTriggered -= startTurnFunction;
                    };
                });
        }

        public static MinionBuilder Payroller()
        {
            return new MinionBuilder("Payroller", 6, 3, 4, "OnPlay: Deal 2 damage to all enemies.", Rarity.Epic)
                .AddOnPlay((minion, playerId) =>
                {
                    var targets = TargetMatcher.GetTargets(playerId, TargetCategory.Enemies);
                    foreach (var target in targets)
                    {
                        target.Value.TakeDamage(2);
                    }
                });
        }
        
        public static SpellBuilder Recruited()
        {
            return new SpellBuilder("Recruited!", 10, "Take control of an enemy minion.", Rarity.Legendary)
                .AddTargetingParams(TargetCategory.EnemyMinions)
                .AddBoardStateRequirementsToPlay((playerId) => !GameController.GetPlayer(playerId).Board.HasMaxMinions())
                .AddOnPlay((spell, playerId) =>
                {
                    var opponentsBoard = GameController.GetOpponent(playerId).Board;
                    var stolenMinion = spell.SelectedTarget as Minion;
                    opponentsBoard.RemoveMinionFromBoard(stolenMinion);
                    var playersBoard = GameController.GetPlayer(playerId).Board;
                    playersBoard.AddMinionToBoard(stolenMinion);
                });
        }
    }
}
