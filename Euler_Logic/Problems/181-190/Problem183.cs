using Euler_Logic.Helpers;
using System;

namespace Euler_Logic.Problems {
    public class Problem183 : ProblemBase {
        public override string ProblemName {
            get { return "183: Maximum Product of Parts"; }
        }

        /*
            Initially I solved this using big integer to calculate M(n). That took several minutes. However, after that I
            discovered that calculating M(n) is easily done using (e) constant. M(n) = n/E, round up if 0.5 or above.

            The rest is easy. Determining if the fraction is terminating or not is simply to determine if there are any
            other prime factors of the denominator besides 2 or 5. If so, then it's not terminating.
         */

        public override string GetAnswer() {
            return Solve(10000).ToString();
        }

        private ulong Solve(int maxN) {
            ulong result = 0;
            for (int n = 5; n <= maxN; n++) {
                result += (ulong)D(n);
            }
            return result;
        }

        private int D(int n) {
            var m = (int)M(n);
            return n * (IsTerminating(n, m) ? -1 : 1);
        }

        private int M(int n) {
            var x = Math.Round((double)n / Math.E, 0);
            return (int)x;
        }

        private bool IsTerminating(int x, int y) {
            var gcd = GCD.GetGCD(x, y);
            var z = y / gcd;
            while (z % 2 == 0) {
                z /= 2;
            }
            while (z % 5 == 0) {
                z /= 5;
            }
            return z == 1;
        }
    }
}