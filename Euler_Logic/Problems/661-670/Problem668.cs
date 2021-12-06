using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem668 : ProblemBase {
        public override string ProblemName {
            get { return "668: Square root Smooth Numbers"; }
        }

        public override string GetAnswer() {
            BruteForce(100);
            return "";
        }

        private void Solve(ulong max) {
            var primes = new PrimeSieve((ulong)Math.Sqrt(max));

        }

        private void Recursive(IEnumerable<ulong> primes, int primeIndex, ulong num, ulong max) {
            for (int nextIndex = primeIndex; nextIndex < primes.Count(); nextIndex++) {
                var prime = primes.ElementAt(nextIndex);
                if (prime * num > max) {
                    break;
                }
                var temp = num;
                do {
                    temp *= prime;
                    if (temp <= max) {
                        Recursive(primes, primeIndex + 1, temp, max);
                    } else {
                        break;
                    }
                } while (true);
            }
        }

        private List<ulong> _found;
        private void BruteForce(ulong max) {
            _found = new List<ulong>();
            var primes = new PrimeSieve(max);
            for (ulong num = 4; num <= max; num++) {
                if (!primes.IsPrime(num)) {
                    if (IsGood(num, primes)) {
                        _found.Add(num);
                    }
                }
            }
        }

        private bool IsGood(ulong num, PrimeSieve primes) {
            var root = (ulong)Math.Sqrt(num);
            if (root * root == num) {
                root -= 1;
            }
            var temp = num;
            foreach (var prime in primes.Enumerate) {
                if (prime > root) {
                    break;
                }
                while (temp % prime == 0) {
                    temp /= prime;
                }
                if (temp == 1) {
                    break;
                }
            }
            return temp == 1;
        }
    }
}
