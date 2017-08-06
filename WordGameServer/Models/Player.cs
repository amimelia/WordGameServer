using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WordGameServer.DbModel;

namespace WordGameServer.Models
{
    public class Player
    {
        public user wordGameUser;
        public int money;
        public int points;
        public Card[] cards;
        public String guessedWord;
        public bool hasSubmittedWord;
        public int bet;
    }
}