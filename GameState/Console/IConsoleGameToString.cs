namespace GameState
{
    public interface IConsoleGameToString
    {
        string GameToString();
        string GameToString(int currentPlayerId);
        void PrintGameToString();
        void PrintGameToString(int currentPlayerId);
    }
}