using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    internal interface IGameEvent
    {
        delegate void GameEventHandler(GameEvent gameEvent);
        event GameEventHandler GameEventTriggered;
    }
}
