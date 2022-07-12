using System.Collections.Generic;
using System.Numerics;

namespace Euler_Logic.Problems {
    public class Problem20 : ProblemBase {
        private List<int> _digits = new List<int>();

        public override string ProblemName {
            get { return "20: Factorial digit sum"; }
        }

        public override string GetAnswer() {
            return Solve().ToString();
        }

        private BigInteger Solve() {
            BigInteger num = 2;
            for (BigInteger next = 3; next <= 100; next++) {
                num *= next;
            }
            BigInteger sum = 0;
            do {
                sum += num % 10;
                num /= 10;
            } while (num != 0);
            return sum;
        }
    }
}
