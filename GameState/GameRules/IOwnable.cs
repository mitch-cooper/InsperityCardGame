using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public interface IOwnable
    {
        public Guid OwnerId { get; }
    }
}
