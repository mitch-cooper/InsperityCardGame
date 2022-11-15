using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public interface IConsoleDrawable
    {
        List<string> GetDrawToConsoleLines();
    }
}
