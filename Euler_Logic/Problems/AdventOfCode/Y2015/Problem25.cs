using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems.AdventOfCode.Y2015 {
    public class Problem25 : AdventOfCodeBase {
        public override string ProblemName => "Advent of Code 2015: 25";

        public override string GetAnswer() {
            return Answer1().ToString();
        }

        private ulong Answer1() {
            return Find(3083, 2978);
        }

        private ulong Find(int findX, int findY) {
            int diagonal = 2;
            ulong num = 20151125;
            do {
                for (int a = 1; a <= diagonal; a++) {
                    int x = a;
                    int y = (diagonal - a) + 1;
                    num = (num * 252533) % 33554393;
                    if (x == findX && y == findY) {
                        return num;
                    }
                }
                diagonal++;
            } while (true);
        }
    }
}
