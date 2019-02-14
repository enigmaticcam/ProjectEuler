using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem601 : ProblemBase {
        private PrimeSieve _primes = new PrimeSieve(31);
        private Dictionary<ulong, int> _primeCounts = new Dictionary<ulong, int>();

        /*
            Any number that is an even number immediately stops and yields a streak of 1 because the next
            number is not an even number and therefore cannot be divisible by 2. If n is 100, that means
            49 numbers stop at 2 and 49 numbers can keep going. We can divide the remaining 49 that keep
            going by 3, leaving 16 that keep going and (49 - 16) = 33 that stop. We continue to do this for
            4, 5, 6, and so on.
            
            However, for numbers that are not prime (4, 6, 8, etc), we don't divide by the whole number. 
            For example, we don't divide by 4 because we've already divded by 2. Instead, we only need to
            divide by 2 again. And we don't divide by 6 at all, because we've already divided by 2 and 3 by 
            the time we've reached 6. So for numbers that are not prime, we need to factorize them and determine
            which prime factors to remove. By 8, we've already divded by 2 twice, so only need to divide by
            2. By 9, we've already divided by 3, so only need to divide by 3 again, and so on.
         */

        public override string ProblemName {
            get { return "601: Divisibility streaks"; }
        }

        public override string GetAnswer() {
            return Solve().ToString();
        }

        private ulong Solve() {
            ulong sum = 0;
            for (ulong i = 1; i <= 31; i++) {
                sum += Find(i, (ulong)Math.Pow(4, i));
            }
            return sum;
        }

        private ulong Find(ulong s, ulong n) {
            ClearPrimeCounts();
            ulong total = n - 2;
            ulong stop = 0;
            for (ulong num = 2; num <= s + 1; num++) {
                ulong divideBy = num;
                if (!_primes.IsPrime(divideBy)) {
                    divideBy = CalculateDivideBy(divideBy);
                } else {
                    _primeCounts[divideBy]++;
                }
                ulong remaining = total / divideBy;
                stop = total - remaining;
                total = remaining;
            }
            return stop;
        }

        private void ClearPrimeCounts() {
            foreach (var prime in _primes.Enumerate) {
                _primeCounts[prime] = 0;
            }
        }

        private ulong CalculateDivideBy(ulong divideBy) {
            ulong num = divideBy;
            foreach (var prime in _primes.Enumerate) {
                if (num % prime == 0) {
                    int count = 0;
                    do {
                        divideBy /= prime;
                        count++;
                        if (_primeCounts[prime] < count) {
                            _primeCounts[prime]++;
                        } else {
                            num /= prime;
                        }
                    } while (divideBy % prime == 0);
                    if (divideBy == 1) {
                        break;
                    }
                }
            }
            return num;
        }
    }
}