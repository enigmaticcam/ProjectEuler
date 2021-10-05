using Euler_Logic.Helpers;
using System;
using System.Collections.Generic;

namespace Euler_Logic.Problems {
    public class Problem131 : ProblemBase {
        private PrimeSieveWithPrimeListDecimal _primes = new PrimeSieveWithPrimeListDecimal();
        private HashSet<decimal> _powersOfThree = new HashSet<decimal>();

        /*
            If you blow out the first few answers using a brute force algorithm, it can be seen that n will
            always be a power of 3. For example, the first four prime numbers are 7, 19, 37, and 61, and
            n for each are 1 (1^3), 8 (2^3), 27 (3^3), and 64 (4^3). It can also be seen that n steadily 
            increases as the prime number increases (though not with a predictable pattern).

            After looking at the first dozen answers, I was finally able to see that:
            if a = root of n, or n^(1/3)
            then a + p = (a + 1)^3
            
            So then, for each prime (p), all I need to do is find (a) where log(a + P, base a + 1) = 3.
            Since n steadily increases, for each prime you can start from the last n. Also, you can stop
            when log(a + P, base a + 1) < 3. Do this for all primes under 1000000.
         */

        public override string ProblemName {
            get { return "131: Prime cube partnership"; }
        }

        public override string GetAnswer() {
            _primes.SievePrimes(1000000);
            return Solve().ToString();
        }

        private int Solve() {
            decimal x = 0;
            int primeIndex = 0;
            int count = 0;
            do {
                decimal prime = _primes[primeIndex];
                decimal a = 0;
                decimal subX = x;
                do {
                    subX++;
                    a = prime + (subX * subX * subX);
                    if ((decimal)Math.Pow((double)subX + 1, 3) == a) {
                        x = subX;
                        count++;
                    }
                } while ((decimal)Math.Log((double)a, (double)subX + 1) >= 3);
                primeIndex++;
            } while (primeIndex < _primes.Count);
            return count;
        }
    }
}
