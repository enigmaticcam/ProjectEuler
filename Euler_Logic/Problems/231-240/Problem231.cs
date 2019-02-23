using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem231 : ProblemBase {
        private PrimeSieve _primes;

        /*
            The equation for binomial coefficient (m,n) is: m! / (n! - (m - n)!). If I can find the prime factors of m!, 
            n!, and (m - n)!, all I have to do is start with the prime factors of m! and subtract the prime factors of
            n! and (m - n)!.

            To get the prime factors of a factorial (x): starting with some prime (p), n = x / p. If (n) > 0, then
            try n = x / p^2. If n is still > 0, try n = x / p^3. Continue until n = 0. The sum of all (n) is thus
            the powers of prime (p).
         */

        public override string ProblemName {
            get { return "231: The prime factorisation of binomial coefficients"; }
        }

        public override string GetAnswer() {
            ulong x = 20000000;
            ulong y = 15000000;
            _primes = new PrimeSieve(x);
            return Solve(x, y).ToString();
        }

        private ulong Solve(ulong x, ulong y) {
            ulong sum = 0;
            var n = PrimeFactorOfFactorial(x);
            var m = PrimeFactorOfFactorial(y);
            var nMinusM = PrimeFactorOfFactorial(x - y);
            Subtract(n, m);
            Subtract(n, nMinusM);
            foreach (var prime in n.Keys) {
                sum += prime * n[prime];
            }
            return sum;
        }

        private Dictionary<ulong, ulong> PrimeFactorOfFactorial(ulong num) {
            var factors = new Dictionary<ulong, ulong>();
            foreach (var prime in _primes.Enumerate) {
                ulong remainder = num / prime;
                if (remainder > 0) {
                    ulong power = 2;
                    ulong count = remainder;
                    while (remainder > 0) {
                        remainder = num / (ulong)Math.Pow(prime, power);
                        count += remainder;
                        power++;
                    }
                    factors.Add(prime, count);
                } else {
                    break;
                }
            }
            return factors;
        }

        private void Subtract(Dictionary<ulong, ulong> x, Dictionary<ulong, ulong> y) {
            foreach (var key in y.Keys) {
                x[key] -= y[key];
            }
        }
    }
}