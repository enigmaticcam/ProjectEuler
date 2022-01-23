using System;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem01 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2017: 1"; }
        }

        public override string GetAnswer2() {
            return Answer2(Input()[0]);
        }

        public override string GetAnswer() {
            return Answer1(Input()[0]);
        }

        private string Answer1(string text) {
            int sum = 0;
            for (int index = 0; index < text.Length; index++) {
                string character = text.Substring(index, 1);
                if (index == text.Length - 1) {
                    if (character == text.Substring(0, 1)) {
                        sum += Convert.ToInt32(character);
                    }
                } else if (character == text.Substring(index + 1, 1)) {
                    sum += Convert.ToInt32(character);
                }
            }
            return sum.ToString();
        }

        private string Answer2(string text) {
            int sum = 0;
            for (int index = 0; index < text.Length; index++) {
                int next = ((text.Length / 2) + index) % text.Length;
                if (text.Substring(index, 1) == text.Substring(next, 1)) {
                    sum += Convert.ToInt32(text.Substring(index, 1));
                }
            }
            return sum.ToString();
        }
    }
}
