using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems.AdventOfCode.Y2018 {
    public class Problem01 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2018: 1"; }
        }

        public override string GetAnswer() {
            return Answer1();
        }

        public override string GetAnswer2() {
            return Answer2();
        }

        private string Answer1() {
            var input = Input();
            ulong result = 0;
            input.ForEach(x => {
                var addOrSubtract = x.Substring(0, 1);
                var num = Convert.ToUInt64(x.Substring(1, x.Length - 1));
                if (addOrSubtract == "+") {
                    result += num;
                } else if (addOrSubtract == "-") {
                    result -= num;
                }
            });
            return result.ToString();
        }

        private string Answer2() {
            var input = Input();
            ulong result = 0;
            var hash = new HashSet<ulong>();
            int index = 0;
            do {
                var next = input[index];
                var addOrSubtract = next.Substring(0, 1);
                var num = Convert.ToUInt64(next.Substring(1, next.Length - 1));
                if (addOrSubtract == "+") {
                    result += num;
                } else if (addOrSubtract == "-") {
                    result -= num;
                }
                if (hash.Contains(result)) {
                    return result.ToString();
                }
                hash.Add(result);
                index = (index + 1) % input.Count;
            } while (true);
        }
    }
}
