using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem370 : ProblemBase {

        public override string ProblemName {
            get { return "370: Geometric triangles"; }
        }

        public override string GetAnswer() {
            //ulong max = (ulong)Math.Pow(10, 12);
            ulong max = (ulong)Math.Pow(10, 12) * 25;
            _primes = new PrimeSieve((ulong)Math.Sqrt(max / 3));
            return Solve1(max).ToString();
            return "";
        }

        private ulong Solve1(ulong maxP) {
            ulong count = maxP / 3;
            double phi = (1 + Math.Sqrt(5)) / 2;
            ulong maxY = (ulong)Math.Sqrt(maxP / 3);
            var primes = new PrimeSieve(maxY);
            for (ulong y = 2; y <= maxY; y++) {
                ulong maxX = (ulong)((double)y * phi);
                FindFirst(y, y + 1);
                var p = _first.A + _first.B + _first.C;
                FindFirst(y, y + 2);
                var diff = _first.A + _first.B + _first.C - p - 2;
                if (primes.IsPrime(y)) {
                    for (ulong x = y + 1; x <= maxX; x++) {
                        count += maxP / p;
                        diff += 2;
                        p += diff;
                        if (p > maxP) {
                            break;
                        }
                    }
                } else {
                    FindPrimes(y);
                    var skip = new bool[maxX - y];
                    foreach (var prime in _primesList) {
                        var num = prime;
                        while (num + y <= maxX) {
                            skip[num - 1] = true;
                            num += prime;
                        }
                    }
                    for (ulong num = 1; num <= maxX - y; num++) {
                        if (!skip[num - 1]) {
                            count += maxP / p;
                        }
                        diff += 2;
                        p += diff;
                        if (p > maxP) {
                            break;
                        }
                    }
                }

            }
            return count;
        }

        private List<ulong> _primesList = new List<ulong>();
        private PrimeSieve _primes;
        private void FindPrimes(ulong num) {
            _primesList.Clear();
            foreach (var prime in _primes.Enumerate) {
                if (num % prime == 0) {
                    _primesList.Add(prime);
                    do {
                        num /= prime;
                    } while (num % prime == 0);
                    if (num == 1) {
                        break;
                    } else if (_primes.IsPrime(num)) {
                        _primesList.Add(num);
                        break;
                    }
                }
            }
        }

        private Triangle _first = new Triangle();
        private void FindFirst(ulong x, ulong y) {
            _first.A = x * x;
            _first.C = y * y;
            _first.B = y * x;
        }

        private class Triangle {
            public ulong A { get; set; }
            public ulong B { get; set; }
            public ulong C { get; set; }
        }
    }
}