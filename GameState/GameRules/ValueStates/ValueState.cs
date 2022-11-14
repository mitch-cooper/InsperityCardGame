﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameState.GameRules
{
    public abstract class ValueState : ConsoleGameToString
    {
        public int BaseValue { get; protected set; }
        public int CurrentValue { get; protected set; }

        protected ValueState(int value) : this(value, value)
        {
        }

        protected ValueState(int baseValue, int currentValue)
        {
            BaseValue = baseValue;
            CurrentValue = currentValue;
        }
        //public virtual int SetValue(int value)
        //{
        //    SetBaseValue(value);
        //    SetCurrentValue(value);
        //    return BaseValue;
        //}
        //public virtual int SetBaseValue(int value)
        //{
        //    BaseValue = value;
        //    return BaseValue;
        //}
        //public virtual int SetCurrentValue(int value)
        //{
        //    CurrentValue = value;
        //    return CurrentValue;
        //}
        public abstract int AddToCurrentValue(int valueChange);
        public abstract int AddToBaseValue(int valueChange);

        public virtual (string Value, ConsoleColor Color) GameToStringValues()
        {
            return (CurrentValue.ToString(), ConsoleColor.White);
        }

        public override string GameToString()
        {
            var values = GameToStringValues();
            return ColorConsole.FormatEmbeddedColor(values.Value, values.Color);
        }

        public override string GameToString(int currentPlayerId)
        {
            return GameToString();
        }

        public override void PrintGameToString()
        {
            var values = GameToStringValues();
            ColorConsole.Write(values.Value, values.Color);
        }

        public override void PrintGameToString(int currentPlayerId)
        {
            PrintGameToString();
        }
    }
}