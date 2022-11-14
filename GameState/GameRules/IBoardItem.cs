using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public interface IBoardItem : IOwnable
    {
        AttackValueState Attack { get; }
        HealthValueState Health { get; }
        event EventHandler<int> Died;
        int AttacksThisTurn { get; }
        int SleepTurnTimer { get; }
        void TakeDamage(int value);
        void RestoreHealth(int value);
        void PromptAttackAndAttack(int playerId);
        IBoardItem PromptAttack(int playerId);
        void AttackBoardItem(IBoardItem unit);
    }
}
