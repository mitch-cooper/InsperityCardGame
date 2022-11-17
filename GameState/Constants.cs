using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using GameState.Cards;
using GameState.GameRules;

namespace GameState
{
    public static class Constants
    {
        public static Action<Guid> DoNothing => (playerId) => { };
        public static Action<IBoardItemCard, Guid> DoNothingBoardItemCard => (builder, playerId) => { };
        public static Action<ISpell, Guid> DoNothingSpell => (builder, playerId) => { };

        public static readonly int BaseDeckSize = 20;
        public static readonly int MaxHandSize = 8;
        public static readonly int MaxCoins = 10;
        public static readonly int MaxMinions = 6;
        public static readonly int BasePlayerHealth = 25;

        #region Keys
        public static readonly PlayerInput EndTurnKey = new PlayerInput(ConsoleKey.E);
        public static readonly PlayerInput YesKey = new PlayerInput(ConsoleKey.Y);
        public static readonly PlayerInput NoKey = new PlayerInput(ConsoleKey.N);
        public static readonly PlayerInput CancelKey = new PlayerInput(ConsoleKey.C);
        public static readonly PlayerInput EventHistoryKey = new PlayerInput(ConsoleKey.H);

        public static ReadOnlyCollection<PlayerInput> HandCardKeys = new ReadOnlyCollection<PlayerInput>(new List<PlayerInput>()
        {
            new PlayerInput(ConsoleKey.D1, "1"),
            new PlayerInput(ConsoleKey.D2, "2"),
            new PlayerInput(ConsoleKey.D3, "3"),
            new PlayerInput(ConsoleKey.D4, "4"),
            new PlayerInput(ConsoleKey.D5, "5"),
            new PlayerInput(ConsoleKey.D6, "6"),
            new PlayerInput(ConsoleKey.D7, "7"),
            new PlayerInput(ConsoleKey.D8, "8")
        });

        public static readonly PlayerInput CurrentPlayerKey = new PlayerInput(ConsoleKey.P);
        public static ReadOnlyCollection<PlayerInput> CurrentPlayersMinionKeys = new ReadOnlyCollection<PlayerInput>(new List<PlayerInput>()
        {
            new PlayerInput(ConsoleKey.F1),
            new PlayerInput(ConsoleKey.F2),
            new PlayerInput(ConsoleKey.F3),
            new PlayerInput(ConsoleKey.F4),
            new PlayerInput(ConsoleKey.F5),
            new PlayerInput(ConsoleKey.F6)
        });

        public static readonly PlayerInput OpponentPlayerKey = new PlayerInput(ConsoleKey.O);
        public static ReadOnlyCollection<PlayerInput> OpponentsMinionKeys = new ReadOnlyCollection<PlayerInput>(new List<PlayerInput>()
        {
            new PlayerInput(ConsoleKey.F7),
            new PlayerInput(ConsoleKey.F8),
            new PlayerInput(ConsoleKey.F9),
            new PlayerInput(ConsoleKey.F10),
            new PlayerInput(ConsoleKey.OemPlus, "+"),
            new PlayerInput(ConsoleKey.F12)
        });

        #endregion
    }
}
