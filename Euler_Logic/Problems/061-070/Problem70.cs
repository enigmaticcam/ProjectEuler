using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem70 : ProblemBase {
        private HashSet<int> _primes = new HashSet<int>();
        private Number[] _numbers;

        public override string ProblemName {
            get { return "70: Totient permutation"; }
        }

        public override string GetAnswer() {
            int max = 10000000;
            SievePrimes(max);
            Instantiate(max);
            BuildTotients(max);
            return FindMax().ToString();
        }

        private void SievePrimes(int max) {
            HashSet<int> numbers = new HashSet<int>();
            for (int num = 2; num <= max; num++) {
                if (!numbers.Contains(num)) {
                    _primes.Add(num);
                    int composite = num;
                    do {
                        numbers.Add(composite);
                        composite += num;
                    } while (composite <= max);
                }
            }
        }

        private void Instantiate(int max) {
            _numbers = new Number[max];
            for (int num = 1; num <= max; num++) {
                _numbers[num - 1] = new Number();
                _numbers[num - 1].PhiCount = 1;
                _numbers[num - 1].Unfactored = num;
            }
        }

        private void BuildTotients(int max) {
            foreach (int prime in _primes) {
                int composite = prime;
                do {
                    _numbers[composite - 1].Unfactored /= prime;
                    _numbers[composite - 1].PhiCount *= (prime - 1);
                    while (_numbers[composite - 1].Unfactored % prime == 0) {
                        _numbers[composite - 1].Unfactored /= prime;
                        _numbers[composite - 1].PhiCount *= prime;
                    }
                    composite += prime;
                } while (composite <= max);
            }
        }

        private decimal FindMax() {
            int bestValue = -1;
            decimal bestScore = decimal.MaxValue;
            for (decimal index = 0; index <= _numbers.GetUpperBound(0); index++) {
                decimal score = (index + 1) / _numbers[(int)index].PhiCount;
                if (index != 0 && score < bestScore && IsPermutation(_numbers[(int)index].PhiCount, (int)index + 1)) {
                    bestScore = score;
                    bestValue = (int)index + 1;
                }
            }
            return bestValue;
        }

        private bool IsPermutation(int num1, int num2) {
            string x = num1.ToString();
            string y = num2.ToString();
            for (int index = 0; index < x.Length; index++) {
                if (x.Replace(x.Substring(index, 1), "").Length != y.Replace(x.Substring(index, 1), "").Length) {
                    return false;
                }
            }
            return true;
        }

        private class Number {
            public int Unfactored { get; set; }
            public int PhiCount { get; set; }
        }
    }
}
