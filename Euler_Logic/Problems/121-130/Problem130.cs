using Euler_Logic.Helpers;

namespace Euler_Logic.Problems {
    public class Problem130 : ProblemBase {
        private PrimeSieveWithPrimeListULong _primes = new PrimeSieveWithPrimeListULong();

        /*
            For any number n, a simple solution is to start with 1, then multiply by 10,
            add 1, and return the remainder when divded by n. If the remainder is 0, you
            are done. If it is not, then do it again this time starting with the remainder
            (instead of 1). Continue to do this until the remainder is 0.

            Sieve primes up to some limit. I hoped all primes up to 1000000 would be enough.
            Then starting at 3(n), where n is not a prime and is not divisible by 2 or 5,
            check to see if the repunit of n is divisible by n - 1. Keep doing this until we
            find the first 25 instances.
        */

        public override string ProblemName {
            get { return "130: Composites with prime repunit property"; }
        }

        public override string GetAnswer() {
            _primes.SievePrimes(1000000);
            return Solve(25).ToString();
        }

        private ulong Solve(int count) {
            ulong num = 3;
            int found = 0;
            ulong sum = 0;
            do {
                if (num % 5 != 0 && !_primes.IsPrime(num)) {
                    var repunit = GetRepunit(num);
                    if ((num - 1) % repunit == 0) {
                        found += 1;
                        sum += num;
                    }
                }
                num += 2;
            } while (found < count);
            return sum;
        }

        private ulong GetRepunit(ulong num) {
            ulong remainder = 1;
            ulong count = 1;
            do {
                remainder = ((remainder * 10) + 1) % num;
                count++;
            } while (remainder != 0);
            return count;
        }
    }
}
