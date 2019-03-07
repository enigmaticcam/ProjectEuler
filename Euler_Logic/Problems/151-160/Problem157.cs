using Euler_Logic.Helpers;
using System;

namespace Euler_Logic.Problems {
    public class Problem157 : ProblemBase {
        public override string ProblemName {
            get { return "157: Solving the diophantine equation 1/a+1/b= p/10n"; }
        }

        /*
            A silly algorithm that takes about 30 minutes.

            Suppose we're trying to find 1/3 + 1/x = p/10. 1/3 is equal to 0.33333. That means the
            very lowest value that (p) can possibly be is 4 (4/10 = 0.4). So 4/10 - 1/3 = 1/15. So 
            we know then that (3,15) is a valid pair.

            The next possible value for (p) would be 5. 5/10 - 1/3 = 1/6, so we know (3,6) is a valid
            pair. We continue to do this until 1/(1/p) < 3, or when (p) exceeds 2*10.

            This is basically what I do, except I use fractions instead of decimal points.
         */

        public override string GetAnswer() {
            return Solve(9).ToString();
        }

        private ulong Solve(int powerOf10Max) {
            ulong sum = 0;
            for (int power = 1; power <= powerOf10Max; power++) {
                sum += Count((ulong)Math.Pow(10, power));
            }
            return sum;
        }

        private ulong Count(ulong n) {
            ulong count = 0;
            for (ulong num = 1; num <= n * 2; num++) {
                ulong next = n / num + 1;
                ulong lcm = LCM.GetLCM(n, num);
                do {
                    ulong diff = (lcm / n * next) - (lcm / num);
                    if (diff > lcm || lcm / diff < num) {
                        break;
                    }
                    if (lcm % diff == 0) {
                        count++;
                    }
                    next++;
                } while (true);
            }
            return count;
        }
    }
}