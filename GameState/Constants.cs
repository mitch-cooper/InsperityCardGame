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
        public static readonly ConsoleKey EndTurnKey = ConsoleKey.E;
        public static readonly ConsoleKey YesKey = ConsoleKey.Y;
        public static readonly ConsoleKey NoKey = ConsoleKey.N;
        public static readonly ConsoleKey CancelKey = ConsoleKey.C;
        public static readonly ConsoleKey EventHistoryKey = ConsoleKey.H;

        public static ReadOnlyCollection<ConsoleKey> HandCardKeys = new ReadOnlyCollection<ConsoleKey>(new List<ConsoleKey>()
        {
            ConsoleKey.D1,
            ConsoleKey.D2,
            ConsoleKey.D3,
            ConsoleKey.D4,
            ConsoleKey.D5,
            ConsoleKey.D6,
            ConsoleKey.D7,
            ConsoleKey.D8
        });

        public static readonly ConsoleKey CurrentPlayerKey = ConsoleKey.P;
        public static ReadOnlyCollection<ConsoleKey> CurrentPlayersMinionKeys = new ReadOnlyCollection<ConsoleKey>(new List<ConsoleKey>()
        {
            ConsoleKey.F1,
            ConsoleKey.F2,
            ConsoleKey.F3,
            ConsoleKey.F4,
            ConsoleKey.F5,
            ConsoleKey.F6
        });

        public static readonly ConsoleKey OpponentPlayerKey = ConsoleKey.O;
        public static ReadOnlyCollection<ConsoleKey> OpponentsMinionKeys = new ReadOnlyCollection<ConsoleKey>(new List<ConsoleKey>()
        {
            ConsoleKey.F7,
            ConsoleKey.F8,
            ConsoleKey.F9,
            ConsoleKey.F10,
            ConsoleKey.F11,
            ConsoleKey.F12
        });

        #endregion
    }
}
