using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public abstract class BoardCharacter : Card, IBoardItem, IGameEventEmitter
    {
        //public new Action</*IBoardItem, */Guid> OnPlay { get; protected set; }
        public AttackValueState Attack { get; protected set; }
        public HealthValueState Health { get; protected set; }
        public int AttacksThisTurn { get; set; }
        public int SleepTurnTimer { get; set; }
        public event IGameEventEmitter.GameEventHandler DeathTriggered;
        public event IGameEventEmitter.GameEventHandler AttackTriggered;
        
        protected BoardCharacter(Guid ownerId, string name, string text, Rarity rarity, CostValueState cost, AttackValueState attack, HealthValueState health)
            : base(ownerId, name, text, rarity, cost)
        {
            Attack = attack;
            Health = health;
            AttacksThisTurn = 0;
            SleepTurnTimer = 1;
        }

        protected void ResetCharacter()
        {
            DeathTriggered = null;
            AttackTriggered = null;
            AttacksThisTurn = 0;
            SleepTurnTimer = 1;
        }

        protected abstract void OnDeathEvent();

        protected void OnCharacterDeathEvent(GameEvent gameEvent)
        {
            DeathTriggered?.Invoke(gameEvent);
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
            AttackBoardItem(Prompts.SelectAttackTarget(playerId));
        }

        public void AttackBoardItem(BoardCharacter unit)
        {
            if (unit == null)
            {
                return;
            }
            AttacksThisTurn++;
            AttackTriggered?.Invoke(new GameEvent(this, GameEventType.Attack, $"{Name} attacked {unit.Name}."));
            unit.TakeDamage(Attack.CurrentValue);
            TakeDamage(unit.Attack.CurrentValue);
        }
    }
}
