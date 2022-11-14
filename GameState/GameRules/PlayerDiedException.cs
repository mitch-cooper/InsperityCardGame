using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public class PlayerDiedException : Exception
    {
        public int PlayerId { get; }

        public PlayerDiedException(int playerId)
        {
            PlayerId = playerId;
        }

        public PlayerDiedException(int playerId, string message)
            : base(message)
        {
            PlayerId = playerId;
        }

        public PlayerDiedException(int playerId, string message, Exception inner)
            : base(message, inner)
        {
            PlayerId = playerId;
        }
    }
}
