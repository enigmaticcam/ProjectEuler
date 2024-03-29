﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem06 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2017: 6"; }
        }

        public override string GetAnswer() {
            return Answer1(Input());
        }

        public override string GetAnswer2() {
            return Answer2(Input());
        }

        private string Answer1(List<string> input) {
            var banks = input[0].Split('\t').Select(x => Convert.ToInt32(x)).ToList();
            HashSet<string> hash = new HashSet<string>();
            int count = 0;
            do {
                int highest = FindHighestCount(banks);
                int spread = banks[highest];
                int add = spread / banks.Count;
                int remainder = spread % banks.Count;
                int next = highest;
                banks[highest] = 0;
                for (int spreadIndex = 1; spreadIndex <= banks.Count; spreadIndex++) {
                    next = (next + 1) % banks.Count;
                    banks[next] += add;
                    if (remainder > 0) {
                        banks[next]++;
                        remainder = Math.Max(remainder - 1, 0);
                    }
                }
                count++;
                string key = string.Join(":", banks);
                if (hash.Contains(key)) {
                    return count.ToString();
                } else {
                    hash.Add(key);
                }
            } while (true);
        }

        private string Answer2(List<string> input) {
            var banks = input[0].Split('\t').Select(x => Convert.ToInt32(x)).ToList();
            Dictionary<string, int> hash = new Dictionary<string, int>();
            int count = 0;
            do {
                int highest = FindHighestCount(banks);
                int spread = banks[highest];
                int add = spread / banks.Count;
                int remainder = spread % banks.Count;
                int next = highest;
                banks[highest] = 0;
                for (int spreadIndex = 1; spreadIndex <= banks.Count; spreadIndex++) {
                    next = (next + 1) % banks.Count;
                    banks[next] += add;
                    if (remainder > 0) {
                        banks[next]++;
                        remainder = Math.Max(remainder - 1, 0);
                    }
                }
                count++;
                string key = string.Join(":", banks);
                if (hash.ContainsKey(key)) {
                    return (count - hash[key]).ToString();
                } else {
                    hash.Add(key, count);
                }
            } while (true);
        }

        private int FindHighestCount(List<int> banks) {
            int highestIndex = 0;
            int highestValue = int.MinValue;
            for (int index = 0; index < banks.Count; index++) {
                if (banks[index] > highestValue) {
                    highestValue = banks[index];
                    highestIndex = index;
                }
            }
            return highestIndex;
        }
    }
}
