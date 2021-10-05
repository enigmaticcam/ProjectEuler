using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem204 : ProblemBase {
        private List<List<ulong>> _powers = new List<List<ulong>>();

        /*
            First find all prime numbers up to type (n). Then save a list of all powers of each prime where
            the powers does not exceed 10^9. Finally, consider all possible combinations of each power
            where the product does not exceed 10^9.
         */

        public override string ProblemName {
            get { return "204: Generalised Hamming Numbers"; }
        }

        public override string GetAnswer() {
            ulong maxNum = (ulong)Math.Pow(10, 9);
            ulong maxPrime = 100;
            BuildPowers(maxPrime, maxNum);
            Solve(maxNum, 0, 1);
            return _count.ToString();
        }

        private void BuildPowers(ulong maxPrime, ulong maxNum) {
            PrimeSieve primes = new PrimeSieve(maxPrime);
            foreach (var prime in primes.Enumerate) {
                List<ulong> powers = new List<ulong>() { 1 };
                ulong num = prime;
                do {
                    powers.Add(num);
                    num *= prime;
                } while (num <= maxNum);
                _powers.Add(powers);
            }
        }

        private ulong _count = 0;
        private void Solve(ulong maxNum, int primeIndex, ulong product) {
            foreach (var power in _powers[primeIndex]) {
                if (product * power <= maxNum) {
                    if (primeIndex < _powers.Count - 1) {
                        Solve(maxNum, primeIndex + 1, product * power);
                    } else {
                        _count++;
                    }
                } else {
                    break;
                }
            }
        }
    }
}