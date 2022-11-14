using System;
using System.Collections.Generic;
using System.Text;
using GameState.Cards;
using GameState.GameRules;

namespace GameState
{
    public class Minion : BoardCharacter, IConsoleDrawable
    {
        public int SleepTurnTimer { get; set; }
        //public Action<int> OnPlay { get; protected set; }
        //public Action<int> OnDraw { get; protected set; }
        public Action<int> OnSummon { get; protected set; }
        public Action<int> OnDeath { get; protected set; }

        public Minion(MinionBuilder builder) : base(builder.PlayerId, builder.Name, builder.Text,
            builder.Rarity, builder.Cost, builder.Attack, builder.Health)
        {
            OnPlay = playerId =>
            {
                var player = GameState.GetPlayer(playerId);
                player.Hand.PlayCard(Id);
                builder.OnPlay(playerId);
            };
            OnSummon = playerId =>
            {
                builder.OnSummon(playerId);
                SleepTurnTimer = 1;
            };
            OnDeath = builder.OnDeath;
            OnDraw = builder.OnDraw;
        }

        public List<string> GetDrawToConsoleLines()
        {
            var cah = GetCostAttackHealthForPrint();
            var lines = new List<string>()
            {
                $" _____ ",
                $"|{ColorConsole.FormatEmbeddedColor(cah.Cost.Value, cah.Cost.Color).PadRight(2, ' ')}   |",
                $"|     |",
                $"|     |",
                $"|     |",
                $"|{ColorConsole.FormatEmbeddedColor(cah.Attack.Value.PadRight(2, '_'), cah.Attack.Color)}_{ColorConsole.FormatEmbeddedColor(cah.Health.Value.PadLeft(2, '_'), cah.Health.Color)}|",
            };
            return lines;
        }

        public override string GameToString()
        {
            var cah = GetCostAttackHealthForPrint();
            var text = !string.IsNullOrWhiteSpace(Text) ? $" {Text}" : string.Empty;
            return $"[{ColorConsole.FormatEmbeddedColor(Name, (ConsoleColor)Rarity)} ({ColorConsole.FormatEmbeddedColor(cah.Cost.Value, cah.Cost.Color)}/{ColorConsole.FormatEmbeddedColor(cah.Attack.Value, cah.Attack.Color)}/{ColorConsole.FormatEmbeddedColor(cah.Health.Value, cah.Health.Color)}){text}]";
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
