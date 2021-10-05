using Euler_Logic.Helpers;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem429 : ProblemBase {
        private PrimeSieveWithPrimeListULong _primes = new PrimeSieveWithPrimeListULong();
        private Dictionary<ulong, ulong> _primePowers = new Dictionary<ulong, ulong>();

        /*
            According to this (http://mathworld.wolfram.com/UnitaryDivisorFunction.html), the sum of the squares of the unitary divisors 
            for N is: (1 + p1^2a1) * (1 + p2^2a2)...(1 + pn^2an), where p1-pn are the prime factors of N, and a1-an are the exponents of the prime
            factors. Since each component of this would be very large for 100000000!, we would need a way to do calculate x^y % z. This
            can be found here: https://en.wikipedia.org/wiki/Exponentiation_by_squaring. Now, all we need to do is prime factorize 100000000!.
            It turns out this is very easy and can be described here: https://janmr.com/blog/2010/10/prime-factors-of-factorial-numbers/.
         */

        public override string ProblemName {
            get { return "429: Sum of squares of unitary divisors"; }
        }

        public override string GetAnswer() {
            ulong max = 100000000;
            _primes.SievePrimes(max);
            BuildPrimePowers(max);
            return Solve().ToString();
        }

        private void BuildPrimePowers(ulong max) {
            foreach (var prime in _primes.Enumerate) {
                ulong sum = 0;
                ulong power = prime;
                ulong count = 0;
                do {
                    count = max / power;
                    sum += count;
                    power *= prime;
                } while (count > 0);
                _primePowers.Add(prime, sum);
            }
        }

        private ulong Solve() {
            ulong sum = 1;
            foreach (var prime in _primes.Enumerate) {
                sum = (sum * (1 + Exp(prime, _primePowers[prime] * 2, 1000000009))) % 1000000009;
            }
            return sum;
        }

        private ulong Exp(ulong num, ulong exponent, ulong mod) {
            if (exponent == 0) {
                return 1;
            } else if (exponent == 1) {
                return num % mod;
            } else if (exponent % 2 == 0) {
                return Exp((num * num) % mod, exponent / 2, mod);
            } else {
                return (num * Exp((num * num) % mod, (exponent - 1) / 2, mod)) % mod;
            }
        }
    }
}