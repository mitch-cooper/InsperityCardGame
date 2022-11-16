﻿using System;
using System.Collections.Generic;
using GameState.GameRules;

namespace GameState
{
    public interface IPlayerBoard
    {
        List<Minion> GetAllMinions();
        Dictionary<ConsoleKey, Minion> GetMinionsThatCanAttack();
        Dictionary<ConsoleKey, BoardCharacter> GetAttackableTargets();
        void ResetBoard();
        bool HasMaxMinions();
    }
}