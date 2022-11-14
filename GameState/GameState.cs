using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameState.Cards.Collection;
using GameState.GameRules;

namespace GameState
{
    public static class GameState
    {
        internal static Player Player1 { get; set; }
        internal static Player Player2 { get; set; }
        internal static TurnSystem TurnSystem { get; set; } = new TurnSystem();
        internal static Logger HistoryLog { get; set; } = new Logger();

        //public delegate void GameEventHandler(GameEvent gameEvent);
        //public static event GameEventHandler GameEventTriggered;

        internal static Player GetPlayer(int id)
        {
            return Player1.OwnerId == id ? Player1 : Player2.OwnerId == id ? Player2 : throw new Exception("Player not found");
        }

        internal static Player GetOpponent(int myPlayerId)
        {
            return Player1.OwnerId == myPlayerId ? Player2 : Player2.OwnerId == myPlayerId ? Player1 : throw new Exception("Player not found");
        }

        public static void StartGame(Player player1, Player player2)
        {
            if (player1 == null || player2 == null)
            {
                throw new Exception("Player(s) is/are null.");
            }

            Player1 = player1;
            Player2 = player2;

            Player1.ResetPlayer();
            Player2.ResetPlayer();

            HistoryLog.ResetHistory();
            TurnSystem.ResetTurns(Player1, Player2);
            TurnSystem.GameEventTriggered += HistoryLog.AddEvent;

            PrintGame();

            try
            {
                while (true)
                {
                    TurnSystem.StartTurn(TurnSystem.GoesFirst);
                    PrintGame();
                    TurnSystem.StartTurn(TurnSystem.GoesSecond);
                    PrintGame();
                }
            }
            catch (PlayerDiedException deadPlayer)
            {
                var winner = GetOpponent(deadPlayer.PlayerId);
                ColorConsole.WriteWrappedHeader($"{winner.Name} wins!");
                if (Prompts.WantToPlayAgain())
                {
                    StartGame(Player1, Player2);
                }
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }
        }

        public static void PrintGame()
        {
            Console.Clear();
            PrintPlayer(Player2, true);
            PrintDivider();
            PrintPlayer(Player1, false);
            ColorConsole.WriteLine(string.Empty);
            HistoryLog.PrintNthEvents(3);
            ColorConsole.WriteLine(string.Empty);
            ColorConsole.WriteLine(string.Empty);
        }

        private static void PrintDivider()
        {
            var colCount = 70;
            var lines = new List<string>()
            {
                new string('_', colCount),
                "___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|___|__",
                new string(' ', colCount)
            };

            foreach (var line in lines)
            {
                ColorConsole.WriteEmbeddedColorLine(line);
            }
        }

        private static void PrintPlayer(Player player, bool isTopOfScreen)
        {
            var spaceBaseValue = 37;
            var coins = player.GetCoinsDrawToConsoleLines();
            var playerLines = player.GetDrawToConsoleLines();
            var hand = player.Hand.GetDrawToConsoleLines();
            var linesCoinsPlayerHand = GameToConsoleHelper.InitializeListWithValue("", Math.Min(coins.Count, Math.Min(playerLines.Count, hand.Count)));
            for(var i = 0; i < linesCoinsPlayerHand.Count; i++)
            {
                var spaces = new string(' ', spaceBaseValue - 16 - hand[i].Length / 2);
                linesCoinsPlayerHand[i] = $"{coins[i]}{new string(' ', spaceBaseValue / 2)}{playerLines[i]}{spaces}{hand[i]}{spaces}";
            }

            var linesBoard = player.Board.GetDrawToConsoleLines();
            var deck = player.Deck.GetDrawToConsoleLines();
            var linesBoardDeck = GameToConsoleHelper.InitializeListWithValue("", Math.Min(linesBoard.Count, deck.Count));
            for (var i = 0; i < linesBoardDeck.Count; i++)
            {
                linesBoardDeck[i] = $"{linesBoard[i]}{deck[i]}";
            }

            var allLines = new List<string>();
            if (isTopOfScreen)
            {
                allLines.AddRange(linesCoinsPlayerHand);
                allLines.AddRange(linesBoardDeck);
            }
            else
            {
                allLines.AddRange(linesBoardDeck);
                allLines.AddRange(linesCoinsPlayerHand);
            }

            foreach (var line in allLines)
            {
                ColorConsole.WriteEmbeddedColorLine(line);
            }
        }
    }
}
