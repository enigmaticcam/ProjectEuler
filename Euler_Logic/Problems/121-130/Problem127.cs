using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem127 : ProblemBase {
        private Dictionary<ulong, HashSet<ulong>> _primeFactors = new Dictionary<ulong, HashSet<ulong>>();
        private List<ulong> _primes = new List<ulong>();
        private ulong _sum = 0;

        public override string ProblemName {
            get { return "127: abc-hits"; }
        }

        /*
            Brute-force solution that takes 10 minutes. Build a list of prime factors for all numbers 2 to 120000. Then loop through C, building a list of distinct
            primes and a prime product. For each C, loop through A where A < C - A. As you find A and then B (C - A), continue adding to the distinct prime list
            and prime product until (1) the prime product exceeds C, (2) either A, B, or C share a prime factor, or (3) an abc-hit is found.
         */

        public override string GetAnswer() {
            ulong max = 120000;
            GeneratePrimeFactors(max);
            FindC(max);
            return _sum.ToString();
        }

        private void GeneratePrimeFactors(ulong max) {
            for (ulong num = 2; num < max; num++) {
                if (!_primeFactors.ContainsKey(num)) {
                    _primeFactors.Add(num, new HashSet<ulong>());
                    _primeFactors[num].Add(num);
                    ulong composite = num + num;
                    while (composite <= max) {
                        if (!_primeFactors.ContainsKey(composite)) {
                            _primeFactors.Add(composite, new HashSet<ulong>());
                        }
                        _primeFactors[composite].Add(num);
                        composite += num;
                    }
                }
            }
        }

        private void FindC(ulong max) {
            for (ulong c = 3; c < max; c++) {
                _primes.Clear();
                ulong primeProduct = 1;
                foreach (ulong factor in _primeFactors[c]) {
                    _primes.Add(factor);
                    primeProduct *= factor;
                }
                FindA(c, primeProduct);
            }
        }

        private void FindA(ulong c, ulong primeProduct) {
            int reverseIndex = 0;
            for (ulong a = 1; a < c - a; a++) {
                bool tryA = true;
                if (a != 1) {
                    var factors = _primeFactors[a];
                    reverseIndex = -1;
                    for (int index = 0; index < factors.Count; index++) {
                        ulong factor = factors.ElementAt(index);
                        if (!_primes.Contains(factor) && primeProduct * factor < c) {
                            _primes.Add(factor);
                            primeProduct *= factor;
                        } else {
                            tryA = false;
                            break;
                        }
                        reverseIndex = index;
                    }
                }
                if (tryA) {
                    FindB(c, a, primeProduct);
                }
                if (a != 1) {
                    var factors = _primeFactors[a];
                    for (int backIndex = reverseIndex; backIndex >= 0; backIndex--) {
                        ulong backFactor = factors.ElementAt(backIndex);
                        _primes.Remove(backFactor);
                        primeProduct /= backFactor;
                    }
                }

            }
        }

        private void FindB(ulong c, ulong a, ulong primeProduct) {
            ulong b = c - a;
            bool tryB = true;
            foreach (var factor in _primeFactors[b]) {
                if (_primes.Contains(factor) || primeProduct * factor >= c) {
                    tryB = false;
                    break;
                }
                primeProduct *= factor;
            }
            if (tryB) {
                _sum += c;
            }
        }
    }
}