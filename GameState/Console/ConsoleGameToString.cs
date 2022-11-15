using System;

namespace GameState
{
    public abstract class ConsoleGameToString : IConsoleGameToString
    {
        public abstract string GameToString();
        public abstract string GameToString(Guid currentPlayerId);
        public virtual void PrintGameToString()
        {
            ColorConsole.WriteEmbeddedColorLine(GameToString());
        }

        public virtual void PrintGameToString(Guid currentPlayerId)
        {
            ColorConsole.WriteEmbeddedColorLine(GameToString(currentPlayerId));
        }
    }
}