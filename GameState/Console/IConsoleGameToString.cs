using System;

namespace GameState
{
    public interface IConsoleGameToString
    {
        string GameToString();
        string GameToString(Guid currentPlayerId);
        void PrintGameToString();
        void PrintGameToString(Guid currentPlayerId);
    }
}