using System;
using System.Collections.Generic;
using GameState.GameRules;

namespace GameState
{
    public interface IPlayerBoard
    {
        List<Minion> GetAllMinions();
        Dictionary<PlayerInput, Minion> GetMinionsThatCanAttack();
        Dictionary<PlayerInput, BoardCharacter> GetAttackableTargets();
        void ResetBoard();
        bool HasMaxMinions();
    }
}