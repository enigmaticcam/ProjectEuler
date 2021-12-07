using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2021 {
    public class Problem07 : AdventOfCodeBase {
        private List<int> _crabs;

        public override string ProblemName {
            get { return "Advent of Code 2021: 07"; }
        }

        public override string GetAnswer() {
            GetCrabs(Input());
            return Answer2().ToString();
        }

        private int Answer1() {
            return TryAll(true);
        }

        private int Answer2() {
            return TryAll(false);
        }

        private int TryAll(bool isConstant) {
            int min = _crabs.Min();
            int max = _crabs.Max();
            int best = int.MaxValue;
            for (int num = min; num <= max; num++) {
                var fuel = 0;
                foreach (var crab in _crabs) {
                    if (isConstant) {
                        fuel += Math.Abs(crab - num);
                    } else {
                        fuel += GetDiff(crab, num);
                    }
                }
                if (fuel < best) {
                    best = fuel;
                }
            }
            return best;
        }

        private int GetDiff(int start, int end) {
            int diff = Math.Abs(start - end);
            int numToAdd = (diff / 2) * (diff + 1);
            if (diff % 2 == 1) {
                numToAdd += diff / 2 + 1;
            }
            return numToAdd;
        }

        private void GetCrabs(List<string> input) {
            _crabs = input[0].Split(',').Select(x => Convert.ToInt32(x)).ToList();
        }
    }
}
