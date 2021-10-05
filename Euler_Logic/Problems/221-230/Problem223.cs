using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem223 : ProblemBase {
        private PrimeSieve _primes;

        /*
            -----------------------------------NEW METHOD-----------------------------------
            According to http://mathworld.wolfram.com/PythagoreanTriple.html, pythagorian triplets can be derived using a matrix transformation.
            Any single triplet can generate three more. It's only a question of what base triplet(s) to start from.
        
            -----------------------------------OLD METHOD-----------------------------------
            a^2 + b^2 = c^2 can be rewritten:
            
            a^2 + b^2 = c^2
            a^2 - 1 = c^2 - b^2
            a^2 - 1 = (c + b)(c - b)

            For all legal values of (a), factorize a^2-1 to find all of its divisors. For each set of divisors that equals a^2-1, find (c) and (b)
            using following rationalization given two divisors (d1, d2):

            b = (d1 - d2) / 2
            d1 = d2 - c

            Not very optimal, takes about 10-15 minutes. I prime sieve all primes up to 1 billion to make factorization more efficient.
         */

        public override string ProblemName {
            get { return "223: Almost right-angled triangles I"; }
        }

        public override string GetAnswer() {
            return Best(25000000).ToString();
            ulong maxP = 25000000;
            InitializePrimeFactors();
            _primes = new PrimeSieve(1000000000);
            _divisors = new ulong[100000];
            return Solve(maxP).ToString();
        }

        private ulong Best(ulong maxP) {
            ulong count = 0;
            var list = new List<Triple>();
            list.Add(new Triple(1, 1, 1));
            list.Add(new Triple(1, 2, 2));
            do {
                var next = new List<Triple>();
                foreach (var tri in list) {
                    var newTri = new Triple(
                        a: 2 * tri.C + tri.B - 2 * tri.A,
                        b: 2 * tri.C + 2 * tri.B - tri.A,
                        c: 3 * tri.C + 2 * tri.B - 2 * tri.A);
                    if (newTri.P <= maxP) {
                        next.Add(newTri);
                    }
                    newTri = new Triple(
                        a: 2 * tri.C + tri.B + 2 * tri.A,
                        b: 2 * tri.C + 2 * tri.B + tri.A,
                        c: 3 * tri.C + 2 * tri.B + 2 * tri.A);
                    if (newTri.P <= maxP) {
                        next.Add(newTri);
                    }
                    if (tri.A != tri.B) {
                        newTri = new Triple(
                        a: 2 * tri.C - 2 * tri.B + tri.A,
                        b: 2 * tri.C - tri.B + 2 * tri.A,
                        c: 3 * tri.C - 2 * tri.B + 2 * tri.A);
                        if (newTri.P <= maxP) {
                            next.Add(newTri);
                        }
                    }
                }
                count += (ulong)list.Count;
                list = next;
            } while (list.Count > 0);
            return count;
        }

        private class Triple {
            public Triple() { }
            public Triple(ulong a, ulong b, ulong c) {
                A = a;
                B = b;
                C = c;
            }

            public ulong A { get; set; }
            public ulong B { get; set; }
            public ulong C { get; set; }
            public ulong P {
                get { return A + B + C; }
            }
        }

        private ulong Solve(ulong maxP) {
            ulong count = (maxP - 1) / 2;
            for (ulong a = 4; a <= maxP / 3; a++) {
                var num = (a * a) - 1;
                if (a % 2 == 0) {
                    if (IsDivisorGood(1, num, a, maxP)) {
                        count++;
                    }
                }
                GetDivisors(num);
                for (int index = 0; index < _divisorMaxIndex; index++) {
                    if (IsDivisorGood(_divisors[index], num, a, maxP)) {
                        count++;
                    }
                }
            }
            return count;
        }

        private bool IsDivisorGood(ulong d1, ulong num, ulong a, ulong maxP) {
            var d2 = num / d1;
            var b = (d2 - d1);
            if (b % 2 == 0) {
                b /= 2;
                var c = d2 - b;
                if (a <= b && b <= c && a + b + c <= maxP) {
                    return true;
                }
            }
            return false;
        }

        private PrimeFactor[] _primeFactors;
        private int _primeFactorIndex;
        private ulong[] _divisors;
        private int _divisorMaxIndex;
        private void GetDivisors(ulong num) {
            _divisorMaxIndex = 0;
            _primeFactorIndex = 0;
            ulong max = (ulong)Math.Sqrt(num);
            foreach (var prime in _primes.Enumerate) {
                if (prime > max) {
                    break;
                }
                if (num % prime == 0) {
                    _primeFactors[_primeFactorIndex].Power = 0;
                    _primeFactors[_primeFactorIndex].Prime = prime;
                    do {
                        _primeFactors[_primeFactorIndex].Power++;
                        num /= prime;
                    } while (num % prime == 0);
                    _primeFactorIndex++;
                    if (num == 1) {
                        break;
                    }
                    if (num <= max && _primes.IsPrime(num)) {
                        _primeFactors[_primeFactorIndex].Power = 1;
                        _primeFactors[_primeFactorIndex].Prime = num;
                        _primeFactorIndex++;
                        break;
                    }
                }
            }
            DivisorRecurisve(max, 0, 1);
        }

        private void DivisorRecurisve(ulong max, int primeFactorIndex, ulong product) {
            var prime = _primeFactors[primeFactorIndex];
            ulong power = 1;
            for (ulong next = 0; next <= prime.Power; next++) {
                if (product * power <= max) {
                    if (primeFactorIndex < _primeFactorIndex - 1) {
                        DivisorRecurisve(max, primeFactorIndex + 1, product * power);
                    } else if (product * power != 1) {
                        _divisors[_divisorMaxIndex] = product * power;
                        _divisorMaxIndex++;
                    }
                }
                power *= prime.Prime;
            }
        }

        private void InitializePrimeFactors() {
            _primeFactors = new PrimeFactor[100];
            for (int index = 0; index < _primeFactors.Length; index++) {
                _primeFactors[index] = new PrimeFactor();
            }
        }

        private class PrimeFactor {
            public ulong Prime { get; set; }
            public ulong Power { get; set; }
        }
    }
}
