using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Logic.Problems {
    public class Problem132 : ProblemBase {
        private PrimeSieveWithPrimeListULong _primes = new PrimeSieveWithPrimeListULong();

        public override string ProblemName {
            get { return "132: Large repunit factors"; }
        }

        public override string GetAnswer() {
            _primes.SievePrimes(1000000);
            return Solve().ToString();
        }

        private ulong Solve() {
            ulong sum = 0;
            int count = 0;
            foreach (var prime in _primes.Enumerate) {
                if ((prime - 1) % 10 == 0) {
                    sum += prime;
                    if (count == 40) {
                        break;
                    }
                }
            }
            return sum;
        }
    }
}
