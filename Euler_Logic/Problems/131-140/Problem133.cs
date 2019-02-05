using Euler_Logic.Helpers;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem133 : ProblemBase {
        private PrimeSieveWithPrimeListULong _primes = new PrimeSieveWithPrimeListULong();
        private Dictionary<ulong, bool> _isGood = new Dictionary<ulong, bool>();

        /*
            We can easily determine when a single prime is divisible by R(n) by starting with x = 1, then
            continue to do x = (x * 10 + 1) % prime until x equals 0. For example, if p = 7, then:

            x = 1
            x = (1 * 10 + 1) = 11 % 7 = 4
            x = (4 * 10 + 1) = 41 % 7 = 6
            x = (6 * 10 + 1) = 61 % 7 = 5
            x = (5 * 10 + 1) = 51 % 7 = 2
            x = (2 * 10 + 1) = 21 % 7 = 0

            I can be observed that when p = 7, then there are 6 iterations.

            Now instead of looking at the prime, let's look at the iteration length (l). If p = 7, then l = 6.
            The question is, after 10 iterations, will x = 0? No, because 10 % 6 = 4. What about after 100 iterations?
            Well then we simply multiply 4 by 10 (40) and return 40 % 6. That gives us 4 again. So because we
            got 4 twice, we know that p = 7 will never give us a number divisible by R(10^n).

            Do this for all prime numbers below 100000. Many primes share the same length (l), so use a hash
            to store all the answers for each discovered (l).
         */

        public override string ProblemName {
            get { return "133: Repunit nonfactors"; }
        }

        public override string GetAnswer() {
            ulong max = 100000;
            _primes.SievePrimes(max);
            return Solve().ToString();
        }

        private ulong Solve() {
            _isGood.Add(1, true);
            ulong sum = 0;
            foreach (var prime in _primes.Enumerate) {
                if (IsGood(prime)) {
                    sum += prime;
                }
            }
            return sum;
        }

        private bool IsGood(ulong prime) {
            ulong length = GetPatternLength(prime);
            if (!_isGood.ContainsKey(length)) {
                var ten = 10 % length;
                HashSet<ulong> found = new HashSet<ulong>();
                while (ten != 0) {
                    ten = (ten * 10) % length;
                    if (found.Contains(ten)) {
                        _isGood.Add(length, true);
                        return true;
                    } else {
                        found.Add(ten);
                    }
                }
                _isGood.Add(length, false);
                return false;
            }
            return _isGood[length];
        }

        private ulong GetPatternLength(ulong prime) {
            ulong num = 11 % prime;
            ulong start = num;
            ulong count = 0;
            do {
                num = (num * 10 + 1) % prime;
                count++;
            } while (num != start);
            return count;
        }
    }
}