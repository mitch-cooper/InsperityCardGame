using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameState.GameRules
{
    internal class Logger
    {
        public List<GameEvent> EventHistory { get; set; } = new List<GameEvent>();

        public void ResetHistory()
        {
            EventHistory.Clear();
        }

        public void AddEvent(GameEvent gameEvent)
        {
            EventHistory = EventHistory.Prepend(gameEvent).ToList();
        }

        public void PrintNthEvents(int n)
        {
            ColorConsole.WriteLine("Event History: ");
            foreach (var (gameEvent, index) in EventHistory.Take(n).WithIndex())
            {
                ColorConsole.WriteEmbeddedColorLine($"\t{index + 1}: {gameEvent.GameToString()}");
            }
        }
    }
}
