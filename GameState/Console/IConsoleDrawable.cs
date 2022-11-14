using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public interface IConsoleDrawable
    {
        // void DrawToConsole(bool useNewLine = true);
        List<string> GetDrawToConsoleLines();
    }
}
