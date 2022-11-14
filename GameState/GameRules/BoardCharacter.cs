using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public abstract class BoardCharacter : Card, IBoardItem
    {
        public AttackValueState Attack { get; protected set; }
        public HealthValueState Health { get; protected set; }
        public event EventHandler<int> Died;
        public int AttacksThisTurn { get; set; }
        public int SleepTurnTimer { get; set; }

        protected BoardCharacter() : base()
        {
            Attack = new AttackValueState(0);
            Health = new HealthValueState(0);
            AttacksThisTurn = 0;
            SleepTurnTimer = 1;
        }

        protected BoardCharacter(int ownerId, string name, string text, Rarity rarity, int cost, int attack, int health) : base(ownerId, name, text, rarity, cost)
        {
            Attack = new AttackValueState(attack);
            Health = new HealthValueState(health);
            AttacksThisTurn = 0;
            SleepTurnTimer = 1;
        }

        protected virtual void OnDeath(int playerId)
        {
            Died?.Invoke(this, playerId);
        }

        public void TakeDamage(int value)
        {
            var newHealth = Health.AddToCurrentValue((int)(value * -1));
            if (newHealth <= 0)
            {
                OnDeath(OwnerId);
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

        public void PromptAttackAndAttack(int playerId)
        {
            AttackBoardItem(PromptAttack(playerId));
        }

        public IBoardItem PromptAttack(int playerId)
        {
            var targets = GameState.GetOpponent(playerId).Board.GetAttackableTargets();
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
        }
    }
}
