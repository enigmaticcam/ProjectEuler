using System;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Helpers {
    public class PrimeCount {
        private HashSet<ulong> _primesCache = new HashSet<ulong>();
        private List<ulong> _primes;
        private List<ulong> _count;

        public IEnumerable<ulong> Enumerate {
            get { return _primes; }
        }

        public ulong Count(ulong n) {
            if (_primes != null && n <= _primes.Max()) {
                return BinarySearch(n);
            }

            ulong v = (ulong)Math.Sqrt(n);
            ulong[] higher = new ulong[v + 2];
            ulong[] lower = new ulong[v + 2];
            bool[] used = new bool[v + 2];
            ulong result = n - 1;

            for (ulong p = 2; p <= v; p++) {
                lower[p] = p - 1;
                higher[p] = n / p - 1;
            }
            for (ulong p = 2; p <= v; p++) {
                if (lower[p] == lower[p - 1]) {
                    continue;
                }
                _primesCache.Add(p);
                var temp = lower[p - 1];
                result -= higher[p] - temp;

                ulong pSquare = p * p;
                ulong end = Math.Min(v, n / pSquare);
                ulong j = 1 + (p & 1);
                for (ulong i = p + j; i <= end; i += j) {
                    if (used[i]) {
                        continue;
                    }
                    var d = i * p;
                    if (d <= v) {
                        higher[i] -= higher[d] - temp;
                    } else {
                        higher[i] -= lower[n / d] - temp;
                    }
                }
                for (ulong i = v; i >= pSquare; i--) {
                    lower[i] -= lower[i / p] - temp;
                }
                for (ulong i = pSquare; i <= end; i += p * j) {
                    used[i] = true;
                }
            }
            if (_primes == null) {
                _primes = new List<ulong>(_primesCache);
            }
            return result;
        }

        private ulong BinarySearch(ulong n) {
            if (_count == null) {
                _count = new List<ulong>();
                ulong num = 0;
                ulong count = 0;
                foreach (var prime in _primes) {
                    for (; num < prime; num++) {
                        _count.Add(count);
                    }
                    count++;
                    num++;
                    _count.Add(count);
                }
            }
            return _count[(int)n];
        }
    }
}