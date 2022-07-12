using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem457 : ProblemBase {
        private PrimeSieve _primes;

        public override string ProblemName => "457: A polynomial modulo the square of a prime";

        public override string GetAnswer() {
            BruteForce(999);
            return "";
        }

        private void BruteForce(ulong max) {
            var primes = new PrimeSieve(max);
            var allPrimes = primes.Enumerate.ToList();
            var allGood = new List<ulong>();
            for (ulong n = 4; n <= max * max; n++) {
                var num = n * n - (3 * n) - 1;
                var subMax = (ulong)Math.Sqrt(num);
                bool found = false;
                ulong primeFound = 0;
                foreach (var prime in allPrimes) {
                    if (prime > subMax) break;
                    if (num % (prime * prime) == 0) {
                        allGood.Add(prime);
                        found = true;
                        primeFound = prime;
                        break;
                    }
                }
                if (found) allPrimes.Remove(primeFound);
            }
        }
    }
}
