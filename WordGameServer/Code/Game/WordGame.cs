using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using WordGameServer.Code.Communication.Events;
using WordGameServer.Models;

namespace WordGameServer.Code.Game
{
    public class WordGame
    {
        List<string> Members;
        public GameModel GameModel;
        public WordGame(List<string> members)
        {
            Members = members;
            GameId = GetNextGameId();
            CreateGameModel();
        }

        private void CreateGameModel()
        {
            GameModel = new GameModel();
            Card[] randomCards = CardGenerator.getRandomCards(7 + Members.Count * 2);
            Card[] cardsForAll = new Card[7];
            Array.Copy(randomCards, 0, cardsForAll, 0, 7);
            GameModel.cards = new List<Card>(cardsForAll);
            GameModel.players = new List<Player>();
            int stIndex = 7;
            foreach (var userName in Members)
            {
                Player curPlayer = new Player();
                curPlayer.wordGameUser = UsersManager.Instance.GetUserByName(userName);
                curPlayer.cards = new Card[2];
                curPlayer.cards[0] = randomCards[stIndex];
                curPlayer.cards[1] = randomCards[stIndex + 1];
                curPlayer.money = GameConstants.InitialStartAmount;
                curPlayer.points = 0;
                curPlayer.guessedWord = "";
                stIndex += 2;
                GameModel.players.Add(curPlayer);
            }

            GameModel.state = GameStates.NEW_ROUND;
            GameModel.roundNumber = 1;
            GameModel.secondsCount = 0;
        }

        private Player GetPlayerByName(string userName)
        {
            return GameModel.players.Where(player => player.wordGameUser.UserName == userName).FirstOrDefault();
        }

        private Player GetPlayerByName(GameModel model, string userName)
        {
            return model.players.Where(player => player.wordGameUser.UserName == userName).FirstOrDefault();
        }

        internal void WordGuessed(string userName, string word, int score)
        {
            if (GameModel.state == GameStates.GAME_PENDING)
            {
                var player = GetPlayerByName(userName);
                player.guessedWord = word;
                player.points = score;
            }
        }



        internal void BetPlaced(string userName, int betAmount)
        {
            if (GameModel.state == GameStates.GAME_PENDING)
            {
                var player = GetPlayerByName(userName);
                player.bet = betAmount;
            }
        }


        private static int _GameIdCounter = 1;
        private static object _GameIdCounterLock = new object();
        public static int GetNextGameId()
        {
            lock (_GameIdCounterLock)
            {
                _GameIdCounter++;
                return _GameIdCounter;
            }
        }

        public int GameId { get; internal set; }

        internal void StartGame()
        {
            GameEvent StartGameEvent = new GameEvent();
            StartGameEvent.EventKey = EventOptionTypes.GAME_CREATE_EVENT;
            StartGameEvent.EventAuthor = "Server";
            StartGameEvent.EventData = GameId;
            foreach(var player in Members)
            {
                Communication.CommunicationManager.Instance.SendEventToClient(player, StartGameEvent);
            }

            Thread t = new Thread(new ThreadStart(GameProcess));
            t.Start();
        }

        private void GameProcess()
        {
            while (Members.Count > 1)
            {
                if (Members.Count == 0)
                    return;
                int roundPopupTimer = 0;
                while (roundPopupTimer < 3)
                {
                    Thread.Sleep(1000);
                    roundPopupTimer++;
                }

                MoveToGamePending();

                int guessWordState = 0;
                while (guessWordState < 40)
                {
                    Thread.Sleep(1000);
                    guessWordState++;
                    GameModel.secondsCount++;
                }

                MoveToStartingBetting();
                Thread.Sleep(7000);
                int totalBet = 0;
                foreach (var player in GameModel.players)
                {
                    player.bet = Math.Min(player.bet, getMinPossibleBet());
                    totalBet += player.bet;
                    player.money -= player.bet;

                }

                List<Player> winnerPlayers = getWinnerPlayers();
                int winAmount = totalBet / winnerPlayers.Count;
                winnerPlayers.ForEach(player => player.money += winAmount);

                MoveToShowingResult();
                Thread.Sleep(5000);

                MoveToNextRound();
            }
            if (Members.Count == 0)
                return;
            CongratulateWinner();
            
        }

        private void MoveToNextRound()
        {
            FilterFinishedPlayers();
            GameModel prevModel = this.GameModel;
            CreateGameModel();
            GameModel.roundNumber = prevModel.roundNumber + 1;
            foreach (var userName in Members)
            {
                GetPlayerByName(userName).money = GetPlayerByName(prevModel, userName).money;
            }
        }

        private void FilterFinishedPlayers()
        {
            int nextMinAmount = (this.GameModel.roundNumber + 1) * 10;
            List<string> usersToRemove = new List<string>();
            foreach(var userName in Members)
            {
                if (GetPlayerByName(userName).money < nextMinAmount)
                {
                    usersToRemove.Add(userName);
                }
            }
            kickLosersOut(usersToRemove);
        }

        private void CongratulateWinner()
        {
            GameEvent updateEvent = new GameEvent();
            updateEvent.EventAuthor = "Server";
            updateEvent.EventKey = "game.live.event.game.won";
            updateEvent.EventData = GameModel;
            Communication.CommunicationManager.Instance.SendEventToClient(Members[0], updateEvent);
        }

        private void kickLosersOut(List<string> usersToRemove)
        {
           foreach(string userName in usersToRemove)
           {
                Members.Remove(userName);
                GameEvent updateEvent = new GameEvent();
                updateEvent.EventAuthor = "Server";
                updateEvent.EventKey = "game.live.event.game.lost";
                updateEvent.EventData = GameModel;
                Communication.CommunicationManager.Instance.SendEventToClient(userName, updateEvent);
            }
        }

        private List<Player> getWinnerPlayers()
        {
            int ScoreCoeff = 100000;
            int betCoeff = 1;
            int maxValue = GameModel.players.Max(player => player.bet * betCoeff + ScoreCoeff * player.points);
            return GameModel.players.Where(player => player.bet * betCoeff + ScoreCoeff * player.points == maxValue).ToList();
        }

        private int getMinPossibleBet()
        {
            return GameModel.roundNumber * 10;
        }

        private void MoveToGamePending()
        {
            this.GameModel.state = GameStates.GAME_PENDING;
            UpdatePlayersGui();
        }

        private void MoveToStartingBetting()
        {
            this.GameModel.state = GameStates.GAME_BETTING_STARTED;
            UpdatePlayersGui();
        }

        private void MoveToShowingResult()
        {
            this.GameModel.state = GameStates.GAME_BETTING_FINISHED;
            UpdatePlayersGui();
        }

        private void UpdatePlayersGui()
        {
            GameEvent updateEvent = new GameEvent();
            updateEvent.EventAuthor = "Server";
            updateEvent.EventKey = "game.live.event";
            updateEvent.EventData = GameModel;
            foreach (var player in Members)
            {
                Communication.CommunicationManager.Instance.SendEventToClient(player, updateEvent);
            }
        }
    }
}