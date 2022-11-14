namespace GameState
{
    public abstract class ConsoleGameToString : IConsoleGameToString
    {
        public abstract string GameToString();
        public abstract string GameToString(int currentPlayerId);
        public virtual void PrintGameToString()
        {
            ColorConsole.WriteEmbeddedColorLine(GameToString());
        }

        public virtual void PrintGameToString(int currentPlayerId)
        {
            ColorConsole.WriteEmbeddedColorLine(GameToString(currentPlayerId));
        }
    }
}