using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem187 : IProblem {
        private bool[] _notPrimes;
        private List<ulong> _primes = new List<ulong>();

        public string ProblemName {
            get { return "187: Semiprimes"; }
        }

        public string GetAnswer() {
            BuildPrimes(100000000);
            return Solve(100000000).ToString();
        }

        private void BuildPrimes(ulong max) {
            max = (max / 2) + 1;
            _notPrimes = new bool[max + 1];
            for (ulong num = 2; num < max; num++) {
                if (!_notPrimes[num]) {
                    _primes.Add(num);
                    for (ulong composite = 2; composite * num < max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
        }

        private ulong Solve(ulong max) {
            ulong count = 0;
            for (int x = 0; x < _primes.Count; x++) {
                for (int y = x; y < _primes.Count; y++) {
                    if (_primes[x] * _primes[y] > max) {
                        break;
                    } else {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
