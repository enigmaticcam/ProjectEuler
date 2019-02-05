using Euler_Logic.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Euler_Logic.Problems {
    public class Problem146 : ProblemBase {
        private PrimeSieveWithPrimeListULong _primes = new PrimeSieveWithPrimeListULong();
        private List<ulong> _nums = new List<ulong>();
        private int _lastIndex = 0;

        /*
            The first thing that can be observed is that (n) must be some multiple of 10. This is because all the digits 1-9
            squared will yield either a 5 or an even number for the last digit when you add 1, 3, 7, 9, 13, or 27. Thus the 
            only digit squared that works is 0. It can also be observed that any (n) cannot be divisible by 3, 7, 9, 13, 
            and 27. Also, per "twin primes" rule, any number that sits between two primes (p1, p2) where p2 - p1 = 1 must be 
            divisible by 6.

            Now let's take a number (n), say 30, and a prime (p), say 7. n^2 = 900, and 900 % p = 4. That means if you add 3
            (p - 4) to 900, it will be divisible by 7, so we can remove 30. In fact, we can do this for all primes up to 
            150,000,000.

            Thus after removing all invalid numbers, we are left with barely 75 that actually need to be tested. Use a 
            standard primality test for these. Takes about 2 minutes, so not the most optimal but good enough for me.
         */

        public override string ProblemName {
            get { return "146: Investigating a Prime Pattern"; }
        }

        public override string GetAnswer() {
            ulong max = 150000000;
            BuildNums(max);
            ReduceNums(max);
            return Solve(max).ToString();
        }

        private void BuildNums(ulong max) {
            for (ulong num = 10; num < max; num += 10) {
                var squared = num * num;
                if (squared % 3 != 0 && squared % 7 != 0 && squared % 13 != 0 && (squared + 2) % 6 == 0) {
                    _nums.Add(num);
                }
            }
        }

        private void ReduceNums(ulong max) {
            var diffs = new HashSet<ulong>() { 1, 3, 7, 9, 13, 27 };
            _lastIndex = _nums.Count - 1;
            var primes = new PrimeSieveWithPrimeListULong();
            primes.SievePrimes(max);
            foreach (var prime in primes.Enumerate) {
                if (prime != 2 && prime != 3 && prime != 5) {
                    for (int index = 0; index < _lastIndex; index++) {
                        var num = _nums[index];
                        if (prime < num) {
                            var diff = prime - (num * num % prime);
                            if (diffs.Contains(diff)) {
                                _nums[index] = _nums[_lastIndex];
                                _lastIndex--;
                            }
                        }
                    }
                }
            }
        }

        private ulong Solve(ulong max) {
            ulong sum = 0;
            for (int index = 0; index <= _lastIndex; index++) {
                if (IsGood(_nums[index])) {
                    sum += _nums[index];
                }
            }
            return sum;
        }

        private bool IsGood(ulong num) {
            num *= num;
            return !PrimalityULong.IsPrime(num + 5)
                && !PrimalityULong.IsPrime(num + 11)
                && !PrimalityULong.IsPrime(num + 15)
                && !PrimalityULong.IsPrime(num + 17)
                && !PrimalityULong.IsPrime(num + 19)
                && !PrimalityULong.IsPrime(num + 21)
                && !PrimalityULong.IsPrime(num + 23)
                && !PrimalityULong.IsPrime(num + 25)
                && PrimalityULong.IsPrime(num + 1)
                && PrimalityULong.IsPrime(num + 3)
                && PrimalityULong.IsPrime(num + 7)
                && PrimalityULong.IsPrime(num + 9)
                && PrimalityULong.IsPrime(num + 13)
                && PrimalityULong.IsPrime(num + 27);
        }
    }
}