using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WordGameServer.Models
{
    public class GameStates
    {
        public static int GAME_PENDING = 1;
        public static int GAME_BETTING_STARTED = 2;
        public static int GAME_BETTING_FINISHED = 3;
        public static int GAME_OVER = 5;
        public static int NEW_ROUND = 6;
        public static int ROUND_FINISHED = 4;
    }
}