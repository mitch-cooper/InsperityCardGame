﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public abstract class BoardCharacter : Card, IBoardItem, IGameEventEmitter
    {
        public AttackValueState Attack { get; protected set; }
        public HealthValueState Health { get; protected set; }
        public int AttacksThisTurn { get; set; }
        public int SleepTurnTimer { get; set; }
        public event IGameEventEmitter.GameEventHandler GameEventTriggered;

        protected BoardCharacter() : base()
        {
            Attack = new AttackValueState(0);
            Health = new HealthValueState(0);
            AttacksThisTurn = 0;
            SleepTurnTimer = 1;
        }

        protected BoardCharacter(Guid ownerId, string name, string text, Rarity rarity, int cost, int attack, int health) : base(ownerId, name, text, rarity, cost)
        {
            Attack = new AttackValueState(attack);
            Health = new HealthValueState(health);
            AttacksThisTurn = 0;
            SleepTurnTimer = 1;
        }

        protected void ResetCharacter()
        {
            GameEventTriggered = null;
            AttacksThisTurn = 0;
            SleepTurnTimer = 1;
        }

        protected abstract void OnDeathEvent();

        protected void OnCharacterEvent(GameEvent gameEvent)
        {
            GameEventTriggered?.Invoke(gameEvent);
        }

        public void TakeDamage(int value)
        {
            var newHealth = Health.AddToCurrentValue((int)(value * -1));
            if (newHealth <= 0)
            {
                OnDeathEvent();
            }
        }

        public void RestoreHealth(int value)
        {
            Health.AddToCurrentValue((int)value);
        }

        public bool CanAttack()
        {
            return SleepTurnTimer == 0 && AttacksThisTurn == 0 && Attack.CurrentValue > 0;
        }

        public void PromptAttackAndAttack(Guid playerId)
        {
            AttackBoardItem(PromptAttack(playerId));
        }

        public IBoardItem PromptAttack(Guid playerId)
        {
            var targets = GameController.GetOpponent(playerId).Board.GetAttackableTargets();
            ColorConsole.WriteLine($"Choose a target to attack: ");
            foreach (var target in targets)
            {
                ColorConsole.WriteEmbeddedColorLine($"\t{target.Key} - {target.Value}");
            }

            var input = Console.ReadKey();
            if (!targets.ContainsKey(input.Key))
            {
                ColorConsole.WriteLine("Unrecognized input. Try again.");
                return PromptAttack(playerId);
            }
            return targets[input.Key];
        }

        public void AttackBoardItem(IBoardItem unit)
        {
            unit.TakeDamage(Attack.CurrentValue);
            TakeDamage(unit.Attack.CurrentValue);
            AttacksThisTurn++;
            OnCharacterEvent(new GameEvent(this, GameEventType.Attack, $"{Name} attacked {unit.Name}."));
        }

    }
}
