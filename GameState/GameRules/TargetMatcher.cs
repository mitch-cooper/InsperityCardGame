using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameState.GameRules
{
    internal static class TargetMatcher
    {
        public static List<IBoardItem> GetTargets(Guid currentPlayerId, TargetCategory baseFilter, Predicate<IBoardItem> filterPredicate = null)
        {
            var enemyPlayer = GameController.GetOpponent(currentPlayerId);
            var enemyMinions = enemyPlayer.Board.GetAllMinions();
            var player = GameController.GetPlayer(currentPlayerId);
            var friendlyMinions = player.Board.GetAllMinions();

            var targets = new List<IBoardItem>();

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

            if (enemyPlayerTargetOptions.Contains(baseFilter) && filterPredicate(enemyPlayer))
            {
                targets.Add(enemyPlayer);
            }
            if (enemyMinionsTargetOptions.Contains(baseFilter) && enemyMinions.Any(x => filterPredicate(x)))
            {
                targets.AddRange(enemyMinions.Where(x => filterPredicate(x)));
            }
            if (friendlyMinionTargetOptions.Contains(baseFilter) && friendlyMinions.Any(x => filterPredicate(x)))
            {
                targets.AddRange(friendlyMinions.Where(x => filterPredicate(x)));
            }
            if (playerTargetOptions.Contains(baseFilter) && filterPredicate(player))
            {
                targets.Add(player);
            }

            return targets;
        }
    }

    internal enum TargetCategory
    {
        All = 0,
        Enemies,
        EnemyMinions,
        Friends,
        FriendlyMinions,
        Players,
        Minions
    }
}
