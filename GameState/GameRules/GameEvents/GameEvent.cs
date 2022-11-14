using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public class GameEvent : ConsoleGameToString, IOwnable
    {
        public int OwnerId { get; }
        public GameEventType EventType { get; }
        public string Message { get; set; }

        public GameEvent(int ownerId, GameEventType eventType, string message)
        {
            OwnerId = ownerId;
            EventType = eventType;
            Message = message;
        }

        public override string GameToString()
        {
            return $"[{GameState.GetPlayer(OwnerId).Name}: {Message}]";
        }

        public override string GameToString(int currentPlayerId)
        {
            var ownerName = GameState.GetPlayer(OwnerId).Name;
            var ownerColor = OwnerId == currentPlayerId ?ConsoleColor.Green : ConsoleColor.Red;
            return $"[{ColorConsole.FormatEmbeddedColor(ownerName, ownerColor)}: {Message}]";
        }
    }

    public enum GameEventType
    {
        TurnStart,
        TurnEnd,
        CardPlay,
        Attack,
        Death
    }
}
