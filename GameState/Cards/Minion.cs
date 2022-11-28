using System;
using System.Collections.Generic;
using System.Text;
using GameState.Cards;
using GameState.GameRules;

namespace GameState
{
    public class Minion : BoardCharacter, IConsoleDrawable
    {
        public Action<Minion, Guid> OnPlay { get; set; }
        public Action<Minion, Guid> OnDraw { get; set; }
        public Action<Minion, Guid> OnSummon { get; set; }
        public Action<Minion, Guid> OnDeath { get; set; }
        public int TurnSummoned { get; protected set; }

        public Minion(MinionBuilder builder) : base(builder.OwnerId, builder.Name, builder.Text,
            builder.Rarity, builder.Cost, builder.Attack, builder.Health)
        {
            OnPlay = builder.OnPlay;
            OnSummon = builder.OnSummon;
            OnDeath = builder.OnDeath;
            OnDraw = builder.OnDraw;
            TargetingParams = builder.TargetingParams;
            TargetRequiredToPlay = false;
            BoardStateRequirementsToPlayMet = builder.BoardStateRequirementsToPlayMet;
            Attributes = builder.Attributes;
        }

        public void Summon()
        {
            TurnSummoned = GameController.TurnSystem.TurnCount;
            SleepTurnTimer = Attributes.Contains(BoardCharacterAttribute.Rush) ? 0 : 1;
            OnSummon(this, OwnerId);
            GameController.TurnSystem.TurnStartTriggered += (e) =>
            {
                if (e.Entity.OwnerId == OwnerId)
                {
                    AttacksThisTurn = 0;
                }
            };
            GameController.TurnSystem.TurnEndTriggered += (e) =>
            {
                if (e.Entity.OwnerId == OwnerId && SleepTurnTimer > 0)
                {
                    SleepTurnTimer--;
                }
            };
        }

        protected override void OnDeathEvent()
        {
            OnCharacterDeathEvent(new GameEvent(this, GameEventType.MinionDeath, $"{GameToString()} has died."));
            OnDeath(this, OwnerId);
        }

        public override void PromptAttackAndAttack(Guid playerId)
        {
            Predicate<BoardCharacter> targetFilter = null;
            if (HasAttribute(BoardCharacterAttribute.Rush) && TurnSummoned == GameController.TurnSystem.TurnCount)
            {
                targetFilter = CardBehaviors.RushEffectAttackFilter;
            }
            AttackBoardItem(Prompts.SelectAttackTarget(playerId, targetFilter));
        }

        public List<string> GetDrawToConsoleLines()
        {
            var cah = GetCostAttackHealthForPrint();
            var borderColor = HasAttribute(BoardCharacterAttribute.Frozen) ? Constants.FrozenColor : (ConsoleColor)Rarity;
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
            var text = !string.IsNullOrWhiteSpace(Text) ? $" {ColorConsole.FormatEmbeddedColor(Text, ConsoleColor.Gray)}" : string.Empty;
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
