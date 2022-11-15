using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public class GameEvent : ConsoleGameToString
    {
        public IOwnable Entity { get; }
        public GameEventType EventType { get; }
        public string Message { get; set; }

        public GameEvent(IOwnable entity, GameEventType eventType, string message)
        {
            Entity = entity;
            EventType = eventType;
            Message = message;
        }

        public override string GameToString()
        {
            return $"[{GameController.GetPlayer(Entity.OwnerId).Name}: {Message}]";
        }

        public override string GameToString(Guid currentPlayerId)
        {
            var ownerName = GameController.GetPlayer(Entity.OwnerId).Name;
            var ownerColor = Entity.OwnerId == currentPlayerId ?ConsoleColor.Green : ConsoleColor.Red;
            return $"[{ColorConsole.FormatEmbeddedColor(ownerName, ownerColor)}: {Message}]";
        }
    }

    public enum GameEventType
    {
        TurnStart,
        TurnEnd,
        MinionPlay,
        SpellPlay,
        Attack,
        MinionDeath,
        PlayerDeath,
        MinionSummon
    }
}
