using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public class PlayerDiedException : Exception
    {
        public Guid PlayerId { get; }

        public PlayerDiedException(Guid playerId)
        {
            PlayerId = playerId;
        }

        public PlayerDiedException(Guid playerId, string message)
            : base(message)
        {
            PlayerId = playerId;
        }

        public PlayerDiedException(Guid playerId, string message, Exception inner)
            : base(message, inner)
        {
            PlayerId = playerId;
        }
    }
}
