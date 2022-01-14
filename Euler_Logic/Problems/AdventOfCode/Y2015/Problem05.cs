using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem05 : AdventOfCodeBase {

        public override string ProblemName {
            get { return "Advent of Code 2015: 5"; }
        }

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            int count = 0;
            input.ForEach(x => count += (IsNice1(x) ? 1 : 0));
            return count;
        }

        private int Answer2(List<string> input) {
            int count = 0;
            input.ForEach(x => count += (IsNice2(x) ? 1 : 0));
            return count;
        }

        private bool IsNice2(string text) {
            bool hasPair = false;
            bool hasSandwich = false;
            for (int index1 = 0; index1 < text.Length - 2; index1++) {
                var digit1 = text[index1];
                if (text[index1 + 2] == digit1) hasSandwich = true;
                if (!hasPair) {
                    var digit2 = text[index1 + 1];
                    for (int index2 = index1 + 2; index2 < text.Length - 1; index2++) {
                        if (text[index2] == digit1 && text[index2 + 1] == digit2) {
                            hasPair = true;
                            break;
                        }
                    }
                }
            }
            return hasPair & hasSandwich;
        }

        private bool IsNice1(string text) {
            int vowelCount = 0;
            int twiceInARow = 0;
            for (int index = 0; index < text.Length; index++) {
                var digit = text[index];
                if (digit == 'a' || digit == 'e' || digit == 'i' || digit == 'o' || digit == 'u') vowelCount++;
                if (index > 0) {
                    var prior = text[index - 1];
                    if (prior == digit) twiceInARow++;
                    if (prior == 'a' && digit == 'b') return false;
                    if (prior == 'c' && digit == 'd') return false;
                    if (prior == 'p' && digit == 'q') return false;
                    if (prior == 'x' && digit == 'y') return false;
                }
            }
            return vowelCount >= 3 && twiceInARow >= 1;
        }
    }
}
