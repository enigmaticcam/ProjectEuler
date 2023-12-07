using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem07 : AdventOfCodeBase
    {
        private Dictionary<char, int> _cardValues;
        public override string ProblemName => "Advent of Code 2023: 7";

        public override string GetAnswer()
        {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2()
        {
            return Answer2(Input()).ToString();
        }

        private ulong Answer1(List<string> input)
        {
            SetCardValues();
            var hands = GetHands(input);
            SetHandTypes(hands, false);
            var first = GetHighest(hands);
            return GetScore(first);
        }
        private ulong Answer2(List<string> input)
        {
            SetCardValues();
            ResetJ();
            var hands = GetHands(input);
            SetHandTypes(hands, true);
            var first = GetHighest(hands);
            return GetScore(first);
        }

        private void ResetJ()
        {
            _cardValues['J'] = 0;
        }

        private ulong GetScore(List<Hand> hands)
        {
            ulong score = 0;
            ulong index = 1;
            foreach (var hand in hands)
            {
                score += (ulong)hand.Bet * index;
                index++;
            }
            return score;
        }

        private List<Hand> GetHighest(List<Hand> hands)
        {
            hands = hands
                .OrderBy(x => (int)x.HandType)
                .ThenBy(x => _cardValues[x.Cards[0]])
                .ThenBy(x => _cardValues[x.Cards[1]])
                .ThenBy(x => _cardValues[x.Cards[2]])
                .ThenBy(x => _cardValues[x.Cards[3]])
                .ThenBy(x => _cardValues[x.Cards[4]])
                .ToList();
            return hands;
        }

        private void SetHandTypes(List<Hand> hands, bool hasJoker)
        {
            foreach (var hand in hands)
            {
                _best = enumHandType.High;
                if (hasJoker)
                {
                    BruteForce(hand.Cards.ToArray(), 0);
                    hand.HandType = _best;
                }
                else
                {
                    hand.HandType = SetHandType(hand.Cards.ToArray());
                }
            }
        }

        private enumHandType _best;
        private void BruteForce(char[] cards, int index)
        {
            if (index == 5)
            {
                var next = SetHandType(cards);
                if ((int)next > (int)_best)
                    _best = next;
            } else
            {
                if (cards[index] == 'J')
                {
                    foreach (var key in _cardValues.Keys)
                    {
                        if (key != 'J')
                        {
                            cards[index] = key;
                            BruteForce(cards, index + 1);
                        }
                    }
                    cards[index] = 'J';
                } else
                {
                    BruteForce(cards, index + 1);
                }
            }
        }

        private enumHandType SetHandType(char[] cards)
        {
            var hash = cards
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
            return SetHandType(hash);
        }

        private enumHandType SetHandType(Dictionary<char, int> hash)
        {
            if (hash.Values.Any(x => x == 5))
                return enumHandType.Five;
            if (hash.Values.Any(x => x == 4))
                return enumHandType.Four;
            if (hash.Values.Any(x => x == 3) && hash.Values.Any(x => x == 2))
                return enumHandType.FullHouse;
            if (hash.Values.Any(x => x == 3))
                return enumHandType.Three;
            if (hash.Values.Where(x => x == 2).Count() == 2)
                return enumHandType.Two;
            if (hash.Values.Any(x => x == 2))
                return enumHandType.One;
            return enumHandType.High;
        }

        private List<Hand> GetHands(List<string> input)
        {
            var hands = new List<Hand>();
            foreach (var line in input)
            {
                var hand = new Hand();
                hands.Add(hand);
                var split = line.Split(' ');
                hand.Cards = split[0];
                hand.Bet = Convert.ToInt32(split[1]);
            }
            return hands;
        }

        private void SetCardValues()
        {
            _cardValues = new Dictionary<char, int>()
            {
                { '2', 1 },
                { '3', 2 },
                { '4', 3 },
                { '5', 4 },
                { '6', 5 },
                { '7', 6 },
                { '8', 7 },
                { '9', 8 },
                { 'T', 9 },
                { 'J', 10 },
                { 'Q', 11 },
                { 'K', 12 },
                { 'A', 13 }
            };
        }

        private class Hand
        {
            public string Cards { get; set; }
            public enumHandType HandType { get; set; }
            public int Bet { get; set; }
        }

        private enum enumHandType
        {
            High,
            One,
            Two,
            Three,
            FullHouse,
            Four,
            Five,
        }
    }
}
