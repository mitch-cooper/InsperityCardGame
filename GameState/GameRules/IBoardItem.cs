using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public interface IBoardItem : IOwnable
    {
        string Name { get; }
        AttackValueState Attack { get; }
        HealthValueState Health { get; }
        int AttacksThisTurn { get; }
        int SleepTurnTimer { get; }
        void TakeDamage(int value);
        void RestoreHealth(int value);
        void PromptAttackAndAttack(Guid playerId);
        IBoardItem PromptAttack(Guid playerId);
        void AttackBoardItem(IBoardItem unit);
    }
}
