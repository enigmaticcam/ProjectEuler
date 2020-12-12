using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2019 {
    public class Problem01 : AdventOfCodeBase {
        public override string ProblemName {
            get { return "Advent of Code 2019: 1"; }
        }

        public override string GetAnswer() {
            return Answer2().ToString();
        }

        public ulong Answer1() {
            var input = GetNumbers();
            ulong sum = 0;
            input.ForEach(x => sum += x / 3 - 2);
            return sum;
        }

        public ulong Answer2() {
            var input = GetNumbers();
            ulong sum = 0;
            input.ForEach(x => sum += CalcFuel(x));
            return sum;
        }

        private ulong CalcFuel(ulong mass) {
            var fuel = mass / 3;
            if (fuel < 2) {
                return 0;
            } else {
                fuel -= 2;
                return fuel + CalcFuel(fuel);
            }
        }

        private List<ulong> GetNumbers() {
            return Input().Select(x => Convert.ToUInt64(x)).ToList();
        }
    }
}
