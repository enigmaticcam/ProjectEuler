using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2017 {
    public class Problem04 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2017: 4"; }
        }

        public override string GetAnswer() {
            return Answer1(Input());
        }

        public override string GetAnswer2() {
            return Answer2(Input());
        }

        public string Answer1(List<string> input) {
            int count = 0;
            foreach (string line in input) {
                string[] codes = line.Split(' ');
                HashSet<string> hash = new HashSet<string>();
                bool isValid = true;
                foreach (string code in codes) {
                    if (hash.Contains(code)) {
                        isValid = false;
                        break;
                    } else {
                        hash.Add(code);
                    }
                }
                if (isValid) {
                    count++;
                }
            }
            return count.ToString();
        }

        public string Answer2(List<string> input) {
            int count = 0;
            foreach (string line in input) {
                string[] codes = line.Split(' ');
                bool isValid = true;
                for (int text1 = 0; text1 < codes.Length; text1++) {
                    for (int text2 = text1 + 1; text2 < codes.Length; text2++) {
                        if (IsAnagram(codes[text1], codes[text2])) {
                            isValid = false;
                            break;
                        }
                    }
                }
                if (isValid) {
                    count++;
                }
            }
            return count.ToString();
        }

        private bool IsAnagram(string text1, string text2) {
            if (text1.Length != text2.Length) {
                return false;
            }
            for (int index = 0; index < text1.Length; index++) {
                string digit = text1.Substring(index, 1);
                if (text1.Replace(digit, "").Length != text2.Replace(digit, "").Length) {
                    return false;
                }
            }
            return true;
        }
    }
}
