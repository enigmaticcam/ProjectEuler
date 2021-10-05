using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2016 {
    public class Problem07 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2016: 7";

        public override string GetAnswer() {
            return Answer2(Input()).ToString();
        }

        private int Answer1(List<string> input) {
            return CountTLS(input);
        }

        private int Answer2(List<string> input) {
            return CountSSL(input);
        }

        private int CountTLS(List<string> input) {
            int count = 0;
            foreach (var line in input) {
                bool inBrackets = false;
                bool isGood = false;
                for (int index = 0; index < line.Length - 3; index++) {
                    if (line[index] == '[') {
                        inBrackets = true;
                    } else if (line[index] == ']') {
                        inBrackets = false;
                    } else if (line[index] == line[index + 3] && line[index + 1] == line[index + 2] && line[index] != line[index + 1]) {
                        if (inBrackets) {
                            isGood = false;
                            break;
                        } else {
                            isGood = true;
                        }
                    }
                }
                if (isGood) {
                    count++;
                }
            }
            return count;
        }

        private int CountSSL(List<string> input) {
            int count = 0;
            foreach (var line in input) {
                bool inBrackets = false;
                var inRef = new HashSet<Tuple<char, char>>();
                var outRef = new HashSet<Tuple<char, char>>();
                for (int index = 0; index < line.Length - 2; index++) {
                    if (line[index] == '[') {
                        inBrackets = true;
                    } else if (line[index] == ']') {
                        inBrackets = false;
                    } else if (line[index] == line[index + 2] && line[index] != line[index + 1]) {
                        if (inBrackets) {
                            inRef.Add(new Tuple<char, char>(line[index], line[index + 1]));
                        } else {
                            outRef.Add(new Tuple<char, char>(line[index], line[index + 1]));
                        }
                    }
                }
                foreach (var isIn in inRef) {
                    var isOut = new Tuple<char, char>(isIn.Item2, isIn.Item1);
                    if (outRef.Contains(isOut)) {
                        count++;
                        break;
                    }
                }
            }
            return count;
        }

        private List<string> Test1Input() {
            return new List<string>() {
                "abba[mnop]qrst",
                "abcd[bddb]xyyx",
                "aaaa[qwer]tyui",
                "ioxxoj[asdfgh]zxcvbn"
            };
        }

        private List<string> Test2Input() {
            return new List<string>() {
                "aba[bab]xyz",
                "xyx[xyx]xyx",
                "aaa[kek]eke",
                "zazbz[bzb]cdb"
            };
        }
    }
}
