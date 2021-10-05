using Euler_Logic.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem203 : ProblemBase {
        private PrimeSieve _primes;
        private Dictionary<ulong, List<Tuple>> _primeFactors = new Dictionary<ulong, List<Tuple>>();

        /*
            If a number is divisible by some square, then at least one of it's prime factors will have a power
            of 2 or more. So all we need to do is factorize all numbers in the first 51 rows. However, these 
            numbers can get very large, and to factorize them individually will take too long.
            
            A better method would be to factorize all numbers up to 51, and for each row, determine the prime 
            factors for each number as you proceed to the right. This can be done by multiplying fractions.
            For example, if we're looking at row 6, the numbers for each cell can be calculated like so:

            1 = 1
            1 * 5 / 1 = 5
            5 * 4 / 2 = 10
            10 * 3 / 3 = 10
            10 * 2 / 4 = 5
            5 * 1 / 5 = 1

            So we start with the prime factors of 5. Then we add the prime factors of 4 and subtract the prime
            factors of 2. That gives us the prime factors of 10. Then we add the prime factors of 3 and subtract
            the prime factors of 3. That gives us 10 again. Continue to do this until we get back to 5.

            As we multiply these fractions, we check at each number of any primes have a power more than 1. If not,
            then we found a number we can add. Add the numbers to a hash array so we only keep the unique ones.
         */

        public override string ProblemName {
            get { return "203: Squarefree Binomial Coefficients"; }
        }

        public override string GetAnswer() {
            ulong max = 51;
            _primes = new PrimeSieve(max);
            BuildPrimeFactors(max);
            FindValids(max);
            return Solve().ToString();
        }

        private ulong Solve() {
            ulong sum = 0;
            foreach (var num in _valids) {
                sum += num;
            }
            return sum;
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

        private HashSet<ulong> _valids = new HashSet<ulong>();
        private void FindValids(ulong max) {
            _valids.Add(1);
            for (ulong n = 2; n < max; n++) {
                ulong num = 1;
                var subPrimeCounts = _primeFactors[max].ToDictionary(x => x.Prime, x => (ulong)0);
                for (ulong x = n; x >= 1; x--) {
                    ulong y = n - x + 1;
                    num = num * x / y;
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
                    bool add = true;
                    foreach (var key in subPrimeCounts.Keys) {
                        if (subPrimeCounts[key] > 1) {
                            add = false;
                            break;
                        }
                    }
                    if (add) {
                        _valids.Add(num);
                    }
                }
            }
        }

        private class Tuple {
            public ulong Prime { get; set; }
            public ulong Power { get; set; }
        }
    }
}