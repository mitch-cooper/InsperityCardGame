using System;
using System.Collections.Generic;
using System.Text;

namespace GameState
{
    public class PlayerInput
    {
        public ConsoleKey Input { get; }
        public ConsoleColor Color { get; set; }
        private readonly string _display;

        public PlayerInput(ConsoleKey input, string display = null, ConsoleColor? color = null)
        {
            Input = input;
            _display = display ?? input.ToString();
            Color = color ?? Constants.SecondaryActionColor;
        }

        public override string ToString()
        {
            return _display;
        }

        public string ToStringFormattedColor()
        {
            return ColorConsole.FormatEmbeddedColor(_display, Color);
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is PlayerInput item))
            {
                return false;
            }

            return Input.Equals(item.Input);
        }

        public override int GetHashCode()
        {
            return Input.GetHashCode();
        }
    }
}
