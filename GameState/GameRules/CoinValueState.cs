using System;

namespace GameState.GameRules
{
    public class CoinValueState : ValueState
    {
        public CoinValueState(int value) : base(value)
        {
        }

        public CoinValueState(int baseValue, int currentValue) : base(baseValue, currentValue)
        {
        }

        public override int AddToCurrentValue(int valueChange)
        {
            CurrentValue = Math.Min(Constants.MaxCoins, Math.Max(0, CurrentValue + valueChange));
            return CurrentValue;
        }

        public override int AddToBaseValue(int valueChange)
        {
            BaseValue = Math.Min(Constants.MaxCoins, Math.Max(0, BaseValue + valueChange));
            return BaseValue;
        }

        public override (string Value, ConsoleColor Color) GameToStringValues()
        {
            var color = ConsoleColor.White;
            if (CurrentValue > 0)
            {
                color = ConsoleColor.DarkGreen;
            }
            if (CurrentValue == 0)
            {
                color = ConsoleColor.DarkRed;
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