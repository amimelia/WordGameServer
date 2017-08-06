using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WordGameServer.Models
{
    public class Card
    {
        public String symbol;
        public int score;

        public Card()
        {

        }

        public Card(String symbol, int score)
        {
            this.symbol = symbol;
            this.score = score;
        }
    }
}