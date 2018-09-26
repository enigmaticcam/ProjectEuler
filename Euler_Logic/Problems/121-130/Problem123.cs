using Euler_Logic.Helpers;
using System;

namespace Euler_Logic.Problems {
    public class Problem123 : ProblemBase {
        private PrimeSieveWithPrimeListULong _primes = new PrimeSieveWithPrimeListULong();

        /*
            Obviously as the index of the prime grows, it will very quickly surpass a
            64-bit number. So instead of calculating (p(n)-1)^n where you multiply
            (p(n)-1)*(p(n)-1).., instead do ((p(n)-1)%p(n)^2)*((p(n)-1)%p(n)^2)...
            Do the same for (p(n)+1), add the two numbers, and return the remainder.
         */

        public override string ProblemName {
            get { return "123: Prime square remainders"; }
        }

        public override string GetAnswer() {
            _primes.SievePrimes(1000000);
            ulong max = (ulong)Math.Pow(10, 10);
            return Solve(max).ToString();
        }

        private int Solve(ulong max) {
            for (int index = 0; index < _primes.Count; index++) {
                var result = GetRemainder(_primes[index], index + 1);
                if (result > max) {
                    return index + 1;
                }
            }
            return 0;
        }

        private ulong GetRemainder(ulong prime, int index) {            
            ulong squared = prime * prime;
            ulong a = (prime - 1) % squared;
            ulong b = (prime + 1) % squared;
            for (int count = 1; count < index; count++) {
                a = (a * (prime - 1)) % squared;
                b = (b * (prime + 1)) % squared;
            }
            return (a + b) % squared;
        }
    }
}
