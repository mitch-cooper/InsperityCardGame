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
            var owner = GameController.GetPlayer(Entity.OwnerId);
            return $"[{ColorConsole.FormatEmbeddedColor(owner.Name, owner.Color)}: {Message}]";
        }

        public override string GameToString(Guid currentPlayerId)
        {
            return GameToString();
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
