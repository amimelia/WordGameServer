using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WordGameServer.Models
{
    public class GameModel
    {
        public List<Player> players;
        public List<Card> cards;
        public int roundNumber;
        public int state;
        public int secondsCount;
    }
}