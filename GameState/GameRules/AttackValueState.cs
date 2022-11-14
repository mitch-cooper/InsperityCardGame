using System;

namespace GameState.GameRules
{
    public class AttackValueState : ValueState
    {
        public AttackValueState(int value) : base(value)
        {
        }

        public AttackValueState(int baseValue, int currentValue) : base(baseValue, currentValue)
        {
        }

        public override int AddToCurrentValue(int valueChange)
        {
            CurrentValue = Math.Max(0, CurrentValue + valueChange);
            return CurrentValue;
        }

        public override int AddToBaseValue(int valueChange)
        {
            BaseValue = Math.Max(0, BaseValue + valueChange);
            CurrentValue = Math.Max(0, CurrentValue + valueChange);
            return BaseValue;
        }

        public override (string Value, ConsoleColor Color) GameToStringValues()
        {
            var color = ConsoleColor.White;
            if (BaseValue < CurrentValue)
            {
                color = ConsoleColor.DarkGreen;
            }
            return (CurrentValue.ToString(), color);
        }

        //public override void PrintGameToString()
        //{
        //    var values = GameToStringValues();
        //    ColorConsole.Write(values.Value, values.Color);
        //}
    }
}