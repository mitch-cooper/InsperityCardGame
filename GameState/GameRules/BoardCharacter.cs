using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameState.Cards;

namespace GameState.GameRules
{
    public abstract class BoardCharacter : Card, IBoardItem, IGameEventEmitter
    {
        public AttackValueState Attack { get; protected set; }
        public HealthValueState Health { get; protected set; }
        public HashSet<BoardCharacterAttribute> Attributes { get; set; }
        public int AttacksThisTurn { get; set; }
        public int MaxAttacksThisTurn => Attributes.Contains(BoardCharacterAttribute.Windfury) ? 2 : 1;
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
            Attributes = new HashSet<BoardCharacterAttribute>();
        }

        protected void ResetCharacter()
        {
            DeathTriggered = null;
            AttackTriggered = null;
            Cost = new CostValueState(Cost.OriginalBaseValue);
            Attack = new AttackValueState(Attack.OriginalBaseValue);
            Health = new HealthValueState(Health.OriginalBaseValue);
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
                // TODO: wait to resolve the death effect
                OnDeathEvent();
            }
        }

        public void RestoreHealth(int value)
        {
            Health.AddToCurrentValue((int)value);
        }

        public bool CanAttack()
        {
            return SleepTurnTimer == 0 && AttacksThisTurn < MaxAttacksThisTurn && Attack.CurrentValue > 0;
        }

        public virtual void PromptAttackAndAttack(Guid playerId)
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
            AttackTriggered?.Invoke(new GameEvent(this, GameEventType.Attack, $"{GameToString()} attacked {unit.GameToString()}."));
            unit.TakeDamage(Attack.CurrentValue);
            TakeDamage(unit.Attack.CurrentValue);
        }

        public bool HasAttribute(BoardCharacterAttribute attribute)
        {
            return Attributes.Contains(attribute);
        }

        public void Freeze()
        {
            SleepTurnTimer++;
            Attributes.Add(BoardCharacterAttribute.Frozen);
            GameController.TurnSystem.TurnEndTriggered += (e) =>
            {
                if (e.Entity.OwnerId == OwnerId && HasAttribute(BoardCharacterAttribute.Frozen)
                    && ((SleepTurnTimer == 1 && AttacksThisTurn == 0)
                        || SleepTurnTimer == 0))
                {
                    Attributes.Remove(BoardCharacterAttribute.Frozen);
                }
            };
        }
    }
}
