using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem650 : ProblemBase {
        private PrimeSieve _primes;
        private Dictionary<ulong, List<Tuple>> _primeFactors = new Dictionary<ulong, List<Tuple>>();

        /*
            Using row 5 as an example, the values can be calculated like so:

            5/1 = 5
            5 * 4/2 = 10
            10 * 3/3 = 10
            10 * 2/4 = 5
            5 * 1/5 = 1

            So I find the prime factors of all numbers up to 20,000, and add/remove them based on these fractions for each row.
            Unfortunately it's very slow and takes a few hours.
         */

        public override string ProblemName {
            get { return "650: Divisors of Binomial Product"; }
        }

        public override string GetAnswer() {
            ulong max = 20000;
            _primes = new PrimeSieve(max);
            BuildPrimeFactors(max);
            BuildPrimeDivisors(max);
            return Solve(max).ToString();
        }

        private void BuildPrimeFactors(ulong max) {
            for (ulong n = 2; n <= max; n++) {
                List<Tuple> tuples = new List<Tuple>();
                var sub = n;
                foreach (var prime in _primes.Enumerate) {
                    if (sub % prime == 0) {
                        var tuple = new Tuple() { Prime = prime };
                        tuples.Add(tuple);
                        do {
                            tuple.Power++;
                            sub /= prime;
                        } while (sub % prime == 0);
                        if (sub == 1) {
                            break;
                        }
                    }
                }
                _primeFactors.Add(n, tuples);
            }
        }

        private void BuildPrimeDivisors(ulong max) {
            foreach (var prime in _primes.Enumerate) {
                _primeDivisors.Add(prime, new Dictionary<ulong, ulong>());
                ulong num = 1;
                ulong sub = 1;
                for (ulong power = 1; power <= max; power++) {
                    sub = (sub * prime) % 1000000007;
                    num = (num + sub) % 1000000007;
                    _primeDivisors[prime].Add(power, num);
                }
            }
        }

        private ulong Solve(ulong max) {
            ulong sum = 1;
            for (ulong n = 2; n <= max; n++) {
                sum = (sum + GetS(n)) % 1000000007;
            }
            return sum;
        }

        private ulong GetS(ulong n) {
            if (n == 2) {
                return 3;
            }
            var totalPrimeCounts = new Dictionary<ulong, ulong>();
            var subPrimeCounts = _primeFactors[n].ToDictionary(x => x.Prime, x => (ulong)0);
            ulong y = 1;
            ulong stop = n / 2 + (n % 2 == 0 ? (ulong)1 : 2);
            for (ulong x = n; x >= stop; x--) {
                if (x != 1) {
                    _primeFactors[x].ForEach(tuple => {
                        if (!subPrimeCounts.ContainsKey(tuple.Prime)) {
                            subPrimeCounts.Add(tuple.Prime, tuple.Power);
                        } else {
                            subPrimeCounts[tuple.Prime] += tuple.Power;
                        }
                    });
                }
                if (y != 1) {
                    _primeFactors[y].ForEach(tuple => subPrimeCounts[tuple.Prime] -= tuple.Power);
                }
                var doubleIt = (ulong)2;
                if (n % 2 == 0 && x == n / 2 + 1) {
                    doubleIt = 1;
                }
                foreach (var key in subPrimeCounts.Keys) {
                    if (subPrimeCounts[key] != 0) {
                        if (!totalPrimeCounts.ContainsKey(key)) {
                            totalPrimeCounts.Add(key, subPrimeCounts[key] * doubleIt);
                        } else {
                            totalPrimeCounts[key] += subPrimeCounts[key] * doubleIt;
                        }
                    }
                }
                y++;
            }
            return CalculateDivisorSum(totalPrimeCounts);
        }

        private Dictionary<ulong, Dictionary<ulong, ulong>> _primeDivisors = new Dictionary<ulong, Dictionary<ulong, ulong>>();
        private ulong CalculateDivisorSum(Dictionary<ulong, ulong> primeCounts) {
            ulong sum = 1;
            foreach (var key in primeCounts.Keys) {
                var value = primeCounts[key];
                if (!_primeDivisors.ContainsKey(key)) {
                    _primeDivisors.Add(key, new Dictionary<ulong, ulong>());
                }
                if (!_primeDivisors[key].ContainsKey(value)) {
                    ulong num = 1;
                    ulong sub = 1;
                    for (ulong power = 1; power <= value; power++) {
                        sub = (sub * key) % 1000000007;
                        num = (num + sub) % 1000000007;
                    }
                    _primeDivisors[key].Add(value, num);
                }
                sum = (sum * _primeDivisors[key][value]) % 1000000007;
            }
            return sum;
        }

        private class Tuple {
            public ulong Prime { get; set; }
            public ulong Power { get; set; }
        }
    }
}