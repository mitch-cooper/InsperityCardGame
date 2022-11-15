using System;

namespace GameState.GameRules
{
    public class HealthValueState : ValueState
    {
        public HealthValueState(int value) : base(value)
        {
        }

        public HealthValueState(int baseValue, int currentValue) : base(baseValue, currentValue)
        {
        }

        public override int AddToCurrentValue(int valueChange)
        {
            CurrentValue = Math.Min(CurrentValue + valueChange, BaseValue);
            return CurrentValue;
        }

        public override int AddToBaseValue(int valueChange)
        {
            if (valueChange > 0)
            {
                BaseValue += valueChange;
                CurrentValue += valueChange;
            }
            else if (valueChange < 0)
            {
                if (Math.Abs(valueChange) >= BaseValue)
                {
                    BaseValue = 1;
                    CurrentValue = 1;
                }
                else
                {
                    BaseValue -= valueChange;
                    if (BaseValue < CurrentValue)
                    {
                        CurrentValue = BaseValue;
                    }
                }
            }

            return BaseValue;
        }

        public override (string Value, ConsoleColor Color) GameToStringValues()
        {
            var color = ConsoleColor.White;
            if (BaseValue > CurrentValue)
            {
                color = ConsoleColor.DarkRed;
            }
            if (BaseValue < CurrentValue)
            {
                color = ConsoleColor.DarkGreen;
            }
            return (CurrentValue.ToString(), color);
        }
    }
}