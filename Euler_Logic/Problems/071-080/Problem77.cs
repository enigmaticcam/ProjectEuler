using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem77 : ProblemBase {
        private List<decimal> _primes = new List<decimal>();
        private List<Dictionary<decimal, int>> _primeCounts = new List<Dictionary<decimal, int>>();

        public override string ProblemName {
            get { return "77: Prime summations"; }
        }

        public override string GetAnswer() {
            _primeCounts.Add(new Dictionary<decimal, int>());
            return PrimeSummations(5000).ToString();
        }

        private int PrimeSummations(decimal count) {
            decimal num = 1;
            do {
                for (int primeIndex = 0; primeIndex < _primes.Count; primeIndex++) {
                    BuildSums(num, primeIndex, num);
                }
                if (IsPrime(num)) {
                    _primes.Add(num);
                    _primeCounts.Add(new Dictionary<decimal, int>());
                    BuildSums(2, _primes.Count - 1, num);
                }
                if (GetCount(_primes.Count - 1, num) >= count) {
                    return (int)num;
                }
                num++;
            } while (true);
        }

        private void BuildSums(decimal weight, int primeIndex, decimal num) {
            for (decimal weightIndex = weight; weightIndex <= num; weightIndex++) {
                decimal tempWeight = 0;
                while (tempWeight <= weightIndex) {
                    int count = GetCount(primeIndex, weightIndex);
                    if (tempWeight == weightIndex) {
                        SetCount(primeIndex, weightIndex, count + 1);
                    } else {
                        SetCount(primeIndex, weightIndex, count + GetCount(primeIndex - 1, weightIndex - tempWeight));
                    }
                    tempWeight += _primes[primeIndex];
                }
            }
        }

        private int GetCount(int primeIndex, decimal weight) {
            primeIndex += 1;
            if (!_primeCounts[primeIndex].ContainsKey(weight)) {
                _primeCounts[primeIndex].Add(weight, 0);
            }
            return _primeCounts[primeIndex][weight];
        }

        private void SetCount(int primeIndex, decimal weight, int value) {
            primeIndex += 1;
            if (!_primeCounts[primeIndex].ContainsKey(weight)) {
                _primeCounts[primeIndex].Add(weight, value);
            } else {
                _primeCounts[primeIndex][weight] = value;
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
