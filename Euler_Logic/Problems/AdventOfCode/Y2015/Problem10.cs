using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem10 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2015: 10";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            return PlayGame(input[0], 40);
        }

        private int Answer2(List<string> input) {
            return PlayGame(input[0], 50);
        }

        private int PlayGame(string text, int turns) {
            for (int turn = 1; turn <= turns; turn++) {
                var next = new StringBuilder();
                var count = 1;
                for (int index = 1; index < text.Length; index++) {
                    if (text[index] == text[index - 1]) {
                        count++;
                    } else {
                        next.Append(count.ToString());
                        next.Append(text[index - 1]);
                        count = 1;
                    }
                }
                next.Append(count.ToString());
                next.Append(text.Last());
                text = next.ToString();
            }
            return text.Length;
        }

        private List<string> TestInput() {
            return new List<string>() { "1" };
        }
    }
}
