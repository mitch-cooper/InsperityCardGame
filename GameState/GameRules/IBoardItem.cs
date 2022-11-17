using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public interface IBoardItem : IBoardItemCard
    {
        int AttacksThisTurn { get; }
        int SleepTurnTimer { get; }
        void TakeDamage(int value);
        void RestoreHealth(int value);
        void PromptAttackAndAttack(Guid playerId);
        void AttackBoardItem(BoardCharacter unit);
    }
}
