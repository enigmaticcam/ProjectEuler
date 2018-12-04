using Euler_Logic.Helpers;
using System;

namespace Euler_Logic.Problems {
    public class Problem134 : ProblemBase {
        private PrimeSieveWithPrimeListULong _primes = new PrimeSieveWithPrimeListULong();

        /*
            Not an efficient algorithm, but gets the job done in about 4-5 minutes. Brute-force every consecutive prime (p1, p2).
            Starting with the initial remainder of p1 % p2, continue adding a power of 10 until the remainder when divided by p2
            is 0. To avoid potential of numbers larger than 64-bit, use only remainders.

            For example, if the primes are 19 and 23, the starting remainder would be 19 (19 % 23). The next number to test would
            be 119, which is the same as adding 100 to 19, which is the same as adding (100 % 23) to 19. If 100 % 23 is 8, then
            we simply add 8 to 19 and return the remainder when divided by 23. Continue to do this until the remainder is 0.
         */

        public override string ProblemName {
            get { return "134: Prime pair connection"; }
        }

        public override string GetAnswer() {
            _primes.SievePrimes(2000000);
            return Solve(1000000).ToString();
        }

        private ulong Solve(ulong max) {
            ulong sum = 0;
            for (int index = 2; index < _primes.Count - 1; index++) {
                var prime1 = _primes[index];
                if (prime1 > max) {
                    break;
                }
                var prime2 = _primes[index + 1];
                ulong digits = (ulong)Math.Log10(prime1);
                ulong baseTen = (ulong)Math.Pow(10, digits + 1);
                ulong remainder = prime1 % prime2;
                ulong offset = baseTen % prime2;
                ulong n = 0;
                do {
                    n++;
                    remainder = (remainder + offset) % prime2;
                } while (remainder != 0);
                sum += n * baseTen + prime1;
            }
            return sum;
        }
    }
}