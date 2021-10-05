using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem137 : ProblemBase {
        private List<ulong> _fibs = new List<ulong>();

        /*
            According to https://oeis.org/A081018, if the sequence of golden nuggets is a(n), then:
            a(n) = Fib(3) + Fib(7) + Fib(11) + ... + Fib(4n+3)

            So all we need to do is calculate the fib numbers up to (4 * 15 + 3) = 63, and then sum
            Fib(3) + Fib(7) + Fib(11) + Fib(15) + ... + Fib(63)
         */

        public override string ProblemName {
            get { return "137: Fibonacci golden nuggets"; }
        }

        public override string GetAnswer() {
            int max = 15;
            GenerateFibs(max);
            return Solve(max).ToString();
        }

        private void GenerateFibs(int max) {
            _fibs.Add(1);
            _fibs.Add(1);
            int stop = 4 * max + 3;
            for (int count = 3; count < stop; count++) {
                _fibs.Add(_fibs[count - 3] + _fibs[count - 2]);
            }
        }

        private ulong Solve(int max) {
            ulong sum = 0;
            for (int count = 0; count < max; count++) {
                int fibIndex = 4 * count + 3;
                sum += _fibs[fibIndex - 1];
            }
            return sum;
        }
    }
}