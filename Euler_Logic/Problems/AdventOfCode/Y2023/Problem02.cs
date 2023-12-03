using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2023
{
    public class Problem02 : AdventOfCodeBase
    {
        public override string ProblemName => "Advent of Code 2023: 2";

        public override string GetAnswer()
        {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2()
        {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input)
        {
            var sets = GetSets(input);
            return FindValids(sets, new State()
            {
                Max = new Dictionary<string, int>()
                {
                    { "red", 12 },
                    { "green", 13 },
                    { "blue", 14 }
                }
            });
        }

        private ulong Answer2(List<string> input)
        {
            var sets = GetSets(input);
            return FindFewest(sets);
        }

        private ulong FindFewest(List<MarbleSet> sets)
        {
            ulong powerSum = 0;
            var hash = new Dictionary<string, int>();
            foreach (var game in sets)
            {
                hash.Clear();
                foreach (var set in game.Counts)
                {
                    foreach (var color in set)
                    {
                        if (!hash.ContainsKey(color.Item1))
                            hash.Add(color.Item1, color.Item2);
                        if (hash[color.Item1] < color.Item2)
                            hash[color.Item1] = color.Item2;
                    }
                }
                ulong prod = 1;
                foreach (var color in hash)
                {
                    prod *= (ulong)color.Value;
                }
                powerSum += prod;
            }
            return powerSum;
        }

        private int FindValids(List<MarbleSet> sets, State state)
        {
            int sum = 0;
            foreach (var game in sets)
            {
                sum += game.GameId;
                bool isGood = true;
                foreach (var set in game.Counts)
                {
                    foreach (var color in set)
                    {
                        if (state.Max[color.Item1] < color.Item2)
                        {
                            isGood = false;
                            break;
                        }
                    }
                    if (!isGood)
                        break;
                }
                if (!isGood)
                    sum -= game.GameId;
            }
            return sum;
        }

        private List<MarbleSet> GetSets(List<string> input)
        {
            var sets = new List<MarbleSet>();
            foreach (var line in input)
            {
                var set = GetSet(line);
                sets.Add(set);
            }
            return sets;
        }

        private MarbleSet GetSet(string line)
        {
            var final = new MarbleSet() { Counts = new List<List<Tuple<string, int>>>() };
            var endGameIndex = line.IndexOf(':');
            var gameSplit = line.Substring(0, endGameIndex).Split(' ');
            final.GameId = Convert.ToInt32(gameSplit[1]);
            var setSplit = line.Substring(endGameIndex + 1).Split(';');
            foreach (var set in setSplit)
            {
                var subList = new List<Tuple<string, int>>();
                final.Counts.Add(subList);
                var colorSplit = set.Split(',');
                foreach (var color in colorSplit)
                {
                    var wordSplit = color.Trim().Split(' ');
                    var finalCount = Convert.ToInt32(wordSplit[0]);
                    var finalColor = wordSplit[1];
                    subList.Add(new Tuple<string, int>(finalColor, finalCount));
                }
            }
            return final;
        }

        private class MarbleSet
        {
            public int GameId { get; set; }
            public List<List<Tuple<string, int>>> Counts { get; set; }
            public bool IsInvalid { get; set; }
        }

        private class State
        {
            public Dictionary<string, int> Max { get; set; }
        }
    }
}
