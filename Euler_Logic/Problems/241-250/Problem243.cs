using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem243 : ProblemBase {
        private bool[] _notPrimes;
        private List<decimal> _primes = new List<decimal>();
              
        public override string ProblemName {
            get { return "243: Resilience"; }
        }

        /*
            https://en.wikipedia.org/wiki/Euler%27s_totient_function
            Using Euler's Totient: the number of numbers relatively prime to x are the distinct prime factors of x(p1, p2, p3...pn) 
            where: x(p1/(p1-1))(p2/(p2-1))...(pn/(pn-1)). First thing I do then is a prime sieve of numbers up to 10^7, hoping that's enough primes.

            Starting at 2 and iterating up +1 and looking for when a new lowest ratio was found, this is what I got:

            2,4,6,12,18,24,30,60,90,120,150,180,210,420,630,840,4050,1260,1470,1680,1890,2100,4620

            I found that the differences between one and another were repeated a number of times, and then drastically increased. So my algorith starts at 2,
            and instead of moving up +1 each time, when it finds a new best value, it saves the difference between that num and the last best value,
            and then iterates by that degree each time. We keep increasing the difference we iterate by as new best values are found until we find the answer.

            Turns out there are only 93 best values to be found until we get the answer.
         */

        public override string GetAnswer() {
            SievePrimes(10000000);
            return Solve().ToString();
        }

        private decimal Solve() {
            decimal diff = 0;
            decimal last = 0;
            decimal best = decimal.MaxValue;
            decimal num = 2;
            decimal test = 0;
            decimal end = (decimal)15499 / (decimal)94744;
            do {
                test = CalcTotient(num);
                if (test < best) {
                    best = test;
                    diff = num - last;
                    last = num;
                }
                num += diff;
            } while (test >= end);
            return num -= diff;
        }

        private decimal CalcTotient(decimal num) {
            decimal sum = 1;
            decimal temp = num;
            foreach (var prime in _primes) {
                if (temp % prime == 0) {
                    sum *= prime / (prime - 1);
                    temp /= prime;
                    while (temp % prime == 0) {
                        temp /= prime;
                    }
                }
                if (temp == 1) {
                    break;
                }
            }
            return num / sum / (num - 1);
        }

        private void SievePrimes(uint max) {
            _notPrimes = new bool[max + 1];
            for (uint num = 2; num <= max; num++) {
                if (!_notPrimes[num]) {
                    _primes.Add(num);
                    for (uint composite = 2; composite * num <= max; composite++) {
                        _notPrimes[composite * num] = true;
                    }
                }
            }
            _notPrimes = null;
        }
    }
}
