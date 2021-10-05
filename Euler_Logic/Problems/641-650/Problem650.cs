using Euler_Logic.Helpers;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem650 : ProblemBase {
        private PrimeSieve _primes;
        private Dictionary<ulong, List<Tuple>> _primeFactors = new Dictionary<ulong, List<Tuple>>();
        private Dictionary<ulong, PowerSum> _powerSums = new Dictionary<ulong, PowerSum>();
        private ulong _mod = 1000000007;

        /*
            Any individual cell (m,k) can be calculated: m! / (k! * (m - k)!). The product of an entire 
            row (n) would then be n!^n / ((2)1! * (2)2! * (2)3! ... * (2)(n - 1)!). So I calculate the
            prime factors of all these factorials. Then I simply start with the prime factors of n!^n and
            subtract the double of all prime factors of all subsequent factorials less than n. 

            The above will calculate B(n). To calulate D(n), we simply take the sums of all the powers of
            the prime factors of B(n) and multiply them. For example, if B(n) = 96, then the prime factors
            of 96 would be 2^5 * 3^1. So then, D(96) = (2^0 + 2^1 + 2^2 + 2^3 + 2^4 + 2^5) * (3^0 + 3^1) 
            = 252. I use a hash dictionary to assist with this. Takes 3gb of ram but it solves in less
            than 60 seconds!
         */

        public override string ProblemName {
            get { return "650: Divisors of Binomial Product"; }
        }

        public override string GetAnswer() {
            ulong max = 20000;
            _primes = new PrimeSieve(max);
            BuildPrimeFactors(max);
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

        private ulong Solve(ulong max) {
            ulong sum = 1;
            var subtract = new Dictionary<ulong, ulong>();
            for (ulong n = 2; n <= max; n++) {
                var next = FactorialPrimeFactors(n);
                ulong subSum = 1;
                foreach (var prime in _primes.Enumerate) {
                    if (prime > n) {
                        break;
                    }
                    if (!subtract.ContainsKey(prime)) {
                        subtract.Add(prime, next[prime] / (n - 1) * 2);
                        subSum = (subSum * GetPowerSum(prime, next[prime])) % _mod;
                    } else {
                        subSum = (subSum * GetPowerSum(prime, next[prime] - subtract[prime])) % _mod;
                        subtract[prime] += next[prime] / (n - 1) * 2;
                    }
                }
                sum = (subSum + sum) % _mod;
            }
            return sum;
        }

        private ulong GetPowerSum(ulong prime, ulong power) {
            if (!_powerSums.ContainsKey(prime)) {
                _powerSums.Add(prime, new PowerSum() { HighestPowerKey = 0, HighestPowerValue = 1, Sums = new Dictionary<ulong, ulong>() });
                _powerSums[prime].Sums.Add(0, 1);
            }
            if (_powerSums[prime].Sums.ContainsKey(power)) {
                return _powerSums[prime].Sums[power];
            }
            var powerSum = _powerSums[prime];
            var sum = powerSum.Sums[powerSum.HighestPowerKey];
            for (var next = powerSum.HighestPowerKey + 1; next <= power; next++) {
                powerSum.HighestPowerValue = (powerSum.HighestPowerValue * prime) % _mod;
                powerSum.HighestPowerKey = next;
                sum = (sum + powerSum.HighestPowerValue) % _mod;
                powerSum.Sums.Add(powerSum.HighestPowerKey, sum);
            }
            return sum;
        }

        private Dictionary<ulong, ulong> FactorialPrimeFactors(ulong n) {
            var factors = new Dictionary<ulong, ulong>();
            foreach (var prime in _primes.Enumerate) {
                if (prime > n) {
                    break;
                }
                ulong sum = 0;
                ulong power = prime;
                ulong result = n / power;
                do {
                    sum += result;
                    power *= prime;
                    result = n / power;
                } while (result > 0);
                factors.Add(prime, sum * (n - 1));
            }
            return factors;
        }

        private class Tuple {
            public ulong Prime { get; set; }
            public ulong Power { get; set; }
        }

        private class PowerSum {
            public Dictionary<ulong, ulong> Sums { get; set; }
            public ulong HighestPowerKey { get; set; }
            public ulong HighestPowerValue { get; set; }
        }
    }
}
