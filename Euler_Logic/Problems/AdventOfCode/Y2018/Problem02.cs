using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem02 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 2"; }
        }

        public override string GetAnswer() {
            return Answer2();
        }

        private string Answer1() {
            ulong two = 0;
            ulong three = 0;
            var input = Input();
            input.ForEach(x => {
                var hash = new Dictionary<char, ulong>();
                for (int index = 0; index < x.Length; index++) {
                    var ch = x[index];
                    if (!hash.ContainsKey(ch)) {
                        hash.Add(ch, 1);
                    } else {
                        hash[ch]++;
                    }
                }
                if (hash.Values.Where(y => y == 3).Count() > 0) {
                    three++;
                }
                if (hash.Values.Where(y => y == 2).Count() > 0) {
                    two++;
                }
            });
            return (three * two).ToString();
        }

        private string Answer2() {
            var input = Input();
            char[] diff = new char[input[0].Length - 1];
            for (int index1 = 0; index1 < input.Count; index1++) {
                var line1 = input[index1];
                for (int index2 = index1 + 1; index2 < input.Count; index2++) {
                    var line2 = input[index2];
                    int differCount = 0;
                    int diffIndex = 0;
                    for (int charIndex = 0; charIndex < line1.Length; charIndex++) {
                        if (line1[charIndex] != line2[charIndex]) {
                            differCount++;
                            if (differCount > 1) {
                                break;
                            }
                        } else {
                            diff[diffIndex] = line1[charIndex];
                            diffIndex++;
                        }
                    }
                    if (differCount == 1) {
                        return new string(diff);
                    }
                }
            }
            return "";
        }
    }
}
