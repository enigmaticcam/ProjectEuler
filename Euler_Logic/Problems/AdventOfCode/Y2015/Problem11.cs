using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem11 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2015: 11";

        public override string GetAnswer() {
            return Answer1(Input()).ToString();
        }

        public override string GetAnswer2() {
            return Answer2(Input()).ToString();
        }

        private string Answer1(List<string> input) {
            var text = input[0].ToCharArray();
            FindNext(text);
            return new string(text);
        }

        private string Answer2(List<string> input) {
            var text = input[0].ToCharArray();
            FindNext(text);
            FindNext(text);
            return new string(text);
        }

        private void FindNext(char[] text) {
            do {
                Increase(text);
                if (new string(text) == "ghjaabcc") {
                    bool sto = true;
                }
            } while (!IsGood(text));
        }

        private void Increase(char[] text) {
            int index = text.Length - 1;
            do {
                if (text[index] == 'z') {
                    text[index] = 'a';
                    index--;
                } else {
                    text[index]++;
                    if (text[index] == 'i') {
                        text[index]++;
                    } else if (text[index] == 'o') {
                        text[index]++;
                    } else if (text[index] == 'l') {
                        text[index]++;
                    }
                    break;
                }
            } while (true);
        }

        private bool IsGood(char[] text) {
            return HasIncreasingStraight(text) && DoesHaveTwoPairs(text) && DoesNotHaveBadLetters(text);
        }

        private bool HasIncreasingStraight(char[] text) {
            for (int index = 0; index < text.Length - 3; index++) {
                if (text[index] == text[index + 1] - 1 && text[index + 1] == text[index + 2] - 1) {
                    return true;
                }
            }
            return false;
        }

        private bool DoesNotHaveBadLetters(char[] text) {
            foreach (var digit in text) {
                if (digit == 'i' || digit == 'o' || digit == 'l') {
                    return false;
                }
            }
            return true;
        }

        private bool DoesHaveTwoPairs(char[] text) {
            char lastPair = ' ';
            for (int index = 0; index < text.Length - 1; index++) {
                if (text[index] == text[index + 1]) {
                    if (lastPair == ' ') {
                        lastPair = text[index];
                    } else if (lastPair != text[index]) {
                        return true;
                    }
                    index++;
                }
            }
            return false;
        }
    }
}
