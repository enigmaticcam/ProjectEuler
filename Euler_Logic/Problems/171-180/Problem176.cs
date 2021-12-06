using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem176 : ProblemBase {
        public override string ProblemName {
            get { return "176: Right-angled triangles that share a cathetus"; }
        }

        public override string GetAnswer() {
            Solve1(10);
            return "";
        }

        private PrimeSieve _primes;
        private Dictionary<ulong, ulong> _factors = new Dictionary<ulong, ulong>();
        private void Solve1(ulong countLookingFor) {
            _primes = new PrimeSieve(100000000);
            ulong v = 3;
            do {
                _factors.Clear();
                GetFactors(v);
                v++;
            } while (true);
        }

        private void GetFactors(ulong num) {
            foreach (var prime in _primes.Enumerate) {
                if (num % prime == 0) {
                    ulong count = 0;
                    do {
                        num /= prime;
                        count++;
                    } while (num % prime == 0);
                    _factors.Add(prime, count);
                    if (_primes.IsPrime(num)) {
                        _factors.Add(num, 1);
                        num = 1;
                    }
                    if (num == 1) {
                        break;
                    }
                }
            }
        }

        private int[] _counts;
        private void Count(ulong max) {
            _counts = new int[max + 1];
            ulong m = 2;
            do {
                ulong minN = (m * m <= max ? 1 : (ulong)Math.Sqrt(m * m - max));
                ulong maxN = Math.Min(m - 1, max / 2 / m);
                if (minN > maxN) {
                    break;
                }
                for (ulong n = minN; n <= maxN; n++) {
                    if ((m % 2 == 0 || n % 2 == 0) && GCD.GetGCD(m, n) == 1) {
                        ulong a = (m * m) - (n * n);
                        ulong b = 2 * m * n;
                        ulong subA = a;
                        ulong subB = b;
                        while (subA <= max && subB <= max) {
                            _counts[subA]++;
                            _counts[subB]++;
                            subA += a;
                            subB += b;
                        }
                    }
                }
                m++;
            } while (true);
        }

        private void LookFor(ulong num) {
            ulong m = 2;
            do {
                for (ulong n = 1; n < m; n++) {
                    if (GCD.GetGCD(m, n) == 1) {
                        ulong a = (m * m) - (n * n);
                        ulong b = 2 * m * n;
                        if (a == num || b == num || num % a == 0 || num % b == 0) {
                            bool stop = true;
                        }
                    }
                }
                m++;
            } while (true);
        }

        private class Tuple {
            public ulong Count { get; set; }
            public ulong MaxM { get; set; }
        }
    }
}
