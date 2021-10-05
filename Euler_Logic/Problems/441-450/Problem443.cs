using Euler_Logic.Helpers;
using System;

namespace Euler_Logic.Problems {
    public class Problem443 : ProblemBase {
        private GCDULong _gcd = new GCDULong();

        /*
            Consider the difference between value and n + 1. If that difference is a prime number, then the values for the remaining n until
            n equals that difference will all increase by 1. We can then skip them all and just calculate the last one. If the difference
            is not a prime number, just increase it by one until the GCD is not 1. Continue to do this until we reach the max.
         */

        public override string ProblemName {
            get { return "443: GCD sequence"; }
        }

        public override string GetAnswer() {
            return Solve((ulong)Math.Pow(10, 15)).ToString();
        }

        private ulong Solve(ulong max) {
            ulong n = 4;
            ulong value = 13;
            do {
                ulong diff = value - (n + 1);
                if (PrimalityULong.IsPrime(diff)) {
                    if (diff > max) {
                        return max - n + value;
                    } else {
                        value += n - 2 + diff;
                        n = diff;
                    }
                } else {
                    do {
                        n++;
                        var gcd = _gcd.GCD(n, value);
                        value += gcd;
                        if (gcd != 1) {
                            break;
                        }
                    } while (true);
                }
            } while (true);
        }
    }
}