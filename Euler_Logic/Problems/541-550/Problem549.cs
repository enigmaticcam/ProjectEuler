using Euler_Logic.Helpers;
using System.Collections.Generic;

namespace Euler_Logic.Problems {

    public class Problem549 : ProblemBase {
        private PrimeSieveWithPrimeListULong _primes = new PrimeSieveWithPrimeListULong();
        private Dictionary<ulong, ulong> _bestPrimeFactors = new Dictionary<ulong, ulong>();

        /*
            The lowest factorial for any number n is the highest of the lowest factorials for the prime factors of n. So for example, the prime factors
            of 120 are 2^3, 3^1, and 5^1. The lowest factorial for 8, 3, and 5 - whichever is the highest is the highest for 120.

            So first we prime sieve all numbers 2 to 10^8. Then we find the lowest factorial for all primes and their powers. This can be done
            by first starting with the lowest factorial for a prime, which is always that prime, and then instead of looping from there
            to (p + 1)!, (p + 2)! etc., we loop (p * 2)!, (p * 3)!, etc. Finally, we factorize all numbers to 10^8 and for each number
            return the highest of the lowest factorial for all its prime factors.

            Not very efficient, but it gets the job done in about 1-2 minutes.
         */

        public override string ProblemName {
            get { return "549: Divisibility of factorials"; }
        }

        public override string GetAnswer() {
            ulong max = 100000000;
            _primes.SievePrimes(max);
            FindBestPrimeFactors(max);
            return Solve(max).ToString();
        }

        private void FindBestPrimeFactors(ulong max) {
            foreach (var prime in _primes.Enumerate) {
                _bestPrimeFactors.Add(prime, prime);
                ulong power = prime * prime;
                for (int exponent = 2; power <= max; exponent++) {
                    ulong factorial = prime;
                    ulong result = prime;
                    do {
                        result += prime;
                        factorial *= result;
                        factorial %= power;
                    } while (factorial % power != 0);
                    _bestPrimeFactors.Add(power, result);
                    power *= prime;
                }
            }
        }

        private ulong Solve(ulong max) {
            ulong sum = 0;
            for (ulong num = 2; num <= max; num++) {
                sum += Factorization(num);
            }
            return sum;
        }

        private ulong Factorization(ulong n) {
            if (_primes.IsPrime(n)) {
                return n;
            }
            ulong highest = 0;
            foreach (var prime in _primes.Enumerate) {

                if (n % prime == 0) {
                    ulong factor = 1;
                    do {
                        n /= prime;
                        factor *= prime;
                    } while (n % prime == 0);
                    ulong thisHighest = _bestPrimeFactors[factor];
                    highest = (thisHighest > highest ? thisHighest : highest);
                    if (_primes.IsPrime(n)) {
                        highest = (n > highest ? n : highest);
                        break;
                    }
                    if (n == 1) {
                        break;
                    }
                }
            }
            return highest;
        }
    }
}