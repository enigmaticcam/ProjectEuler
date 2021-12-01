using Euler_Logic.Helpers;
using System;

namespace Euler_Logic.Problems {
    public class Problem193 : ProblemBase {
        private ulong _sum = 0;
        private PrimeSieve _primes;

        /*
            I used the inclusion-exclusion principle. The count of all numbers less than 2^50 that are multiples of 2^2 is
            2^50 / 2^2. However, if we do the same for the next prime number, 2^50 / 2^3, we have to subtract 2^50 / (2^2 * 2^3).
            Basically do this for all primes as long as the product of the primes does not exceed 2^50.

            I ran into overflow issues on the testing if a new prime^2. So instead I tested if 2^50 / prime^2 is more than my
            current product. If not, then I can continue with the next prime.
         */

        public override string ProblemName {
            get { return "193: Squarefree Numbers"; }
        }

        public override string GetAnswer() {
            return Solve((ulong)Math.Pow(2, 50)).ToString();
        }

        private ulong Solve(ulong max) {
            _sum = 0;
            var rootMax = (ulong)Math.Sqrt(max);
            _primes = new PrimeSieve(rootMax);
            IncludeExlucde(true, 0, 1, max);
            return max - _sum;
        }

        private void IncludeExlucde(bool add, int startPrime, ulong prod, ulong max) {
            for (int index = startPrime; index < _primes.Count; index++) {
                var prime = _primes[index];
                prime *= prime;
                if (max / prime >= prod) {
                    if (add) {
                        var temp = _sum;
                        _sum += max / (prime * prod);
                    } else {
                        _sum -= max / (prime * prod);
                    }
                    IncludeExlucde(!add, index + 1, prime * prod, max);
                } else {
                    break;
                }
            }
        }
    }
}