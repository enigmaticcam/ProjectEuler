using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem01 : AdventOfCodeBase {

        public override string ProblemName {
            get { return "Advent of Code 2015: 1"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            int length = input[0].Length;
            var text = input[0].Replace("(", "");
            int count = length - text.Length;
            count -= text.Length;
            return count;
        }

        private int Answer2(List<string> input) {
            int position = 0;
            int count = 0;
            foreach (var digit in input[0]) {
                count++;
                if (digit == '(') {
                    position++;
                } else {
                    position--;
                }
                if (position == -1) return count;
            }
            return 0;
        }
    }
}
