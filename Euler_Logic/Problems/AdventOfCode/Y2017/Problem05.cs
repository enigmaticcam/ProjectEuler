using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem05 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2017: 5"; }
        }

        public override string GetAnswer() {
            return Answer1(Input());
        }

        public override string GetAnswer2() {
            return Answer2(Input());
        }

        private string Answer1(List<string> input) {
            var jumps = input.Select(x => Convert.ToInt32(x)).ToList();
            int position = 0;
            int count = 0;
            do {
                int newPosition = position + jumps[position];
                jumps[position]++;
                position = newPosition;
                count++;
            } while (position < jumps.Count);
            return count.ToString();
        }

        private string Answer2(List<string> input) {
            var jumps = input.Select(x => Convert.ToInt32(x)).ToList();
            int position = 0;
            int count = 0;
            do {
                int newPosition = position + jumps[position];
                if (jumps[position] >= 3) {
                    jumps[position]--;
                } else {
                    jumps[position]++;
                }
                position = newPosition;
                count++;
            } while (position < jumps.Count);
            return count.ToString();
        }
    }
}
