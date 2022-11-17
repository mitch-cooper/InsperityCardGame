using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameState.GameRules
{
    public static class TargetMatcher
    {
        public static Dictionary<PlayerInput, BoardCharacter> GetTargets(Guid currentPlayerId, TargetCategory targetCategory, Predicate<BoardCharacter> filterPredicate = null)
        {
            var enemyPlayer = GameController.GetOpponent(currentPlayerId);
            var enemyMinions = enemyPlayer.Board.GetAllMinions();
            var player = GameController.GetPlayer(currentPlayerId);
            var friendlyMinions = player.Board.GetAllMinions();

            var targets = new Dictionary<PlayerInput, BoardCharacter>();

            filterPredicate ??= (x => true);

            var enemyPlayerTargetOptions = new List<TargetCategory>()
            {
                TargetCategory.All,
                TargetCategory.Enemies,
                TargetCategory.Players
            };
            var enemyMinionsTargetOptions = new List<TargetCategory>()
            {
                TargetCategory.All,
                TargetCategory.Enemies,
                TargetCategory.EnemyMinions,
                TargetCategory.Minions
            };
            var friendlyMinionTargetOptions = new List<TargetCategory>()
            {
                TargetCategory.All,
                TargetCategory.Friends,
                TargetCategory.FriendlyMinions,
                TargetCategory.Minions
            };
            var playerTargetOptions = new List<TargetCategory>()
            {
                TargetCategory.All,
                TargetCategory.Friends,
                TargetCategory.Players
            };

            if (enemyPlayerTargetOptions.Contains(targetCategory) && filterPredicate(enemyPlayer))
            {
                targets.Add(Constants.OpponentPlayerKey, enemyPlayer);
            }
            if (enemyMinionsTargetOptions.Contains(targetCategory) && enemyMinions.Any(x => filterPredicate(x)))
            {
                foreach (var enemyMinion in enemyMinions.Where(x => filterPredicate(x)))
                {
                    targets.Add(Constants.OpponentsMinionKeys[enemyMinions.IndexOf(enemyMinion)], enemyMinion);
                }
            }
            if (friendlyMinionTargetOptions.Contains(targetCategory) && friendlyMinions.Any(x => filterPredicate(x)))
            {
                foreach(var friendlyMinion in friendlyMinions.Where(x => filterPredicate(x)))
                {
                    targets.Add(Constants.CurrentPlayersMinionKeys[friendlyMinions.IndexOf(friendlyMinion)], friendlyMinion);
                }
            }
            if (playerTargetOptions.Contains(targetCategory) && filterPredicate(player))
            {
                targets.Add(Constants.CurrentPlayerKey, player);
            }

            return targets;
        }
    }

    public enum TargetCategory
    {
        All = 0,
        Enemies,
        EnemyMinions,
        Friends,
        FriendlyMinions,
        Players,
        Minions,
        None
    }
}
