using Euler_Logic.Helpers;
using System;

namespace Euler_Logic.Problems {
    public class Problem132 : ProblemBase {
        private PrimeSieveWithPrimeListULong _primes = new PrimeSieveWithPrimeListULong();

        /*
            I found that one easy way to determine if a prime is a factor of r(k)
            is to start at 1, multiply by 10, add 1, and return the remainder when
            divided by said prime. Continue to do this until you've done this k times.

            Knowing this, I was able to determine that as you you increment until you
            reach k, any prime will eventually resolve to a remainder of 0 at some k
            (with the exception of 5). If it takes 3 times to do this, then you know
            every 3rd k will have said prime as a factor.

            So I simply loop through all primes (p), and for each p I find how many
            iterations of k it takes to resolve to 0, then I check if 10^9 divided by
            k returns a remainder of 0. If it does, then that's a prime factor of
            r(10^9).
         */

        public override string ProblemName {
            get { return "132: Large repunit factors"; }
        }

        public override string GetAnswer() {
            _primes.SievePrimes(1000000);
            return Solve((ulong)Math.Pow(10, 9), 40).ToString();
        }

        private ulong Solve(ulong max, int factorCount) {
            int primeIndex = 1;
            int total = 0;
            ulong sum = 0;
            do {
                ulong prime = _primes[primeIndex];
                if (prime != 5) {
                    ulong remainder = 1;
                    ulong count = 1;
                    while (remainder != 0) {
                        count++;
                        remainder = (remainder * 10 + 1) % prime;
                    }
                    if (max % count == 0) {
                        total++;
                        sum += prime;
                        if (total == factorCount) {
                            return sum;
                        }
                    }
                }
                primeIndex++;
            } while (true);
        }
    }
}
