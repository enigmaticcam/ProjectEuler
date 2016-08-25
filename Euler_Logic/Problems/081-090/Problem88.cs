using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem88 : IProblem {
        private int[] _factors;
        private bool[] _notPrimes;
        private int[] _sums;

        public string ProblemName {
            get { return "88: Product-sum numbers"; }
        }

        public string GetAnswer() {
            int max = 12000;
            _factors = new int[max * 2];
            _sums = new int[max * 2 + 1];
            Initialize();
            SievePrimes(max * 2);
            Solve(max);
            return GetSum(max).ToString();
        }

        private void Initialize() {
            for (int index = 0; index <= _sums.GetUpperBound(0); index++) {
                _sums[index] = int.MaxValue;
            }
        }

        private int GetSum(int max) {
            HashSet<int> sums = new HashSet<int>();
            int sum = 0;
            for (int index = 0; index <= max; index++) {
                if (_sums[index] != int.MaxValue && !sums.Contains(_sums[index])) {
                    sum += _sums[index];
                    sums.Add(_sums[index]);
                }
            }
            return sum;
        }

        private void Solve(int max) {
            int num = 4;
            do {
                BuildPrimeFactors(num, num, 0);
                num++;
            } while (num <= max * 2);
        }

        private void BuildPrimeFactors(int maxNum, int num, int factorCount) {
            int max = num;
            for (int prime = 2; prime < max; prime++) {
                if (num % prime == 0) {
                    max = num / prime;
                    _factors[factorCount] = prime;
                    if (num / prime != 1) {
                        _factors[factorCount + 1] = num / prime;
                        CheckProductSums(maxNum, factorCount + 1);
                        if (_notPrimes[num / prime]) {
                            BuildPrimeFactors(maxNum, num / prime, factorCount + 1);
                        }
                    } else {
                        CheckProductSums(maxNum, factorCount);
                    }
                }
            }
        }

        private void CheckProductSums(int maxNum, int factorCount) {
            int sum = 0;
            for (int index = 0; index <= factorCount; index++) {
                sum += _factors[index];
            }
            int digitCount = 1 + factorCount + (maxNum - sum);
            if (_sums[digitCount] > maxNum) {
                _sums[digitCount] = maxNum;
            }
        }

        private void SievePrimes(int max) {
            _notPrimes = new bool[max + 1];
            for (int num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    for (int composite = 2; composite * num <= max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
        }
    }
}
