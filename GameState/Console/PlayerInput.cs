using System;
using System.Collections.Generic;
using System.Text;

namespace GameState
{
    public class PlayerInput
    {
        public ConsoleKey Input { get; private set; }
        private string _display;

        public PlayerInput(ConsoleKey input, string display = null)
        {
            Input = input;
            _display = display ?? input.ToString();
        }

        public override string ToString()
        {
            return _display;
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
