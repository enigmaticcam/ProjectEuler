using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem268 : ProblemBase {
        public override string ProblemName {
            get { return "268: Counting numbers with at least four distinct prime factors less than 100"; }
        }

        public override string GetAnswer() {
            return "";
        }

        private PrimeSieve _primes;
        private void BruteForce(ulong max) {
            _primes = new PrimeSieve(99);
            BruteForceRecursive(max, 0, 0, new ulong[4], 1);
        }

        private void BruteForceRecursive(ulong max, int primeIndex, int countIndex, ulong[] primes, ulong product) {
            for (int index = primeIndex; index < _primes.Count; index++) {
                var prime = _primes[primeIndex];
                if (max / product <  prime) {
                    break;
                }
                primes[countIndex] = prime;
                if (countIndex < primes.Length - 1) {
                    BruteForceRecursive(max, primeIndex + 1, countIndex + 1, primes, product * prime);
                } else {

                }
            }
        }

        private void BruteForceFoundCombo(ulong[] primes, ulong product) {

        }
    }
}
