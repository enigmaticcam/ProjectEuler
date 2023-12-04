using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem04 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 4";

        public override string GetAnswer()
        {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2()
        {
            return Answer2(Input()).ToString();
        }

        public int Answer1(List<string> input)
        {
            var cards = GetCards(input);
            return GetScore(cards);
        }

        public ulong Answer2(List<string> input)
        {
            var cards = GetCards(input);
            return GetCopies(cards);
        }

        private ulong GetCopies(List<Card> cards)
        {
            ulong total = 0;
            cards.ForEach(x => x.Copies = 1);
            for (int index = 0; index < cards.Count; index++)
            {
                var card = cards[index];
                total += card.Copies;
                card.Points = GetScore(card, false);
                for (int count = 1; count <= card.Points && count < cards.Count; count++)
                {
                    cards[count + index].Copies += card.Copies;
                }
            }
            return total;
        }

        private int GetScore(List<Card> cards)
        {
            int score = 0;
            foreach (var card in cards)
            {
                score += GetScore(card, true);
            }
            return score;
        }
        
        private int GetScore(Card card, bool doubleByTwo)
        {
            int points = 0;
            foreach (var mine in card.Mine)
            {
                if (card.Winning.Contains(mine))
                {
                    if (points == 0)
                    {
                        points = 1;
                    }
                    else if (doubleByTwo)
                    {
                        points *= 2;
                    }
                    else
                    {
                        points++;
                    }
                }
            }
            return points;
        }

        private List<Card> GetCards(List<string> input)
        {
            var cards = new List<Card>();
            for (int index = 0; index < input.Count; index++)
            {
                var line = RemoveExtraSpaces(input[index]);
                var card = new Card() { Winning = new HashSet<int>(), Mine = new List<int>() };
                cards.Add(card);
                int colonIndex = line.IndexOf(':');
                card.Number = Convert.ToInt32(line.Substring(0, colonIndex).Split(' ')[1].Replace(":", ""));
                var split = line.Substring(colonIndex + 1).Split('|');
                card.Winning = GetNumbers(split[0]).ToHashSet();
                card.Mine = GetNumbers(split[1]);
            }
            return cards;
        }

        private List<int> GetNumbers(string line)
        {
            return line.Trim().Split(' ')
                .Select(x => Convert.ToInt32(x))
                .ToList();
        }

        private string RemoveExtraSpaces(string line)
        {
            int length;
            do
            {
                length = line.Length;
                line = line.Replace("  ", " ");
            } while (line.Length != length);
            return line;
        }

        private class Card
        {
            public HashSet<int> Winning { get; set; }
            public List<int> Mine { get; set; }
            public int Points { get; set; }
            public int Number { get; set; }
            public ulong Copies { get; set; }
        }
    }
}
