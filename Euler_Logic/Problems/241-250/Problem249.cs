using Euler_Logic.Helpers;

namespace Euler_Logic.Problems {
    public class Problem249 : ProblemBase {

        /*
            This problem is almost the same as counting the ways to make change for a dollar. The differences are (1) instead of coins
            of sizes 1/5/10/25/50/100, you have prime numbers of sizes 2/3/5/7 etc, and (2) while you can use a coin as many times
            as you want, here you can only use a prime number once.

            First find the boundary for our primes after the sum. This is easy, just sum all primes up to 4999. Then solve using
            dynamic programming. Basically the total number of ways to add prime numbers up to (p) to equal number (n) is equal to
            the total number of ways to add prime numbers up to p - 1 to equal number (n), plus prime numbers up to p - 1 to equal
            number n - p. For example, the total ways of summing prime numbers to equal 17 using prime numbers up to 11 is equal
            to the total ways of summing prime numbers to equal 17 using prime numbers up to 7 plus summing prime numbers to equal
            17 - 11 using prime numbers up to 7. Only need two arrays for this, (p) and p - 1. Once that's finished, just sum
            the answers for only prime numbers up to our max boundary.
         */

        public override string ProblemName {
            get { return "249: Prime Subset Sums"; }
        }

        public override string GetAnswer() {
            return Solve(4999).ToString();
        }

        private ulong Solve(ulong max) {
            ulong mod = 10000000000000000;
            var top = GetSumOfAllPrimes(max);
            var primes = new PrimeSieve(max);
            var sums = new ulong[2][];
            sums[0] = new ulong[top];
            sums[1] = new ulong[top];
            ulong maxTop = 0;
            foreach (var prime in primes.Enumerate) {
                maxTop += prime;
                sums[0] = sums[1];
                sums[1] = new ulong[top];
                for (ulong subMax = 1; subMax <= maxTop; subMax++) {
                    if (subMax == prime) {
                        sums[1][subMax - 1] += 1;
                    } 
                    if (subMax > prime) {
                        sums[1][subMax - 1] += sums[0][subMax - prime - 1] + sums[0][subMax - 1];
                    } else {
                        sums[1][subMax - 1] += sums[0][subMax - 1];
                    }
                    sums[1][subMax - 1] = sums[1][subMax - 1] % mod;
                }
            }
            var subPrimes = new PrimeSieve(top);
            ulong count = 0;
            foreach (var prime in subPrimes.Enumerate) {
                count = (count + sums[1][prime - 1]) % mod;
            }
            return count;
        }

        private ulong GetSumOfAllPrimes(ulong max) {
            ulong sum = 0;
            var primes = new PrimeSieve(max);
            foreach (var prime in primes.Enumerate) {
                sum += prime;
            }
            return sum;
        }
    }
}
