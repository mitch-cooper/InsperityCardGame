using System;
using System.Collections.Generic;
using System.Text;
using GameState.Cards;
using GameState.GameRules;

namespace GameState
{
    public class Minion : BoardCharacter, IConsoleDrawable
    {
        public Action<Guid> OnSummon { get; protected set; }
        public Action<Guid> OnDeath { get; protected set; }

        public Minion(MinionBuilder builder) : base(builder.PlayerId, builder.Name, builder.Text,
            builder.Rarity, builder.Cost, builder.Attack, builder.Health)
        {
            OnPlay = builder.OnPlay;
            OnSummon = builder.OnSummon;
            OnDeath = builder.OnDeath;
            OnDraw = builder.OnDraw;
        }

        public void Summon()
        {
            OnSummon(OwnerId);
            SleepTurnTimer = 1;
        }

        protected override void OnDeathEvent()
        {
            OnDeath(OwnerId);
            OnCharacterDeathEvent(new GameEvent(this, GameEventType.MinionDeath, $"{Name} has died."));
        }

        public List<string> GetDrawToConsoleLines()
        {
            var cah = GetCostAttackHealthForPrint();
            var borderColor = (ConsoleColor)Rarity;
            var cost = ColorConsole.FormatEmbeddedColorPadRight(cah.Cost.Value, cah.Cost.Color, 2, ' ', borderColor);
            var attack = ColorConsole.FormatEmbeddedColorPadRight(cah.Attack.Value, cah.Attack.Color, 3, '_', borderColor);
            var health = ColorConsole.FormatEmbeddedColorPadLeft(cah.Health.Value, cah.Health.Color, 2, '_', borderColor);
            var lines = new List<string>()
            {
                ColorConsole.FormatEmbeddedColor($" _____ ", borderColor),
                $"{ColorConsole.FormatEmbeddedColor("|", borderColor)}{cost}{ColorConsole.FormatEmbeddedColor("   |", borderColor)}",
                ColorConsole.FormatEmbeddedColor($"|     |", borderColor),
                ColorConsole.FormatEmbeddedColor($"|     |", borderColor),
                ColorConsole.FormatEmbeddedColor($"|     |", borderColor),
                $"{ColorConsole.FormatEmbeddedColor("|", borderColor)}{attack}{health}{ColorConsole.FormatEmbeddedColor("|", borderColor)}"
            };
            return lines;
        }

        public override string GameToString()
        {
            var cah = GetCostAttackHealthForPrint();
            var text = !string.IsNullOrWhiteSpace(Text) ? $" {Text}" : string.Empty;
            return $"[({ColorConsole.FormatEmbeddedColor(cah.Cost.Value, cah.Cost.Color)}) {ColorConsole.FormatEmbeddedColor(Name, (ConsoleColor)Rarity)} ({ColorConsole.FormatEmbeddedColor(cah.Attack.Value, cah.Attack.Color)}/{ColorConsole.FormatEmbeddedColor(cah.Health.Value, cah.Health.Color)}){text}]";
        }

        public override string GameToString(Guid currentPlayerId)
        {
            var ownerPrefix = OwnerId == currentPlayerId ? $"{ColorConsole.FormatEmbeddedColor("Friend", ConsoleColor.Green)}" : $"{ColorConsole.FormatEmbeddedColor("Enemy", ConsoleColor.Red)}";
            return $"[{ownerPrefix} - {GameToString()}]";
        }

        private ((string Value, ConsoleColor Color) Cost, (string Value, ConsoleColor Color) Attack, (string Value, ConsoleColor
            Color) Health) GetCostAttackHealthForPrint()
        {
            var cost = Cost.GameToStringValues();
            var attack = Attack.GameToStringValues();
            var health = Health.GameToStringValues();
            return (cost, attack, health);
        }
    }
}
