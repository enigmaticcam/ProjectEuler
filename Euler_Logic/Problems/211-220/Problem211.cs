using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem211 : ProblemBase {
        private PrimeSieve _primes;
        private Dictionary<ulong, Dictionary<int, ulong>> _powerSums = new Dictionary<ulong, Dictionary<int, ulong>>();

        /*
            The sum of the divisors of a number (n) can be quickly calculated by multiplying the sum of the powers
            of its prime factors. For example, if (n) = 12, then the prime factors are 2^2 * 3^1. The sum of the
            powers would be: (1+2+4) * (1 + 3) = 28. However, since we are looking for the squares of the divisors,
            that can be calculated (1^2+2^2+4^4) * (1^2+3^2) = 210.

            So I simply find the prime factors of all (n) up to 64,000,000 and calculate the sum of the squares of
            its divisor using the above method, and sum each (n) where the result is a perfect square.
         */

        public override string ProblemName {
            get { return "211: Divisor Square Sum"; }
        }

        public override string GetAnswer() {
            ulong max = 64000000;
            _primes = new PrimeSieve(max);
            return Solve(max).ToString();
        }

        private ulong Solve(ulong max) {
            ulong sum = 1;
            for (ulong num = 2; num < max; num++) {
                var divisorSum = CalcDivisorSum(num);
                var root = (ulong)Math.Sqrt(divisorSum);
                if (root * root == divisorSum) {
                    sum += num;
                }
            }
            return sum;
        }

        private ulong CalcDivisorSum(ulong num) {
            ulong sum = 1;
            if (_primes.IsPrime(num)) {
                return num * num + 1;
            }
            foreach (var prime in _primes.Enumerate) {
                if (num % prime == 0) {
                    int count = 0;
                    do {
                        num /= prime;
                        count++;
                    } while (num % prime == 0);
                    sum *= GetPowerSum(prime, count);
                    if (_primes.IsPrime(num)) {
                        sum *= (1 + (num * num));
                        break;
                    } else if (num == 1) {
                        break;
                    }
                }
            }
            return sum;
        }

        private ulong GetPowerSum(ulong prime, int power) {
            if (!_powerSums.ContainsKey(prime)) {
                _powerSums.Add(prime, new Dictionary<int, ulong>());
            }
            if (!_powerSums[prime].ContainsKey(power)) {
                ulong num = 1;
                ulong sum = 1;
                for (int count = 1; count <= power; count++) {
                    num *= prime;
                    sum += num * num;
                }
                _powerSums[prime].Add(power, sum);
            }
            return _powerSums[prime][power];
        }
    }
}
