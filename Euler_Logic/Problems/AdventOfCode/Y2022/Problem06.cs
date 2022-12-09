using System.Collections.Generic;
using System.Linq;

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
            int moreThanOneCount = 0;
            var hash = new Dictionary<char, int>();
            for (int index = 0; index < length; index++) {
                var digit = text[index];
                if (!hash.ContainsKey(digit)) {
                    hash.Add(digit, 1);
                } else {
                    hash[digit]++;
                    if (hash[digit] == 2) moreThanOneCount++;
                }
            }
            for (int index = length; index < text.Length; index++) {
                var toRemove = text[index - length];
                var toAdd = text[index];
                if (hash[toRemove] == 2) moreThanOneCount--;
                hash[toRemove]--;
                if (!hash.ContainsKey(toAdd)) hash.Add(toAdd, 0);
                if (hash[toAdd] == 1) moreThanOneCount++;
                hash[toAdd]++;
                if (moreThanOneCount == 0) return index + 1;
            }
            return -1;
        }
    }
}
