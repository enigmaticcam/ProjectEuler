using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem72 : IProblem {
        private HashSet<ulong> _primes = new HashSet<ulong>();
        private Number[] _numbers;

        public string ProblemName {
            get { return "72: Counting fractions"; }
        }

        public string GetAnswer() {
            ulong max = 1000000;
            SievePrimes(max);
            Instantiate(max);
            BuildTotients(max);
            return FindMax().ToString();
        }

        private void SievePrimes(ulong max) {
            HashSet<ulong> numbers = new HashSet<ulong>();
            for (ulong num = 2; num <= max; num++) {
                if (!numbers.Contains(num)) {
                    _primes.Add(num);
                    ulong composite = num;
                    do {
                        numbers.Add(composite);
                        composite += num;
                    } while (composite <= max);
                }
            }
        }

        private void Instantiate(ulong max) {
            _numbers = new Number[max];
            for (ulong num = 1; num <= max; num++) {
                _numbers[num - 1] = new Number();
                _numbers[num - 1].PhiCount = 1;
                _numbers[num - 1].Unfactored = num;
            }
        }

        private void BuildTotients(ulong max) {
            foreach (ulong prime in _primes) {
                ulong composite = prime;
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

        private ulong FindMax() {
            ulong total = 0;
            foreach (Number number in _numbers) {
                total += number.PhiCount;
            }
            return total - 1;
        }

        private class Number {
            public ulong Unfactored { get; set; }
            public ulong PhiCount { get; set; }
        }
    }
}
