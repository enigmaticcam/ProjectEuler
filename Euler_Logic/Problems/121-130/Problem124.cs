using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem124 : IProblem {
        private bool[] _notPrimes;
        private List<int> _primes = new List<int>();
        private List<Rad> _rads = new List<Rad>();

        public string ProblemName {
            get { return "124: Ordered radicals"; }
        }

        public string GetAnswer() {
            int max = 100000;
            SievePrimes(max);
            LookForRads(max);
            return GetE(10000).ToString();
        }

        private void SievePrimes(int max) {
            _notPrimes = new bool[max + 1];
            for (int num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    _primes.Add(num);
                    for (int composite = 2; composite * num <= max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
        }

        private void LookForRads(int max) {
            _rads.Add(new Rad(1, 1));
            for (int num = 2; num <= max; num++) {
                _rads.Add(GetRadForNum(num));
            }
        }

        private Rad GetRadForNum(int num) {
            int remainder = num;
            int primeIndex = 0;
            int product = 1;
            int lastPrime = 0;
            while (_notPrimes[remainder]) {
                while (remainder % _primes[primeIndex] != 0) {
                    primeIndex++;
                }
                remainder /= _primes[primeIndex];
                if (lastPrime != _primes[primeIndex]) {
                    lastPrime = _primes[primeIndex];
                    product *= _primes[primeIndex];
                }
            }
            if (remainder == num) {
                return new Rad(num, num);
            } else {
                if (lastPrime != remainder) {
                    product *= remainder;
                }
                return new Rad(num, product);
            }
        }

        private int GetE(int k) {
            _rads = _rads.OrderBy(x => x.rad).ThenBy(x => x.n).ToList();
            return _rads[k - 1].n;
        }

        private class Rad {
            public int n { get; set; }
            public int rad { get; set; }

            public Rad(int n, int rad) {
                this.n = n;
                this.rad = rad;
            }
        }
    }
}
