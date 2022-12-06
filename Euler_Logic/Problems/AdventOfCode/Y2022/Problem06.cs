using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2022 {
    public class Problem06 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2022: 6";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            return Find(input[0], 4);
        }

        private int Answer2(List<string> input) {
            return Find(input[0], 14);
        }

        private int Find(string text, int length) {
            for (int index = 0; index < text.Length - length; index++) {
                if (IsUnique(text, index, length)) {
                    return index + length;
                }
            }
            return -1;
        }

        private bool IsUnique(string text, int start, int length) {
            var hash = new HashSet<char>();
            for (int count = 1; count <= length; count++) {
                var digit = text[start + count - 1];
                if (hash.Contains(digit)) {
                    return false;
                }
                hash.Add(digit);
            }
            return true;
        }
    }
}
