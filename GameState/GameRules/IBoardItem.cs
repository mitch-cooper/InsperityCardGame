using System;
using System.Collections.Generic;
using System.Text;
using GameState.Cards;

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
        void Freeze();
        bool HasAttribute(BoardCharacterAttribute attribute);
    }
}
