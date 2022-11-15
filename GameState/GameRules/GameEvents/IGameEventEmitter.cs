using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public interface IGameEventEmitter
    {
        delegate void GameEventHandler(GameEvent gameEvent);
    }
}
