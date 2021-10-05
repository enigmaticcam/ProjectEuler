using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem276 : ProblemBase {
        private PrimeSieve _primes;

        public override string ProblemName {
            get { return "276: Primitive Triangles"; }
        }

        private ulong _total;
        public override string GetAnswer() {
            int maxP = 100;
            //int maxP = 10000000;
            BuildPrimeFactors((ulong)maxP);
            //return BruteForce(100).ToString();
            return "";
        }

        private int _count;
        private void IncludeExclude(bool add, int startPrime, IEnumerable<int> primes, int prod, int start, int end) {
            for (int index = startPrime; index < primes.Count(); index++) {
                var prime = primes.ElementAt(index);
                var nextProd = prod * prime;
                if (nextProd <= end) {
                    if (add) {
                        _count += (end - start + 1) / nextProd;
                    } else {
                        _count -= (end - start + 1) / nextProd;
                    }
                    IncludeExclude(!add, index + 1, primes, nextProd, start, end);
                }
            }
        }

        private Dictionary<int, List<int>> _factors = new Dictionary<int, List<int>>();
        private void BuildPrimeFactors(ulong maxP) {
            _primes = new PrimeSieve(maxP / 3);
            foreach (var prime in _primes.Enumerate) {
                for (ulong num = prime; num <= maxP / 3; num += prime) {
                    if (!_factors.ContainsKey((int)num)) {
                        _factors.Add((int)num, new List<int>());
                    }
                    _factors[(int)num].Add((int)prime);
                }
            }
        }

        private List<string> _answers = new List<string>();
        private ulong BruteForce(ulong maxP) {
            ulong count = 0;
            for (ulong a = 1; a <= maxP; a++) {
                for (ulong b = a; b <= maxP; b++) {
                    var gcd = GCD.GetGCD(a, b);
                    for (ulong c = b; c <= maxP; c++) {
                        if (a + b + c <= maxP && (a + b) > c && GCD.GetGCD(gcd, c) == 1) {
                            count++;
                            _answers.Add(a + "," + b + "," + c);
                        }
                    }
                }
            }
            return count;
        }
    }
}
