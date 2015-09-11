using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem10 : IProblem {
        public string ProblemName {
            get { return "10: Summation of primes"; }
        }

        public string GetAnswer() {
            return SumOfPrimes(2000000).ToString();
        }

        private decimal SumOfPrimes(decimal max) {
            decimal sum = 2;
            for (decimal i = 3; i < max; i += 2) {
                if (IsPrime(i)) {
                    sum += i;
                }
            }
            return sum;
        }

        private bool IsPrime(decimal num) {
            if (num == 2) {
                return true;
            } else if (num % 2 == 0) {
                return false;
            } else {
                for (ulong factor = 3; factor <= Math.Sqrt((double)num); factor += 2) {
                    if (num % factor == 0) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
