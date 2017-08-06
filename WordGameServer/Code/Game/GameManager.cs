using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WordGameServer.Code.Lobby;
using WordGameServer.Code.Lobby.Models;
using WordGameServer.Models;

namespace WordGameServer.Code.Game
{
    public class GameManager
    {
        private static GameManager _instance = new GameManager();
        public static GameManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private object _lock = new object();
        ConcurrentDictionary<int, WordGame> avaliableGames = new ConcurrentDictionary<int, WordGame>();// <userName, connectionId>

        internal void CreateGame(Room room)
        {
            room.CloseRoom();
            WordGame game = new WordGame(room.Members);
            avaliableGames.TryAdd(game.GameId, game);
            LobbyManager.Instance.RemoveRoomFromList(room);
            game.StartGame();
        }

        internal GameModel GetGameInfo(int gameId)
        {
            WordGame wordGame;
            avaliableGames.TryGetValue(gameId, out wordGame);
            return wordGame.GameModel;
        }

        internal void WordGuessed(string userName, int gameId, string word, int score)
        {
            WordGame wordGame;
            avaliableGames.TryGetValue(gameId, out wordGame);

            if (wordGame == null) return;
            wordGame.WordGuessed(userName, word, score);
        }

        internal void BetPlaced(string userName, int gameId, int betAmount)
        {
            WordGame wordGame;
            avaliableGames.TryGetValue(gameId, out wordGame);

            if (wordGame == null) return;
            wordGame.BetPlaced(userName, betAmount);
        }
    }
}