using Euler_Logic.Helpers;
using System;

namespace Euler_Logic.Problems {
    public class Problem12 : ProblemBase {
        public override string ProblemName {
            get { return "12: Highly divisible triangular number"; }
        }

        public override string GetAnswer() {
            return Solve().ToString();
        }

        private ulong Solve() {
            ulong num = 3;
            ulong next = 2;
            var primes = new PrimeSieve(50000);
            do {
                var count = DivisorCount(num, primes);
                if (count > 500) return num;
                next++;
                num += next;
            } while (true);
        }

        private int DivisorCount(ulong num, PrimeSieve primes) {
            int total = 1;
            ulong max = (ulong)Math.Sqrt(num);
            foreach (var prime in primes.Enumerate) {
                if (prime > max) break;
                if (num % prime == 0) {
                    int count = 1;
                    do {
                        num /= prime;
                        count++;
                    } while (num % prime == 0);
                    total *= count;
                }
            }
            return total;
        }
    }
}
