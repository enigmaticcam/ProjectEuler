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
            ulong test = (ulong)Math.Pow(10, 9);
            foreach (var prime in _primes.Enumerate) {
                if (test % prime == 1) {
                    bool stop = true;
                }
            }
            return 0;
        }
    }
}
