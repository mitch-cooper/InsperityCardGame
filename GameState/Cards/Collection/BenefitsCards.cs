using System;
using System.Collections.Generic;
using System.Linq;
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

        public static MinionBuilder TeamMember()
        {
            return new MinionBuilder("Team Member", 1, 1, 1, "OnPlay: Give a friendly minion +2 attack.", Rarity.Common)
                .AddTargetingParams(TargetCategory.FriendlyMinions)
                .AddOnPlay((minion, playerId) =>
                {
                    if (minion.SelectedTarget != null)
                    {
                        minion.SelectedTarget.Attack.AddToCurrentValue(2);
                    }
                });
        }

        public static MinionBuilder BenefitBoy()
        {
            return new MinionBuilder("Benefit Boy", 2, 2, 2, "OnPlay: Give a random minion in your hand +2/+2.", Rarity.Common)
                .AddOnPlay((minion, playerId) =>
                {
                    var minionsInHand = GameController.GetPlayer(playerId).Hand.GetAllCards().Where(x => x.CardId != minion.CardId && x is Minion).Cast<Minion>().ToList();
                    if (minionsInHand.Any())
                    {
                        var randomMinionInHand = minionsInHand[new Random().Next(minionsInHand.Count)];
                        randomMinionInHand.Health.AddToBaseValue(2);
                        randomMinionInHand.Attack.AddToCurrentValue(2);
                    }
                });
        }
        
        public static MinionBuilder FSAFreezer()
        {
            return new MinionBuilder("FSA Freezer", 2, 2, 2, "OnPlay: Freeze an enemy. (It misses its next attack)", Rarity.Rare)
                .AddTargetingParams(TargetCategory.Enemies)
                .AddOnPlay((minion, playerId) =>
                {
                    minion.SelectedTarget?.Freeze();
                });
        }

        public static MinionBuilder HSAContributer()
        {
            return new MinionBuilder("HSA Contributer", 3, 3, 3, "OnPlay: Restore 3 health. Draw a card.", Rarity.Rare)
                .AddTargetingParams(TargetCategory.All)
                .AddOnPlay((minion, playerId) =>
                {
                    minion.SelectedTarget?.Health.AddToCurrentValue(3);
                    CardBehaviors.DrawCard(playerId);
                });
        }

        public static SpellBuilder FreezeContributions()
        {
            return new SpellBuilder("Freeze Contributions", 3, "Freeze all enemy minions. (They miss their next attack)", Rarity.Rare)
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
            return new MinionBuilder("Worked to Death", 4, 3, 3, "OnDeath: Summon a 3/1 Accidental Life Insurance with Rush. (Can attack minions on the same turn summoned)", Rarity.Epic)
                .AddOnDeath((minion, playerId) =>
                {
                    var board = GameController.GetPlayer(playerId).Board;
                    board.SummonMinion(NonCollectibleCards.Token("Accidental Life Insurance", 2, 3, 1).AddAttribute(BoardCharacterAttribute.Rush).Build(playerId));
                });
        }

        public static SpellBuilder PerformanceReview()
        {
            return new SpellBuilder("Performance Review", 4, "Choose a minion, if it's friendly, give it +3/+3. Otherwise, deal 5 damage to it.", Rarity.Common)
                .AddTargetingParams(TargetCategory.Minions)
                .AddOnPlay((spell, playerId) =>
                {
                    var isFriendlyMinion = GameController.GetPlayer(playerId).Board.GetAllMinions()
                        .Exists(x => x.CardId == spell.SelectedTarget.CardId);
                    if (isFriendlyMinion)
                    {
                        spell.SelectedTarget.Health.AddToBaseValue(3);
                        spell.SelectedTarget.Attack.AddToCurrentValue(3);
                    }
                    else
                    {
                        spell.SelectedTarget.TakeDamage(5);
                    }
                });
        }

        public static SpellBuilder Volunteer()
        {
            return new SpellBuilder("Volunteer", 5, "Give a friendly minion +3 attack. Summon a 3/5 Helper. Draw a card.", Rarity.Rare)
                .AddSpellType(SpellType.Community)
                .AddTargetingParams(TargetCategory.FriendlyMinions)
                .AddOnPlay((spell, playerId) =>
                {
                    spell.SelectedTarget.Attack.AddToCurrentValue(3);
                    var player = GameController.GetPlayer(playerId);
                    var helper = NonCollectibleCards.Token("Helper", 3, 2, 4).Build(playerId);
                    player.Board.SummonMinion(helper);
                    player.Draw();
                });
        }

        public static MinionBuilder CommunityLeader()
        {
            return new MinionBuilder("Community Leader", 5, 4, 4, "OnPlay: Gain +1/+1 (Attack/Health) for each friendly minion.", Rarity.Epic)
                .AddOnPlay((minion, playerId) =>
                {
                    var minionCount = GameController.GetPlayer(playerId).Board.GetAllMinions().Count;
                    minion.Attack.AddToCurrentValue(minionCount);
                    minion.Health.AddToBaseValue(minionCount);
                });
        }
        
        public static SpellBuilder HospitalOvercrowding()
        {
            return new SpellBuilder("Hospital Overcrowding", 7, "Deal 1 damage to all enemy minions. Increase by 1 for each enemy minion.", Rarity.Common)
                .AddSpellType(SpellType.Illness)
                .AddOnPlay((spell, playerId) =>
                {
                    var enemyMinions = GameController.GetOpponent(playerId).Board.GetAllMinions();
                    foreach(var enemyMinion in enemyMinions)
                    {
                        enemyMinion.TakeDamage(1 + enemyMinions.Count);
                    }
                });
        }

        public static MinionBuilder SolutionsDesigner()
        {
            return new MinionBuilder("Solutions Designer", 8, 7, 7, "OnPlay: Double the stats of all minions in your deck.", Rarity.Legendary)
                .AddOnPlay((minion, playerId) =>
                {
                    var minionsInDeck = GameController.GetPlayer(playerId).Deck.CurrentCards.Where(x => x is Minion).Cast<Minion>().ToList();
                    foreach (var minioninDeck in minionsInDeck)
                    {
                        minioninDeck.Health.AddToBaseValue(minioninDeck.Health.CurrentValue);
                        minioninDeck.Attack.AddToCurrentValue(minioninDeck.Attack.CurrentValue);
                    }
                });
        }
    }
}
