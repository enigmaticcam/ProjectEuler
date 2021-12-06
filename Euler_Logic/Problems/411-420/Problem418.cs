using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem418 : ProblemBase {
        public override string ProblemName {
            get { return "418: Factorisation triples"; }
        }

        public override string GetAnswer() {
            FindPrimeFactors();
            return "";
        }

        private Dictionary<ulong, ulong> _primeFactors = new Dictionary<ulong, ulong>();
        private void FindPrimeFactors() {
            var primes = new PrimeSieve(43);
            foreach (var prime in primes.Enumerate) {
                ulong total = 0;
                ulong temp = prime;
                ulong count = 43 / prime;
                do {
                    total += count;
                    temp *= prime;
                    count = 43 / temp;
                } while (count > 0);
                _primeFactors.Add(prime, total);
            }
        }

        private void Solve() {
            for (int index1 = 0; index1 < _primeFactors.Keys.Count; index1++) {
                var prime1 = _primeFactors.Keys.ElementAt(index1);
                var count1 = _primeFactors[prime1];
                for (int index2 = index1 + 1; index2 < _primeFactors.Keys.Count; index2++) {
                    var prime2 = _primeFactors.Keys.ElementAt(index2);
                    var count2 = _primeFactors[prime2];
                    for (ulong power1 = 1; power1 <= count1; power1++) {
                        var num1 = (ulong)Math.Pow(prime1, power1);
                        for (ulong power2 = 1; power2 <= count2; power2++) {
                            var num2 = (ulong)Math.Pow(prime2, power2);
                            

                        }
                    }
                }
            }
        }
    }
}
