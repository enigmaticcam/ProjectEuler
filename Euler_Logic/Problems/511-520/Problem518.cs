using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem518 : ProblemBase {
        private PrimeSieve _primes;

        /*
            If a, b, c form a geometric sequence, then b/a = r, c/b = r, and therefore c/a = r^2. Thus, if 
            (r) is some rational fraction x/y, then c/a = x^2/y^2, which means c/a must form a fraction with
            two perfect squares.

            My solution therefore tries to find unique ratios (where gcd(x,y) = 1). For each ratio, loop
            through all composites of squares of the ratio until both x - 1 and y - 1 are primes. Since both
            (x - 1) and (y - 1) have to be primes, then (x) and (y) have to both be even. So only consider even
            composites of the squares of (x) and (y). Also, since gcd(x, y) = 1, if (x) is even, then only consider
            odd (y) values.

            If a valid x/y ratio is found where (x) and (y) are perfect squares, and (x - 1) and (y - 1) are primes,
            then a = x, c = y, and b = c/(c/a). Check if b is an integer and (b - 1) is a prime. If so, then
            we have found a matching geometric sequence. Continue to do this until x*x is more than 10^8.

            This method does not find any geometric sequence where a = 3, because (a - 1) = 2 and 2 is the only even 
            prime. So I have a separate function that looks for geometric sequences where (a) is equal to only 2.
         */

        public override string ProblemName {
            get { return "518: Prime triples and geometric sequences"; }
        }

        public override string GetAnswer() {
            ulong max = (ulong)Math.Pow(10, 8);
            _primes = new PrimeSieve(max);
            BuildSquares(max);
            return Solve(max).ToString();
        }

        private ulong Solve(ulong max) {
            ulong sum = GetXOver2(max);
            ulong x = 2;
            do {
                var ySkip = (x % 2 == 0 ? (ulong)2 : 1);
                for (ulong y = 1; y < x; y += ySkip) {
                    if (y == 1 || GCD.GetGCD(x, y) == 1) {
                        ulong a = 0;
                        ulong c = 0;
                        a = y * y * 2;
                        c = x * x * 2;
                        while (c <= max) {
                            if (_primes.IsPrime(a - 1) && _primes.IsPrime(c - 1)) {
                                var prod = a * x;
                                if (prod % y == 0) {
                                    var b = prod / y;
                                    if (_primes.IsPrime(b - 1)) {
                                        sum += (a - 1) + (b - 1) + (c - 1);
                                    }
                                }
                            }
                            a += y * y * 2;
                            c += x * x * 2;
                        }
                    }
                }
                x++;
            } while (x * x <= max);
            return sum;
        }

        private ulong GetXOver2(ulong max) {
            ulong sum = 0;
            ulong a = 3;
            foreach (var prime in _primes.Enumerate) {
                if (prime != 2) {
                    ulong c = prime + 1;
                    if (c % a == 0 && _squaresHash.Contains(c / a)) {
                        ulong b = c / (ulong)Math.Sqrt(c / a);
                        if (_primes.IsPrime(b - 1)) {
                            sum += 2 + (b - 1) + prime;
                        }
                    }
                }
            }
            return sum;
        }

        private HashSet<ulong> _squaresHash = new HashSet<ulong>();
        private void BuildSquares(ulong max) {
            ulong root = 2;
            ulong squared = root * root;
            do {
                _squaresHash.Add(squared);
                root++;
                squared = root * root;
            } while (squared <= max);
        }
    }
}