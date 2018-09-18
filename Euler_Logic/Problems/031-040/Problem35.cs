using Euler_Logic.Helpers;
using System;

namespace Euler_Logic.Problems {
    public class Problem35 : ProblemBase {
        private PrimeSieveWithPrimeListUInt _primes = new PrimeSieveWithPrimeListUInt();

        /*
            For any given prime, we can find the next rotation by removing the first digit and putting it at the end.
            If your prime is x, the next rotation is: ((x - a) * 10) + b, where a is 10^n where n is the count of the digits
            of x, and b is the first digit of a.

            For example:

            x = 197
            ((197 - 100) * 10) + 1 = 971
            ((971 - 900) * 10) + 9 = 791

            x = 1193
            ((1193 - 1000) * 10 + 1 = 1931
            ((1931 - 1000) * 10 + 1 = 9311
            ((9311 - 9000) * 10 + 9 = 3119

            After deriving all rotations, we simply check if all of them are prime. Assume any prime less than 10 is automatically good.
            Do this for all primes up to 1 million.
         */

        public override string ProblemName {
            get { return "35: Circular primes"; }
        }

        public override string GetAnswer() {
            _primes.SievePrimes(1000000);
            return Solve().ToString();
        }

        private int Solve() {
            int sum = 0;
            foreach (var prime in _primes.Enumerate) {
                bool isGood = true;
                if (prime >= 10) {
                    uint num = prime;
                    uint max = (uint)Math.Log10(num);
                    uint size = (uint)Math.Pow(10, max);
                    for (int count = 0; count < max; count++) {
                        uint subract = (num / size) * size;
                        uint add = (subract / size);
                        num = ((num - subract) * 10) + add;
                        if (!_primes.IsPrime(num) || num < size) {
                            isGood = false;
                            break;
                        }
                    }
                }
                if (isGood) {
                    sum++;
                }
            }
            return sum;
        }
    }
}