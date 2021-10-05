using Euler_Logic.Helpers;
using System;

namespace Euler_Logic.Problems {
    public class Problem141 : ProblemBase {
        /*
            I loop through unique fractions, starting with 2/1 (where the GCD of the numerator and denominator
            is 1). For each fraction, I begin generating geometric sequences starting with 1. For example,
            2/1 starting with 1 would produce 1,2,4. I set the first number as remainder (r), the second
            as divisor (d), and the last as quotient (q). The actual number (x) would then be d * q + r = x;
            If (x) is a perfect square, then I found a progressive number. I continue to generate geometric
            sequences for each fraction until (x) is beyond 10^12, then I start over with a new fraction.

            Unfortuately I was not able to mathematically determine a limit. I merely set the max fraction
            to be 112/111 because I was unable to find any more progressive numbers using fractions higher
            than 112/x. Lucky me that was enough.
         */

        public override string ProblemName {
            get { return "141: Investigating progressive numbers, n, which are also square"; }
        }

        public override string GetAnswer() {
            ulong max = (ulong)Math.Pow(10, 12);
            return Solve(max).ToString();
        }

        private ulong Solve(ulong max) {
            ulong sum = 0;
            GCDULong gcd = new GCDULong();
            for (ulong a = 2; a <= 112; a++) {
                for (ulong b = 1; b < a; b++) {
                    if (gcd.GCD(a, b) == 1) {
                        sum += Count(a, b, max);
                    }
                }
            }
            return sum;
        }

        private ulong Count(ulong a, ulong b, ulong max) {
            ulong sum = 0;
            ulong count = 0;
            do {
                count++;
                ulong r = count;
                ulong d = r * a;
                if (d % b == 0) {
                    d /= b;
                    ulong q = d * a;
                    if (q % b == 0) {
                        q /= b;
                        ulong squared = d * q + r;
                        if (squared > max) {
                            break;
                        }
                        ulong root = (ulong)Math.Sqrt(squared);
                        if (root * root == squared) {
                            sum += squared;
                        }
                    }
                }
            } while (true);
            return sum;
        }
    }
}