using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem365 : ProblemBase {
        private List<ulong> _primes = new List<ulong>();

        public override string ProblemName {
            get { return "365: A huge binomial coefficient"; }
        }

        public override string GetAnswer() {
            GetPrimes();
            Solve();
            return "";
        }

        private void GetPrimes() {
            var primes = new PrimeSieve(5000);
            foreach (var prime in primes.Enumerate) {
                if (prime > 1000 && prime < 5000) {
                    _primes.Add(prime);
                }
            }
        }

        private void Solve() {
            var tenTo18 = (ulong)Math.Pow(10, 18);
            var tenTo9 = (ulong)Math.Pow(10, 9);
            var diff = tenTo18 - tenTo9;
            for (int index1 = 0; index1 < _primes.Count; index1++) {
                var prime1 = _primes[index1];
                var prime1Count = CalcRemainingFactor(prime1);
                for (int index2 = index1 + 1; index2 < _primes.Count; index2++) {
                    var prime2 = _primes[index2];
                    var prime2Count = CalcRemainingFactor(prime2);
                    for (int index3 = index2 + 1; index3 < _primes.Count; index3++) {
                        var prime3 = _primes[index3];
                        var prime3Count = CalcRemainingFactor(prime3);
                        if (prime1Count == 0 || prime2Count == 0 || prime3Count == 0) {
                            bool stop = true;
                        }
                    }
                }
            }
        }

        private ulong _tenTo18 = (ulong)Math.Pow(10, 18);
        private ulong _tenTo9 = (ulong)Math.Pow(10, 9);
        private ulong _diff = (ulong)Math.Pow(10, 18) - (ulong)Math.Pow(10, 9);
        private int CalcRemainingFactor(ulong prime) {
            int count = GetFactor(_tenTo18, prime);
            count -= GetFactor(_tenTo9, prime);
            count -= GetFactor(_diff, prime);
            return count;
        }

        private Dictionary<ulong, Dictionary<ulong, int>> _factors = new Dictionary<ulong, Dictionary<ulong, int>>();
        private int GetFactor(ulong num, ulong prime) {
            if (!_factors.ContainsKey(num)) {
                _factors.Add(num, new Dictionary<ulong, int>());
            }
            if (!_factors[num].ContainsKey(prime)) {
                ulong count = 0;
                var temp = prime;
                var factor = num / temp;
                while (factor > 0) {
                    count += factor;
                    temp *= prime;
                    factor = num / temp;
                }
                _factors[num].Add(prime, (int)count);
            }
            return _factors[num][prime];
        }
    }
}
