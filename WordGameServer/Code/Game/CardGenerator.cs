using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WordGameServer.Models;

namespace WordGameServer.Code.Game
{
    public class CardGenerator
    {

        private static Random random = new Random();

        public static Card[] getRandomCards(int cardsNum)
        {

            int vowelCount = (int)(cardsNum * 0.4);

            Card[] cards = new Card[cardsNum];

            for (int i = 0; i < vowelCount; i++)
            {
                int score = randomInRange(1, 9);
                int symbolIndex = randomInRange(0, 4);

                cards[i] = new Card("" + vowels[symbolIndex], score);
            }

            for (int i = vowelCount; i < cardsNum; i++)
            {
                int score = randomInRange(1, 9);
                int symbolIndex = randomInRange(0, 33);

                char ch = 'ა';

                if (symbolIndex == 33)
                {
                    ch = '*';
                    score = 0;
                }
                else ch += (char)symbolIndex;

                cards[i] = new Card("" + ch, score);
            }

            return cards;
        }

        private static char[] vowels = new char[] { 'ა', 'ე', 'ი', 'ო', 'უ' };

        private bool isvowel(char ch)
        {
            return (ch == 'ა' || ch == 'ე'
                    || ch == 'ი' || ch == 'ო' || ch == 'უ');
        }

        private static int randomInRange(int min, int max)
        {

            int diff = max - min + 1;

            int rand = random.Next();

            if (rand < 0) rand = -rand;

            rand = rand % diff;

            return rand + min;
        }
    }
}