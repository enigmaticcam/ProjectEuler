using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem09 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2017: 9"; }
        }

        public override string GetAnswer() {
            return Answer1(Input());
        }

        public override string GetAnswer2() {
            return Answer2(Input());
        }

        private string Answer1(List<string> input) {
            string text = input[0];
            int index = 0;
            int groupCount = 0;
            bool isInGarbage = false;
            bool ignoreNext = false;
            int sum = 0;
            do {
                if (!ignoreNext) {
                    string character = text.Substring(index, 1);
                    if (!isInGarbage) {
                        switch (character) {
                            case "{":
                                groupCount++;
                                break;
                            case "}":
                                sum += groupCount;
                                groupCount--;
                                break;
                            case "<":
                                isInGarbage = true;
                                break;
                            case ">":
                                isInGarbage = false;
                                break;
                            case "!":
                                ignoreNext = true;
                                break;
                        }
                    } else {
                        switch (character) {
                            case ">":
                                isInGarbage = false;
                                break;
                            case "!":
                                ignoreNext = true;
                                break;
                        }
                    }
                } else {
                    ignoreNext = false;
                }
                index++;
            } while (index < text.Length);
            return sum.ToString();
        }

        private string Answer2(List<string> input) {
            string text = input[0];
            int index = 0;
            bool isInGarbage = false;
            bool ignoreNext = false;
            int sum = 0;
            do {
                if (!ignoreNext) {
                    string character = text.Substring(index, 1);
                    if (!isInGarbage) {
                        switch (character) {
                            case "<":
                                isInGarbage = true;
                                break;
                            case "!":
                                ignoreNext = true;
                                break;
                        }
                    } else {
                        switch (character) {
                            case ">":
                                isInGarbage = false;
                                break;
                            case "!":
                                ignoreNext = true;
                                break;
                            default:
                                sum++;
                                break;
                        }
                    }
                } else {
                    ignoreNext = false;
                }
                index++;
            } while (index < text.Length);
            return sum.ToString();
        }
    }
}
