using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem501 : ProblemBase {
        private PrimeSieveWithPrimeListULong _primes = new PrimeSieveWithPrimeListULong();
        private PrimeCount _primeCount = new PrimeCount();
        private List<ulong> _found = new List<ulong>();

        /*
            What I figured out: all numbers having exaclty 8 divisors can be derived using prime numbers in 3 ways.
            (1) Given one prime (a), then a^7
            (2) Given two primes (a,b), then a^3*b
            (3) Given three primes (a,b,c), then a*b*c

            Finding all the primes up to 10^12 is silly. Even if you had them all, to loop through all possibilities would
            take a long time. However, if you had a fast way to count the number of primes up to a limit, then it would be
            possible. For example, if given two primes (a, b) and a = 2, then if 2^3 = 8, you can pair 8 with all
            primes up to 10^12 / 8.

            I borrowed a prime counting function from the internet to assist in this. Ultimately my implementation is terrible
            and takes 30 minutes, but whatever.
         */

        public override string ProblemName {
            get { return "501: Eight Divisors"; }
        }

        public override string GetAnswer() {
            return Solve((ulong)Math.Pow(10, 12)).ToString();
        }

        private ulong Solve(ulong max) {
            ulong count = 0;
            _primeCount.Count(max);

            // 1 prime
            ulong maxPowerOfSeven = (ulong)Math.Pow(max, (double)1 / 7);
            foreach (var prime in _primeCount.Enumerate) {
                if (prime <= maxPowerOfSeven) {
                    count++;
                }
            }

            // 2 primes
            for (int primeIndex = 0; primeIndex < _primeCount.Enumerate.Count(); primeIndex++) {
                ulong a = _primeCount.Enumerate.ElementAt(primeIndex);
                ulong maxB = max / (a * a * a);
                if (maxB <= 1) {
                    break;
                }
                count += _primeCount.Count(maxB);
                if (maxB >= a) {
                    count--;
                }
            }

            // 3 primes
            for (int primeIndex1 = 0; primeIndex1 < _primeCount.Enumerate.Count(); primeIndex1++) {
                var a = _primeCount.Enumerate.ElementAt(primeIndex1);
                if (a * a * a > max) {
                    break;
                }
                for (int primeIndex2 = primeIndex1 + 1; primeIndex2 < _primeCount.Enumerate.Count(); primeIndex2++) {
                    var b = _primeCount.Enumerate.ElementAt(primeIndex2);
                    var maxC = max / (a * b);
                    if (maxC <= b) {
                        break;
                    }

                    count += _primeCount.Count(maxC) - ((ulong)primeIndex2 + 1);
                }
            }


            return count;
        }

    }
}