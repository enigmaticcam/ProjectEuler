using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem02 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 2"; }
        }

        public override string GetAnswer() {
            return Answer1().ToString();
        }

        public override string GetAnswer2() {
            return Answer2().ToString();
        }

        public ulong Answer1() {
            var codes = GetCodes();
            int index = 0;
            codes[1] = 12;
            codes[2] = 2;
            do {
                var next = codes[index];
                if (next == 1) {
                    codes[codes[index + 3]] = codes[codes[index + 1]] + codes[codes[index + 2]];
                } else if (next == 2) {
                    codes[codes[index + 3]] = codes[codes[index + 1]] * codes[codes[index + 2]];
                } else if (next == 99) {
                    return codes[0];
                }
                index += 4;
            } while (true);
        }

        public ulong Answer2() {
            var codes = GetCodes();
            for (ulong code1 = 0; code1 <= 99; code1++) {
                for (ulong code2 = 0; code2 <= 99; code2++) {
                    var temp = codes.ToArray();
                    int index = 0;
                    temp[1] = code1;
                    temp[2] = code2;
                    do {
                        var next = temp[index];
                        if (next == 1) {
                            temp[temp[index + 3]] = temp[temp[index + 1]] + temp[temp[index + 2]];
                        } else if (next == 2) {
                            temp[temp[index + 3]] = temp[temp[index + 1]] * temp[temp[index + 2]];
                        } else if (next == 99) {
                            break;
                        }
                        index += 4;
                    } while (true);
                    if (temp[0] == 19690720) {
                        return 100 * code1 + code2;
                    }
                }
            }
            return 0;
        }

        private ulong[] GetCodes() {
            var input = Input().First().Split(',');
            var codes = new ulong[input.Length];
            for (int index = 0; index < input.Length; index++) {
                codes[index] = Convert.ToUInt64(input[index]);
            }
            return codes;
        }
    }
}
