using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem110 : ProblemBase {
        private List<ulong> _primes = new List<ulong>();
        private List<ulong> _powers = new List<ulong>();
        private ulong _count = 1;

        /*
            Solved this in excel. n = (d(z^2)+1) / 2 where d(z) is the number of divisors
            of n. The number of diviors for z can be found by multiplying the products of
            the powers of all the prime factors + 1. Or, if p^p1 are the prime factors, then
            d(z) = (p1 + 1)*(p2 + 1), etc. d(z^2) would then be (2p1 + 1)*(2p2 + 1), etc.
            Finally, if n = (d(z^2)+1)/2, then 2n = (d(z^2)+1). So then, we simply need
            to find the number where (2p1 + 1)*(2p2 + 1), etc is as close to
            8,000,000 as possible.

            Start with all 1's until we exceed 8,000,000. This will give us the first 15
            prime numbers. Then attempt to reduce the number by removing the last prime
            and converting it into 1, 2, 3, or more of the lowest primes. Do this until
            it cannot be done further.
         */

        public override string ProblemName {
            get { return "108: Diophantine reciprocals II"; }
        }

        public override string GetAnswer() {
            ulong max = 3000000;
            FindPrimes(max);
            return Solve(max).ToString();
        }

        private void FindPrimes(ulong max) {
            ulong num = 3;
            _primes.Add(2);
            _powers.Add(1);
            do {
                bool isPrime = true;
                ulong stop = (ulong)Math.Sqrt(num);
                for (ulong div = 2; div <= stop; div++) {
                    if (num % div == 0) {
                        isPrime = false;
                    }
                }
                if (isPrime) {
                    _primes.Add(num);
                    _powers.Add(1);
                    _count *= 3;
                }
                num += 2;
            } while (Math.Pow(3, _primes.Count) < max * 2);
        }

        private ulong Solve(ulong max) {
            int lastIndex = _primes.Count - 1;
            do {
                _primes[lastIndex] -= 1;
                _count /= 3;
                if (!Reduce(max)) {
                    _primes[lastIndex] += 1;
                    _count *= 3;
                    break;
                }
                lastIndex--;
            } while (true);
            ulong answer = 1;
            for (int index = 0; index < _primes.Count; index++) {
                answer *= (ulong)Math.Pow(_primes[index], _powers[index]);
            }
            return answer;
        }

        private bool Reduce(ulong max) {
            //int reduceBy = 1;
            //do {

            //} while (true);
            return false;
        }

        //private bool Reduce(ulong max, int remaining, int lastIndex) {
        //    for (int index = lastIndex; index < _primes.Count; index++) {
        //        _powers[index] += 1;

        //        _powers[index] -= 1;
        //    }
        //}

    }
}
