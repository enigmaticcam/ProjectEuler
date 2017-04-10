using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem7 : ProblemBase {
        public override string ProblemName {
            get { return "7: 10001st prime"; }
        }

        public override string GetAnswer() {
            return WhatIsPrimeAt(10001).ToString();
        }

        private decimal WhatIsPrimeAt(decimal count) {
            decimal primeCount = 1;
            decimal num = 3;
            do {
                if (IsPrime(num)) {
                    primeCount++;
                    if (primeCount == count) {
                        return num;
                    }
                }
                num += 2;
            } while (true);
        }

        private bool IsPrime(decimal num) {
            if (num % 2 == 0) {
                return false;
            } else if (num == 2) {
                return true;
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
