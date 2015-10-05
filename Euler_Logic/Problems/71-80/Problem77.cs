using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem77 : IProblem {
        private List<decimal> _primes = new List<decimal>();
        private Dictionary<decimal, Dictionary<decimal, int>> _primeCounts = new Dictionary<decimal, Dictionary<decimal, int>>();

        public string ProblemName {
            get { return "77: Prime summations"; }
        }

        public string GetAnswer() {
            return PrimeSummations(7).ToString();
        }

        private int PrimeSummations(decimal count) {
            decimal num = 1;
            do {
                for (int primeIndex = 0; primeIndex < _primes.Count; primeIndex++) {
                    BuildSums(num, primeIndex, num);
                }
                if (IsPrime(num)) {
                    _primes.Add(num);
                    SetCount(num, num, 1);
                    if (num != 2) {
                        BuildSums(0, _primes.Count - 1, num);
                    }
                }
                num++;
            } while (true);
        }

        private void BuildSums(decimal weight, int primeIndex, decimal num) {
            if (primeIndex > 0) {
                for (decimal weightIndex = weight; weightIndex <= num; weightIndex++) {
                    for (int prime = 0; prime <= primeIndex; prime++) {
                        decimal tempWeight = 0;
                        while (tempWeight <= weightIndex) {
                            int count = GetCount(_primes.ElementAt(primeIndex), weightIndex);
                            SetCount(_primes.ElementAt(primeIndex), weightIndex, count + GetCount(_primes.ElementAt(primeIndex - 1), weightIndex - tempWeight));
                            tempWeight += _primes[primeIndex];
                        }
                    }
                }
            }
        }

        private int GetCount(decimal prime, decimal weight) {
            if (!_primeCounts.ContainsKey(prime)) {
                _primeCounts.Add(prime, new Dictionary<decimal, int>());
            }
            if (!_primeCounts[prime].ContainsKey(weight)) {
                _primeCounts[prime].Add(weight, 0);
            }
            return _primeCounts[prime][weight];
        }

        private void SetCount(decimal prime, decimal weight, int value) {
            if (!_primeCounts.ContainsKey(prime)) {
                _primeCounts.Add(prime, new Dictionary<decimal, int>());
            }
            if (!_primeCounts[prime].ContainsKey(weight)) {
                _primeCounts[prime].Add(weight, value);
            } else {
                _primeCounts[prime][weight] = value;
            }
        }

        private bool IsPrime(decimal num) {
            if (num == 1) {
                return false;
            } else if (num == 2) {
                return true;
            } else if (num % 2 == 0) {
                return false;
            } else {
                for (ulong factor = 3; factor <= Math.Sqrt((double)num); factor += 2) {
                    if (num % factor == 0) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
