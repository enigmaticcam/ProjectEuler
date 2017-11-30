using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem323 : ProblemBase {
        private Random _random = new Random();

        public override string ProblemName {
            get { return "323: Bitwise-OR operations on random integers"; }
        }

        public override string GetAnswer() {
            return Solve().ToString();
        }

        private decimal Solve() {
            decimal totalCount = 10000000;
            decimal sum = 0;
            for (int count = 1; count <= totalCount; count++) {
                sum += PerformRandom();
            }
            return sum / totalCount;
        }

        private decimal PerformRandom() {
            decimal count = 0;
            int last = 99;
            do {
                count++;
                //uint thirtyBits = (uint)_random.Next(1 << 30);
                //uint twoBits = (uint)_random.Next(1 << 2);
                //uint num = (thirtyBits << 2) | twoBits;
                //last = num | last;
                //if (last == uint.MaxValue) {
                //    return count;
                //}
                int num = _random.Next(1, 6);
                if (num == 1) {
                    return count;
                } else if (num > last) {
                    return count;
                }
                last = num;
            } while (true);
        }
    }
}
